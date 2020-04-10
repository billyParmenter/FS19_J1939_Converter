#include "../inc/sockCANFunc.h"

char delim[] = " ";

void *socCANBroadcast(void *recvMsg)
{
   int errorCode = THREAD_SUCCESS;
    int broadcastSocket; //Endpoint for communication
    struct ifreq ifr;
    struct canfd_frame frame;
    unsigned char data[CANFD_MAX_DLEN];
   	char* logMessage = (char*)malloc(sizeof(char*) * LRG_BUFSIZ);

    canid_t canID; //Variable used to store the CAN ID from the incoming message
    if((broadcastSocket = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < CAN_ERROR) 
    {
        errorCode = CAN_ERROR;
        sprintf(logMessage, "Error: Could not create socket over CAN network");
        Log(FATAL, logMessage);
        pthread_exit((void*)&errorCode);
        
    }
    strcpy(ifr.ifr_name, "vcan0" );
    ioctl(broadcastSocket, SIOCGIFINDEX, &ifr);

    struct sockaddr_can addr;
    memset(&addr, 0, sizeof(addr));
    addr.can_family = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;

    setsockopt(broadcastSocket, SOL_CAN_RAW, CAN_RAW_FILTER, NULL, 0);
    frame.len = 8;
    int todo_broadcast = 1;
	setsockopt(broadcastSocket, SOL_SOCKET, SO_BROADCAST, &todo_broadcast, sizeof(todo_broadcast));


    if (bind(broadcastSocket, (struct sockaddr *)&addr, sizeof(addr)) < CAN_ERROR) {
        errorCode = CAN_ERROR;
        sprintf(logMessage, "Error, could not bind socket over CAN network");
        Log(FATAL, logMessage);
        pthread_exit((void*)&errorCode);

    }
    
    //Parsing to get the CAN ID from the incoming message
    char *msgPtr = strtok((char*)recvMsg, delim);
    canID = strtoul(msgPtr, NULL, 16);
    // Log(DEBUG, msgPtr);
    

    frame.can_id = canID;

    int mtu = CAN_MTU;
    int maxdlen = CAN_MAX_DLEN;

    frame.can_id &= CAN_EFF_MASK;
    frame.can_id |= CAN_EFF_FLAG;


    
    //Parsing to get the first 8 bits of the actual data.
    msgPtr = strtok(NULL, delim);

    int sizeCheck = getSize(msgPtr);

    // for (int j = 0; j <= sizeCheck/2 - 1; ++j)
    // {
    //     frame.data[j] = msgPtr[j];
    // }
    hexstring2data(msgPtr, data, CANFD_MAX_DLEN);
    memcpy(frame.data, data, CANFD_MAX_DLEN);

    // char buff[BUFSIZ];
    
    // int i;
    // for (i = 0; i < 8; i++)
    // {
    //     if (i > 0) printf(":");
    //     printf("%02X", frame.data[i]);
    // }
    // printf("\n");


    if (write(broadcastSocket, &frame, mtu) < CAN_ERROR) {
        errorCode = CAN_ERROR;
        sprintf(logMessage, "Error: Could not write over CAN network");
        Log(FATAL, logMessage);
        close(broadcastSocket);
        pthread_exit((void*)&errorCode);
    }
    
    if (close(broadcastSocket) < 0) {
        errorCode = CAN_ERROR;
        sprintf(logMessage, "Error, could not close socket over CAN network");
        Log(FATAL, logMessage);
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

int hexstring2data(char *arg, unsigned char *data, int maxdlen) {

	int len = strlen(arg);
	int i;
	unsigned char tmp;

	if (!len || len%2 || len > maxdlen*2)
		return 1;

	memset(data, 0, maxdlen);

	for (i=0; i < len/2; i++) {

		tmp = asciiToNibble(*(arg+(2*i)));
		if (tmp > 0x0F)
			return 1;

		data[i] = (tmp << 4);

		tmp = asciiToNibble(*(arg+(2*i)+1));
		if (tmp > 0x0F)
			return 1;

		data[i] |= tmp;
	}

	return 0;
}