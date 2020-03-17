#include "../inc/SocketFunc.h"

bool SocketSetup(int portNumber)
{
	bool successfulConn = SOCKET_SUCCESS; //Variable used for error checking
    int sockfd, newsockfd; //Socket variable in which the system registers onSS
    char buffer[256];
    socklen_t clilen;
    struct sockaddr_in serv_addr, cli_addr; //Struct containing an internet address.

    sockfd = socket(AF_INET, SOCK_STREAM, 0);
    if (sockfd < 0)
	{
        successfulConn = SOCKET_ERROR;
        //perror("ERROR opening socket");
        printf("ERROR opening socket\n");

	}

    //Setting all values in a buffer to zero.
    bzero((char *) &serv_addr, sizeof(serv_addr));
    serv_addr.sin_family = AF_INET;
    serv_addr.sin_addr.s_addr = INADDR_ANY;
    //Converts a port number in host byte order to a port number in network byte order.
    serv_addr.sin_port = htons(portNumber); 

    if(bind(sockfd, (struct sockaddr *) &serv_addr, sizeof(serv_addr)) < 0) 
	{
        successfulConn = SOCKET_ERROR;
		perror("ERROR on binding");
		//printf("ERROR on binding\n");
	}

	listen(sockfd,10);
	while(((strcmp(buffer, "stop") != 0) || successfulConn != SOCKET_ERROR))
	{	

		clilen = sizeof(cli_addr);
		printf("Waiting for a message...\n");
		newsockfd = accept(sockfd, (struct sockaddr *) &cli_addr, &clilen);

		if (newsockfd < 0) 
		{        
            successfulConn = SOCKET_ERROR;
		    //perror("ERROR on accept");
		    printf("ERROR on accept\n");
		}    
		
		bzero(buffer,256);
		if(read(newsockfd,buffer,255) < 0) 
		{
            successfulConn = SOCKET_ERROR;
		    printf("ERROR reading from socket\n");
			//perror("ERROR reading from socket");
		}

		close(newsockfd);
		//Create broadcast thread here
		if(successfulConn)
		{
			printf("Recieved a message:- %s\n",buffer);
			pthread_t broadcastThread;
			void* resultFromThread;
			if(pthread_create(&broadcastThread, NULL, socCANBroadcast, (void*) buffer)< 0) 
			{	printf("Error: Cannot Write To SocketCAN\n");}
			pthread_join(broadcastThread, &resultFromThread); 
		}
	}

    return successfulConn;
}


