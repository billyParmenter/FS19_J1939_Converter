#include "../inc/sockCANFunc.h"

char delim[] = " ";

void *socCANBroadcast(void *recvMsg)
{
    int broadcastSocket; //Endpoint for communication
    unsigned int canID;

    char *msgPtr = strtok((char*)recvMsg, delim);
    canID = strtoul(msgPtr, NULL, 10);

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

    struct can_frame frame;
    //creating socketCAN Data
    printf("Creating Data\n");
    frame.can_id = canID;
    frame.can_dlc = 8;
    //Parsing to get the first 8 bits of the actual data.
    msgPtr = strtok(NULL, delim);
    int sizeCheck = getSize(msgPtr);
    for (int j = 0; j <= sizeCheck/2 - 1; ++j)
    {
        frame.data[j] = msgPtr[j];
    }


    if (write(broadcastSocket, &frame, sizeof(struct can_frame)) != sizeof(struct can_frame)) {
        printf("Error, could not write over CAN network");
    }
    //In case there is more data, write another one
    if(sizeCheck > 8)
    {
        int originalIndex = 0;
        for (int j = 8; msgPtr[j] != '\0'; ++j)
        {    
            frame.data[originalIndex] = msgPtr[j];
            originalIndex++;
        }
        if (write(broadcastSocket, &frame, sizeof(struct can_frame)) != sizeof(struct can_frame)) {
            printf("Error, could not write over CAN network");
        }   
    }

    pthread_exit(NULL);
}


//From https://stackoverflow.com/questions/48367022/c-iterate-through-char-array-with-a-pointer
int getSize (char * s) {
    char * t; // first copy the pointer to not change the original
    int size = 0;

    for (t = s; *t != '\0'; t++) {
        size++;
    }

    return size;
}