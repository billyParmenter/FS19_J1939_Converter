/*
* Filename      :   main.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the source code for the relayController functions.
*                   These functions create and properly close threads with a signal handler.
*/


/*
* Function      : relayFcnsController
* Parameters    : N/A
* Returns       : N/A
* Description   : Controls and facilitates the CAN relay threads that carry out the Socket and
				  socketCAN functionality
*/
void relayStartup(int opt)
{
	// pthread_t readingCanNtwrkThread; char buffer[256];
	// void* resultFromThread;
	// signal(SIGINT, sigintHandler);



	// printf("socketCAN Transmitter Starting...\n");
	// 			//Creating a seperate thread to read from SocketCAN network
	// 			if(pthread_create(&readingCanNtwrkThread, NULL, socCANRead, NULL) < 0) 
	// 			{	printf("Error: Cannot Read from SocketCAN\n"); }
	// 			pthread_join(readingCanNtwrkThread, &resultFromThread); 
	// 			printf("socketCAN Transmitter Done...\n");
}

bool ArgParsing(int argc, char* argv[])
{
    int opt;	//Variable used to parse the command line argument
	bool serverFlag, transmitterFlag; //Variables used to grant certain CAN relay functionalities
    //Argument parsing
	while ((opt = getopt(argc, argv, optstring)) != -1)
	{
		switch (opt) 
		{
			//Two Modes used to control the relay's behaviour (SEE StartupInfo())
	        case 's':
				serverFlag = true;
				break;
			case 't':
				transmitterFlag = true;
				break;			
			default:
			case '?':
				if (isprint (opt)){
          			startupInfo(opt);
				}
				else {
          			startupInfo(opt);
				}
			break;
		}
	}
    //If either flags are signalled, return true for successfull parsing
    if(serverFlag || transmitterFlag)
    {

    }
}
    