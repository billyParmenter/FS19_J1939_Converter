#include "../inc/sockCANFunc.h"

char delim[] = " ";

void *socCANBroadcast(void *recvMsg)
{
    int errorCode = THREAD_SUCCESS;
    int broadcastSocket; //Endpoint for communication
    struct ifreq ifr;
    struct can_frame frame;
    canid_t canID; //Variable used to store the CAN ID from the incoming message
    printf("Creating socketCAN to Broadcast\n"); //Implement logger here
    if((broadcastSocket = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0) 
    {
        errorCode = CAN_ERROR;
        printf("Error: Could not create socket over CAN network\n");
        pthread_exit((void*)&errorCode);
        
    }
    strcpy(ifr.ifr_name, "vcan0" );
    ioctl(broadcastSocket, SIOCGIFINDEX, &ifr);

    struct sockaddr_can addr;
    memset(&addr, 0, sizeof(addr));
    addr.can_family = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;
    if (bind(broadcastSocket, (struct sockaddr *)&addr, sizeof(addr)) < 0) {
        printf("Error, could not bind socket over CAN network");
    }
    
    //Parsing to get the CAN ID from the incoming message
    char *msgPtr = strtok((char*)recvMsg, delim);
    canID = strtoul(msgPtr, NULL, 16);
    frame.can_id = canID;
    //Parsing to get the first 8 bits of the actual data.
    msgPtr = strtok(NULL, delim);
    int sizeCheck = getSize(msgPtr);

    for (int j = 0; j <= sizeCheck/2 - 1; ++j)
    {
        frame.data[j] = msgPtr[j];
    }

    if (write(broadcastSocket, &frame, sizeof(frame)) != sizeof(frame)) {
        printf("Error: Could not write over CAN network");
    }
    
    if (close(broadcastSocket) < 0) {
        printf("Error, could not close socket over CAN network");
        errorCode = CAN_ERROR;
        pthread_exit((void*)&errorCode);
    }

    pthread_exit((void*)&errorCode);
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

unsigned char asciiToNibble(char canidChar) {
    unsigned char convertedCharacter;
    //Converting to the decimal value of a given ASCII hex character.
    if ((canidChar >= '0') && (canidChar <= '9'))
    {   
        return canidChar - '0';   
    }

    else if ((canidChar >= 'A') && (canidChar <= 'F'))
    {   
        return canidChar - 'A' + 10; 
    }

    else if ((canidChar >= 'a') && (canidChar <= 'f'))
    {   
        return canidChar - 'a' + 10;  
    }
    return 16;
}


void *socCANRead(void* ipToDashboard)
{
    int errorCode = THREAD_SUCCESS;
    bool keepRunning = true;
    struct timeval tv;

    tv.tv_sec = 45; //5 seconds
    tv.tv_usec = 0;
    int readingSocket; //Endpoint for communication

    if((readingSocket = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0) 
    {
        errorCode = CAN_ERROR;
        printf("Error, could not create socket over CAN network\n");
        pthread_exit((void*)&errorCode);
    }

    // setsockopt(readingSocket, SOL_SOCKET, SO_RCVTIMEO, (const char*)&tv, sizeof tv);

    struct ifreq ifr;
    strcpy(ifr.ifr_name, "vcan0");
    ioctl(readingSocket, SIOCGIFINDEX, &ifr);

    struct sockaddr_can addr;
    memset(&addr, 0, sizeof(addr));
    addr.can_family = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;
    if (bind(readingSocket, (struct sockaddr *)&addr, sizeof(addr)) < 0) {
        printf("Error, could not bind socket over CAN network\n");
    }

    struct can_frame frame;
    int numOfBytesRead;
    char readMsgBuffer[16];
    char msgToDB[32];
    while(keepRunning)
    {                   
        printf("Reading from SocketCAN network...\n"); //Implement logger here

        numOfBytesRead = read(readingSocket, &frame, sizeof(struct can_frame));

        // Reads with timeout if uncommented.
        // if (numOfBytesRead == EWOULDBLOCK || EAGAIN) {
        //     printf("Reading Timeout\n");
        //     errorCode = CAN_ERROR;
        // }

        if (numOfBytesRead < 0) {
            printf("Error, could not read over CAN network\n");
            keepRunning = false;
        }

        for (int i = 0; i < frame.can_dlc; i++)
        {
            readMsgBuffer[i] = frame.data[i];  //Try to acquire the data here
        }

        sprintf(msgToDB, "%03X %s", frame.can_id, readMsgBuffer);
        // printf("CAN ID:0x%03X\n", frame.can_id);
         
        printf("Message: %s\n", msgToDB);
        // int sizeCheck = getSize(msgToDB);
        // //Format message in [CAN ID][16 Bits of Data]
        // if(sizeCheck < 16)
        // {
        //     strcpy(msgToDB, readMsgBuffer);
        // }
        // else
        // {
        //     printf("Message to Dashboard: %s\n", readMsgBuffer);
        //     socketToDB(readMsgBuffer);   
        // }

    }

    if (close(readingSocket) < 0) {
        printf("Error, could not close socket over CAN network\n");
        errorCode = CAN_ERROR;
    }
    
    pthread_exit((void*)&errorCode);
}

void socketToDB(char* messageToBeSent)
{
	int sockfd; 
    struct sockaddr_in servaddr; 
  
    // socket create and varification 
    sockfd = socket(AF_INET, SOCK_STREAM, 0); 
    if (sockfd == -1) { 
        printf("socket creation failed...\n"); 
        exit(0); 
    } 
    bzero(&servaddr, sizeof(servaddr)); 
  
    // assign IP, PORT 
    servaddr.sin_family = AF_INET; 
    servaddr.sin_addr.s_addr = inet_addr("10.192.201.95"); 
    servaddr.sin_port = htons(4040); 
  
    //Connect the client socket to server socket 
    if (connect(sockfd, (SA*)&servaddr, sizeof(servaddr)) != 0) { 
        printf("connection with the server failed...\n"); 
        exit(0); 
    }  
  
    //Sending Message
    write(sockfd, messageToBeSent, sizeof(messageToBeSent));
    printf("Sent message to DB\n");        
    char clear();
   
    // close the socket 
    close(sockfd); 
}

