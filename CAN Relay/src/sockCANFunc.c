#include "../inc/sockCANFunc.h"

char delim[] = " ";

void *socCANBroadcast(void *recvMsg)
{
    int broadcastSocket; //Endpoint for communication
    struct incomingCANMsg CANMsg; //CAN message struct used for broadcasting incoming data
    //Parsing Data
    printf("Parsing Data\n");
    parseIntoCANMessage((char*) recvMsg, &CANMsg);
    printf("Creating socketCAN\n");
    if((broadcastSocket = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0) 
    {
        printf("Error, could not create socket over CAN network");
    }
    struct ifreq ifr;
    strcpy(ifr.ifr_name, "vcan0" );
    ioctl(broadcastSocket, SIOCGIFINDEX, &ifr);

    struct sockaddr_can addr;
    memset(&addr, 0, sizeof(addr));
    addr.can_family = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;
    if (bind(broadcastSocket, (struct sockaddr *)&addr, sizeof(addr)) < 0) {
        printf("Error, could not bind socket over CAN network");
    }
    // unsigned int canIDBuffer =  strtoul(payload, NULL, 10);
    // printf("%d\n", canIDBuffer);
    struct can_frame frame;
    // frame.can_id = 0x18FE00F0;
    frame.can_id = 0x18FE00F0;
    frame.can_dlc = 5;
    // for (int j = 0; j < sizeCheck; ++j)
    // {
    //     frame.data[j] = ptr[j];
    // }
    if (write(broadcastSocket, &frame, sizeof(struct can_frame)) != sizeof(struct can_frame)) {
        printf("Error, could not write over CAN network");
    }
    pthread_exit(NULL);
}

int parseIntoCANMessage(char* recvMessage, struct incomingCANMsg *formattedMsg)
{
    //Parsing the first bit of the message to get CAN ID
    char *msgPtr = strtok(recvMessage, delim);
    formattedMsg->canID = strtoul(msgPtr, NULL, 10);
    //Formatting to get the first 8 bits of the actual data.
    msgPtr = strtok(NULL, delim);
    memcpy(formattedMsg->firstDataBit, msgPtr,8);
    int sizeCheck = strlen(msgPtr);
    if(sizeCheck > 8)
    {   
        sprintf(formattedMsg->secondDataBit, "%s\n", msgPtr);
    }
    printf("firstDataBit:-%s\n", formattedMsg->firstDataBit);
    printf("secondDataBit:-%s\n", formattedMsg->secondDataBit);
    printf("Size: %d\n", sizeCheck);
    return true;
}