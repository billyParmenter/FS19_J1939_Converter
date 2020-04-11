#include "../inc/SocketFunc.h"


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
	int serverPort = *(int*)args;
	pthread_t newClientThread;	//thread to continue communication with the server
	char* client_message = (char*)malloc(sizeof(char*) * LRG_BUFSIZ);
	char* logMessage = (char*)malloc(sizeof(char*) * LRG_BUFSIZ);

	//Creating the socket. 
	serverSocket = socket(AF_INET , SOCK_STREAM, DEFAULT_INTERNET_PROTOCOL);
	if(serverSocket < 0)	//If serverSocket cannot be created return an error
	{
		sprintf(logMessage, "%s","Failed to create server socket.");
		Log(FATAL, logMessage);
		pthread_exit(SOCKET_ERROR);
	}
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
		sprintf(logMessage, "%s","Failed to bind server socket.");
		Log(FATAL, logMessage);
		pthread_exit(SOCKET_ERROR);
	}
	//Listen on the socket, with  32 max connection requests queued 
	if(listen(serverSocket, 32) == 0 ) 
	{
		sprintf(logMessage, "Server is listening on port: %d", serverPort);
		Log(INFO, logMessage);
		addr_size = sizeof(serverStorage);

	}
	else
	{
		sprintf(logMessage, "Failed to listen over socket");
		Log(FATAL, logMessage);
		pthread_exit(SOCKET_ERROR);
	}

	//Loop to keep the server running
	newSocket = accept(serverSocket, (struct sockaddr *) &serverStorage, &addr_size);
	while(true)
	{
		//Accept call creates a new socket for the incoming connection
		if (newSocket < SOCKET_ERROR){
			sprintf(logMessage, "Failed to accept connection over socket");
	  		Log(FATAL, logMessage);
			pthread_exit(SOCKET_ERROR);   
		} 
		else
		{
			//for each client request creates a thread and assign the client request to it to process
       		//so the main thread can entertain next request
			if(recv(newSocket , client_message , LRG_BUFSIZ , 0) < SOCKET_ERROR)
			{
				sprintf(logMessage, "Error code from recv function: %d", errno);
				Log(FATAL, logMessage);
				//Cleanup here
				pthread_exit(SOCKET_ERROR);   

			}
			sprintf(logMessage, "Server recieved:- %s", client_message);
			Log(INFO, logMessage);
			void* resultFromThread;
			if(pthread_create(&newClientThread, NULL, socCANBroadcast, (void*) client_message)< THREAD_ERROR){

				sprintf(logMessage, "Error: Cannot Write To SocketCAN"); 
				Log(FATAL, logMessage);
				pthread_exit(SOCKET_ERROR); 

			}
			pthread_join(newClientThread, &resultFromThread); 
		}
		
	}
	
	pthread_exit(NULL);
}

