/*
* FILE          : socketCANFunc.c
* PROJECT       : CAN Relay
* PROGRAMMER    : Oloruntoba Samuel Lagunju
* DATE          : April 6th 2020
* DESCRIPTION   : Contains methods that will read or write to the socketCAN network
                    Also contains methods that aid in these functionalities
*/

#include "../inc/sockCANFunc.h"

char delim[] = " ";

/*
* Function      : socCANBroadcast
* Parameters    : void *recvMsg
* Returns       : void* &errorCode
* Description   : Will connect to socketCAN network and write a message to it
*/

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
    frame.can_id = canID;

    int mtu = CAN_MTU;

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


/*
* Function      : socCANRead
* Parameters    : void *ipArg
* Returns       : void* &errorCode
* Description   : Will connect to socketCAN network and read a message from it
*/
void *socCANRead(void* ipArg)
{
    int errorCode = THREAD_SUCCESS;
    bool keepRunning = true;
    char* ipToDB = (char*)ipArg;
    int readingSocket; //Endpoint for communication
    char readMsgBuffer[BUFSIZ];

    char* formattedMessage = (char*)malloc(sizeof(readMsgBuffer)+ SML_BUFFSIZ);
   	char* logMessage = (char*)malloc(sizeof(char*) * LRG_BUFSIZ);


    if((readingSocket = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0) 
    {
        errorCode = CAN_ERROR;
        sprintf(logMessage, "Error, could not create socket over CAN network");
        Log(FATAL, logMessage);
        pthread_exit((void*)&errorCode);
    }

    struct ifreq ifr;
    strcpy(ifr.ifr_name, "vcan0");
    ioctl(readingSocket, SIOCGIFINDEX, &ifr);

    struct sockaddr_can addr;
    memset(&addr, 0, sizeof(addr));
    addr.can_family = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;
    if (bind(readingSocket, (struct sockaddr *)&addr, sizeof(addr)) < 0) {
        errorCode = CAN_ERROR;
        sprintf(logMessage, "Error, could not bind socket over CAN network");
        Log(FATAL, logMessage);
        pthread_exit((void*)&errorCode);

    }

    struct canfd_frame frame;    
    int numOfBytesRead;
    while(keepRunning)
    {                   
        sprintf(logMessage, "Client is reading  from socketCAN network");
		Log(INFO, logMessage);

        numOfBytesRead = read(readingSocket, &frame, sizeof(struct can_frame));

        if (numOfBytesRead < 0) {
            errorCode = CAN_ERROR;
            sprintf(logMessage, "Error, could not read over CAN network");
            Log(FATAL, logMessage);
            keepRunning = false;
        }
    
        //Getting CAN ID of message
        sprint_canframe(readMsgBuffer, &frame, 0, 8);
        sprintf(formattedMessage, "%s", readMsgBuffer);

        sprintf(logMessage, "socketCAN Client recieved %s", formattedMessage);
        Log(INFO, logMessage);
        formattedMessage[strlen(formattedMessage)+1] = '\0';


        if(socketToDB(ipToDB, readMsgBuffer) == SOCKET_ERROR)
        {errorCode = SOCKET_ERROR, keepRunning = false;}
        else{
            sprintf(logMessage, "Sent %s to Dashboard", readMsgBuffer);
            Log(INFO, logMessage);
        }
        memset(readMsgBuffer, 0, sizeof (readMsgBuffer));
    }

    if (close(readingSocket) < 0) {
        sprintf(logMessage, "Error, could not close socket over CAN network");
        Log(FATAL, logMessage);
        errorCode = CAN_ERROR; 
    }
    
    pthread_exit((void*)&errorCode);
}

/*
* Function      : socketToDB
* Parameters    : char* ipAddress, char* messageToBeSent
* Returns       : SOCKET_ERROR or OK_SIG
* Description   : Will connect to dashboard and send message to it
*/
int socketToDB(char* ipAddress, char* messageToBeSent)
{
    int successfulConn = OK_SIG;
	int sockfd; 
    struct sockaddr_in servaddr; 
  
    // socket create and varification 
    sockfd = socket(AF_INET, SOCK_STREAM, 0); 
    if (sockfd == -1) { 
        sprintf(logMessage, "Error, Socket creation failed");
        Log(FATAL, logMessage);
        return SOCKET_ERROR;
    } 
    bzero(&servaddr, sizeof(servaddr)); 
  
    // assign IP, PORT 
    servaddr.sin_family = AF_INET; 
    servaddr.sin_addr.s_addr = inet_addr(ipAddress); 
    servaddr.sin_port = htons(DEFAULT_DB_PORT); 
  
    //Connect the client socket to server socket 
    if (connect(sockfd, (SA*)&servaddr, sizeof(servaddr)) != 0) { 
        sprintf(logMessage, "Error, connection with the server failed...");
        Log(FATAL, logMessage);
        return SOCKET_ERROR;
    }


    //Sending Message
    if(write(sockfd, messageToBeSent, strlen(messageToBeSent)) < 0){
        sprintf(logMessage, "Error, Write to server failed...");
        Log(FATAL, logMessage);
        return SOCKET_ERROR;
    } 

    // close the socket 
    close(sockfd); 
    return successfulConn;
}

/*
* Function      :   data2hexstring
* Parameters    :   unsigned char *data
* Returns       :   char* string
* Description   :   Will convert data in socketCAN frame to hex string
* Source        :   From CAN-UTILS: https://github.com/linux-can/can-utils
*/
char* data2hexstring(unsigned char *data)
{
    char *string = (char*) malloc(sizeof data *2 + 1);

    for (size_t i = 0; i < sizeof data; i++)
    {
        sprintf(string + i * 2, "%02x", data[i]);
    }

    return string;
}

/*
* Function      :   asciiToNibble
* Parameters    :   char canidChar
* Returns       :   unsigned char
* Description   :   Will convert a char to a nibble data format
* Source        :   From CAN-UTILS: https://github.com/linux-can/can-utils
*/
unsigned char asciiToNibble(char canidChar) {
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

/*
* Function      :   hexstring2data
* Parameters    :   char *arg, unsigned char *data, int maxdlen
* Returns       :   1 or 0 
* Description   :   Will convert a string to a socketCAN frame format
* Source        :   From CAN-UTILS: https://github.com/linux-can/can-utils
*/
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

/*
* Function      :   sprint_canframe
* Parameters    :   char *buf , struct canfd_frame *cf, int sep, int maxdlen
* Returns       :   void
* Description   :   Copies content from a buffer to the socketCAN frame
* Source        :   From CAN-UTILS: https://github.com/linux-can/can-utils
*/
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