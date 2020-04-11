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
    hexstring2data(msgPtr, data, CANFD_MAX_DLEN);
    memcpy(frame.data, data, CANFD_MAX_DLEN);


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


void *socCANRead(void* ipArg)
{
    int errorCode = THREAD_SUCCESS;
    bool keepRunning = true;
    struct timeval tv;
    char* ipToDB = (char*)ipArg;
    tv.tv_sec = 45; //5 seconds
    tv.tv_usec = 0;
    int readingSocket; //Endpoint for communication
    char readMsgBuffer[BUFSIZ];

    char* formattedMessage = (char*)malloc(sizeof(readMsgBuffer)+ SML_BUFFSIZ);


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

    struct canfd_frame frame;    
    int numOfBytesRead;
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
    
        //Getting CAN ID of message
        sprint_canframe(readMsgBuffer, &frame, 0, 8);
        sprintf(formattedMessage, "%s", readMsgBuffer);
        formattedMessage[strlen(formattedMessage)+1] = '\0';
        printf("Formatted Message: %s\n", formattedMessage);
        socketToDB(ipToDB, readMsgBuffer);
        memset(readMsgBuffer, 0, sizeof (readMsgBuffer));

    }

    if (close(readingSocket) < 0) {
        printf("Error, could not close socket over CAN network\n");
        errorCode = CAN_ERROR;
    }
    
    pthread_exit((void*)&errorCode);
}

void socketToDB(char* ipAddress, char* messageToBeSent)
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
    servaddr.sin_addr.s_addr = inet_addr(ipAddress); 
    servaddr.sin_port = htons(DEFAULT_DB_PORT); 
  
    //Connect the client socket to server socket 
    if (connect(sockfd, (SA*)&servaddr, sizeof(servaddr)) != 0) { 
        printf("connection with the server failed...\n"); 
        exit(0); 
    }  
  
    //Sending Message
    write(sockfd, messageToBeSent, strlen(messageToBeSent));
    printf("Sent message to DB\n");        
  
    // close the socket 
    close(sockfd); 
}

char* data2hexstring(unsigned char *data)
{
    char *string = (char*) malloc(sizeof data *2 + 1);

    for (size_t i = 0; i < sizeof data; i++)
    {
        sprintf(string + i * 2, "%02x", data[i]);
    }

    return string;
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

void sprint_canframe(char *buf , struct canfd_frame *cf, int sep, int maxdlen) 
{
    /* documentation see lib.h */

    int i,offset;
    int len = (cf->len > maxdlen) ? maxdlen : cf->len;

    if (cf->can_id & CAN_ERR_FLAG) {
        put_eff_id(buf, cf->can_id & (CAN_ERR_MASK|CAN_ERR_FLAG));
        buf[8] = ' ';
        offset = 9;
    } else if (cf->can_id & CAN_EFF_FLAG) {
        put_eff_id(buf, cf->can_id & CAN_EFF_MASK);
        buf[8] = ' ';
        offset = 9;
    } else {
        put_sff_id(buf, cf->can_id & CAN_SFF_MASK);
        buf[3] = ' ';
        offset = 4;
    }

    /* standard CAN frames may have RTR enabled. There are no ERR frames with RTR */
    if (maxdlen == CAN_MAX_DLEN && cf->can_id & CAN_RTR_FLAG) {
        buf[offset++] = 'R';
        /* print a given CAN 2.0B DLC if it's not zero */
        if (cf->len && cf->len <= CAN_MAX_DLC)
            buf[offset++] = hex_asc_upper_lo(cf->len);

        buf[offset] = 0;
        return;
    }

    if (maxdlen == CANFD_MAX_DLEN) {
        /* add CAN FD specific escape char and flags */
        buf[offset++] = ' ';
        buf[offset++] = hex_asc_upper_lo(cf->flags);
        if (sep && len)
            buf[offset++] = '.';
    }

    for (i = 0; i < len; i++) {
        put_hex_byte(buf + offset, cf->data[i]);
        offset += 2;
        if (sep && (i+1 < len))
            buf[offset++] = '.';
    }

    buf[offset] = 0;
}