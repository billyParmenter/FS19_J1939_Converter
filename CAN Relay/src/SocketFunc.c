#include "../inc/SocketFunc.h"


bool ServerFunc()
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
    serv_addr.sin_port = htons(DEFAULT_SERVER_PORT); 

    if(bind(sockfd, (struct sockaddr *) &serv_addr, sizeof(serv_addr)) < 0) 
	{
        successfulConn = SOCKET_ERROR;
		perror("ERROR on binding");
		
		//printf("ERROR on binding\n");
	}
	//Another socket is already listening on the same port.

	if(listen(sockfd, 10) < 0)
	{
        successfulConn = SOCKET_ERROR;
	};
	while(successfulConn != SOCKET_ERROR)
	{	

		printf("Server is listening on port:%d...\n", DEFAULT_SERVER_PORT);
		clilen = sizeof(cli_addr);
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
			{	printf("Error: Cannot Write To SocketCAN\n"); return false; }
			pthread_join(broadcastThread, &resultFromThread); 
		}
	}

    return successfulConn;
}

/*
* Function      : serverThread
* Parameters    : void* args - General pointer passing to the function
* Returns       : SOCKET_ERROR, THREAD_ERROR or NULL
* Description   : Will run a thread that acts as a server for the Converter program.
*/
void* serverThread(void* args)
{
	int serverSocket, newSocket;	//Socket descriptors, an integer (like a file-handle)	
	struct sockaddr_in serverAddr;	//Struct used to defined family/domain, port to listen on for the server and mulitple interfaces
	struct sockaddr_storage serverStorage;	//Struct used to accommodate all supported protocol-specific address structures
	socklen_t addr_size;	//Used to indicate the size of address
	bool stopServer = false;
	int serverPort = *(int*)args;

	//Creating the socket. 
	serverSocket = socket(AF_INET , SOCK_STREAM, 0);
	if(serverSocket < 0)	//If serverSocket cannot be created return an error
	{
		pthread_exit(SOCKET_ERROR);
	}
	char client_message[2000];
	// Configure settings of the server address struct
	// Address family = Internet 
	serverAddr.sin_family = AF_INET;
	//Set port number, using htons function to use proper byte order 

	serverAddr.sin_port = htons(serverPort);
	//Set IP address to localhost 
	serverAddr.sin_addr.s_addr = INADDR_ANY;
	//Set all bits of the padding field to 0 
	memset(serverAddr.sin_zero, '\0', sizeof serverAddr.sin_zero);
	//Bind the address struct to the socket 
	if (bind(serverSocket, (struct sockaddr *) &serverAddr, sizeof(serverAddr)) < 0)
	{ 
		pthread_exit(SOCKET_ERROR);
	}
	//Listen on the socket, with  32 max connection requests queued 
	if(listen(serverSocket, 32) ==0 ) 
	{
		printf("Server is listening on port: %d\n", serverPort);
	}
	else
	{
	   	printf("Error\n");
		pthread_exit(SOCKET_ERROR);
	}

	//Loop to keep the server running
	while(!stopServer)
	{
		//Accept call creates a new socket for the incoming connection
		addr_size = sizeof serverStorage;
		newSocket = accept(serverSocket, (struct sockaddr *) &serverStorage, &addr_size);
		if (newSocket < 0) 
		{  stopServer = true;   } 
		else
		{
			//for each client request creates a thread and assign the client request to it to process
       		//so the main thread can entertain next request
			recv(newSocket , client_message , 2000 , 0);
			printf("Mesage: %s...\n", client_message);
		}
		
	}
	pthread_exit(NULL);
}

