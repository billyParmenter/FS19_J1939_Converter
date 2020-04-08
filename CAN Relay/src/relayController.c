/*
* Filename      :   main.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the source code for the relayController functions.
*                   These functions create and properly close threads with a signal handler.
*/

#include "../inc/relayController.h" //Include

static bool serverFlag, transmitterFlag; //Variables used to grant certain CAN relay functionalities

/*
* Function      : relayStartup
* Parameters    : N/A
* Returns       : N/A
* Description   : Will obtain
*/
bool relayStartup()
{
    char userInput;
	bool keepRunning = true; //Variable used to keep main menu loop running
	while(keepRunning)
	{
		//Checking if the user wants to run with defaults or not
		userInput = getchar();

		//Ask user for Port number and IP Address
		if(userInput == 'n')
		{

		}
		else if(userInput == 'y')

	}
	// pthread_t readingCanNtwrkThread; char buffer[256];
	// void* resultFromThread;
	// signal(SIGINT, sigintHandler);

    printf("Starting CAN Relay Server");
    printf("\n Does user want server? %d\n", serverFlag);
    printf("\n Does user want client? %d\n", transmitterFlag);

    return true;
	// printf("socketCAN Transmitter Starting...\n");
	// 			//Creating a seperate thread to read from SocketCAN network
	// 			if(pthread_create(&readingCanNtwrkThread, NULL, socCANRead, NULL) < 0) 
	// 			{	printf("Error: Cannot Read from SocketCAN\n"); }
	// 			pthread_join(readingCanNtwrkThread, &resultFromThread); 
	// 			printf("socketCAN Transmitter Done...\n");
}



//From https://www.gnu.org/software/libc/manual/html_node/Example-of-Getopt.html
bool ArgParsing(int argc, char* argv[])
{
    int opt;	//Variable used to parse the command line argument
    int argIndex = 0;
	bool validParsing = false; //Variables used to determine if the parsing was valid
    //Argument parsing for "-" used with option
	while ((opt = getopt(argc, argv, optstring)) != -1)
	{
        argIndex++;
		switch (opt) 
		{
			//Two Modes used to control the relay's behaviour (SEE StartupInfo())
	        case 's':
				serverFlag = true;
                //Log here
				break;
			case 'c':
				transmitterFlag = true;
                //Log here
				break;			
			default:
				printf("\nInvalid Option = %s\n", argv[argIndex]);	//if the user inputs an invalid command.
			    break;
		}
	}

    //If the user inputted bin/[EXE] s or bin/[EXE] t, still allow running the program
    if(opt == -1)
    {   
        for (int loopIndex = 1; loopIndex < argc; loopIndex++) {
            if(strcmp(argv[loopIndex], "s") == 0){serverFlag = true;}
            if(strcmp(argv[loopIndex], "c") == 0){transmitterFlag = true;}

        }
    }

    //If either flags were not signalled, print a help message to the user
    if(serverFlag || transmitterFlag)
    {
        validParsing = true;
    }
    
    return validParsing;  //If either flags are signalled, true will be returned to signal successfull parsing

}

/*
* Function      : startupInfo
* Parameters    : N/A
* Returns       : Prints messages onto the screen.
* Description   : Displays the necessary commands in order to run the program appropriately
*                 
*/
void startupInfo()
{
    printf(	PROGNAME " Usage: " PROGNAME " [MODES]\n"
			"Modes:" "\tOptions:\n"
	        "-s	Run Socket Server\n"
			"	This will run the program as a server that will broadcast received message onto the socketCAN network\n"
			"-t	Run socketCAN Client to Dashboard\n"
			"	This will run the program as a client that will transmit messages from the socketCAN network to the Dashboard\n"		
	        "Example:\n"
			//Running the socket server
	        "\tbin/Reader [-s]\n"
			"\tbin/Reader [-c]\n"
			"\tbin/Reader [-s][-c]\n"
			"\tbin/Reader [-t][-c]\n");
}

void MenuInfo()
{		
	printf ("\nCAN Relay Menu"
			"1. Run"
	
	);

	
}