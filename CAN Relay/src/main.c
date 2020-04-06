/*
* Filename      :   main.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the source code for main in the mobile application
*/


#include "../inc/includes.h"

static const char optstring[] = ":if:rjs"; //String containing the legitimate option characters.

int main(int argc, char *argv[])
{
    int opt;
	pthread_t readingCanNtwrkThread; char buffer[256];
	void* resultFromThread;
	// int portNumber = atoi(argv[2]);

    if(argc <= 1)
    {
        startupInfo();
        return ARG_ERROR;
    }

    //Argument parsing
	while ((opt = getopt(argc, argv, optstring)) != -1)
	{
		switch (opt) 
		{
			//Server mode
	        case 's':

				//Creating a seperate thread to read from SocketCAN network
				if(pthread_create(&readingCanNtwrkThread, NULL, socCANRead, NULL) < 0) 
				{	printf("Error: Cannot Read from SocketCAN\n"); }

				//Creating socket server connection
				if(ServerFunc() == SOCKET_ERROR || THREAD_ERROR || PARSE_ERROR)
				{
					printf("Socket Error! Check Logs for more info.\n");
				}
				pthread_join(readingCanNtwrkThread, &resultFromThread); 
				strcpy(buffer, (char*)resultFromThread);
				printf("Result from Thread:-%s\n", buffer);
				break;
			default:
                startupInfo();
				break;
		}
	}
    
    return 0;
}

/*
* Function      : startupInfo
* Parameters    : N/A
* Returns       : Prints messages onto the screen.
* Description   : Displays the necessary commands in order to run the program appropriately
*/
void startupInfo()
{
	system("clear") /*clear output screen*/;
	printf("Invalid usage!\n");
	printf("---How To Use CAN Relay---\n");
    printf(	"Options:\n"
	        " -s \t Run Socket Server\n"
			"	 This will run the program as a server that will broadcast received message onto the socketCAN network\n"
			"-b	 \tRun socketCAN Transmitter\n"
			"	 This will run the program as a client that will transmit messages from the socketCAN network to the Dashboard\n"		
	        "Example:\n"
			//Running the socket server
	        "\tbin/Reader [-s]\n"
			"\tbin/Reader [-b]\n"
			"\tbin/Reader [-s][-b]\n");
}
