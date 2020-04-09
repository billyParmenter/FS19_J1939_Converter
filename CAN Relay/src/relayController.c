/*
* Filename      :   main.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the source code for the relayController functions.
*                   These functions create and properly close threads with a signal handler.
*/

#include "../inc/relayController.h" //Include

static bool serverFlag = false; 
static bool clientFlag = false; //Variables used to grant certain CAN relay functionalities


/*
* Function      : relayController
* Parameters    : N/A
* Returns       : N/A
* Description   : Will obtain port number and IP address in order to start the CAN Relay
*/
bool relayController()
{
	bool keepConRunning = false; //Variable used to check if the user wants to run with server with defaults
	char userInputBuffer[MID_BUFSIZ] = { 0 };
	//Variables needed to start the two seperate functions
	char* ipAddress = (char*)malloc(sizeof(char)* MID_BUFSIZ);
	int portNumber;

	//Assigning Signal Handler
	struct sigaction signalStruct;
	signalStruct.sa_handler = CleanupHandler;

	//If there was an error in creating the signal handler, exit the program.
	if((sigfillset(&signalStruct.sa_mask) == SIGNAL_ERROR 
		|| (sigaction(SIGINT, &signalStruct, NULL)) == SIGNAL_ERROR))
	{
		Log(WARN, "Cannot register signal handler");

	}

	else{Log(INFO, "CAN Relay uses SIGINT as a signal to close program");}	

	//If the server is supposed to run, obtain port numberand run it
	if(serverFlag)
	{
		while(!keepConRunning)
		{
			printf("Enter port number to run server: ");
			//If the value is greater than 
			if((portNumber = getPortNum()) >= MINIMAL_PORT_LIMIT)
			{
				keepConRunning= true;
				Log(INFO, "Valid Port Number Inputted by user...");
			}
			else
			{
				printf("Invalid Input: Port must be greater than or equal to %d\n", MINIMAL_PORT_LIMIT);
				Log(WARN, "Invalid Input: Port must be greater than or equal to MINIMAL_PORT_LIMIT" );
			}
		}
		
		keepConRunning = false;	
	}

	//If the client is supposed to run, obtain ip address
	if(clientFlag)
	{
		//Start the program with defaults, in a loop to avoid invalid input 
		while(!keepConRunning)
		{
			printf("Enter IP Address to connect to Dashboard(xxx.xxx.xxx.xxx): ");
			//If it is a valid IP, stop asking and run the client
			if(getIP(userInputBuffer))
			{
				keepConRunning = true;
				Log(INFO, "Valid IP Address Inputted by user...");

			}
		}

		keepConRunning = false;	

	}
	//If the user inputs are valid, run both functionalites
	if(!keepConRunning)
	{
		//Thread creation here
		pthread_t canNtwrkThread, socketNtwrkThread;
		// // SIGINT       
		if(pthread_create(&socketNtwrkThread, NULL, serverThread, (void*)&portNumber) < 0) 
		{
			// pthread_kill(readingCanNtwrkThread, SIGTERM);
			// printf("Error: Cannot Read from SocketCAN\n"); }
		// ServerFunc();
		}
		int threadResult = pthread_join(socketNtwrkThread, NULL);
		if(threadResult == SOCKET_ERROR)
		{
			keepConRunning = SOCKET_ERROR;
		}
	}

    return keepConRunning;
}




void CleanupHandler(int id)
{
	Log(WARN,"CAN Relay recieved SIGINT as a signal to close program");
	Log(INFO,"CAN Relay is shutting down...");

	exit(0);
}


//From https://www.gnu.org/software/libc/manual/html_node/Example-of-Getopt.html
bool ArgParsing(int argc, char* argv[])
{	
	
    int opt;	//Variable used to parse the command line argument
    int argIndex = 0;
	bool validParsing = false; //Variables used to determine if the parsing was valid
    //Argument parsing for "-" used with option
	while ((opt = getopt(argc, argv, optstring)) != NO_MORE_OPTIONS)
	{
        argIndex++;
		switch (opt) 
		{
			//Two Modes used to control the relay's behaviour (SEE StartupInfo())
	        case 's':
				serverFlag = true;
                //Log here
				Log(INFO, "User started CAN Relay with server mode option");
				break;
			case 'c':
				clientFlag = true;
                //Log here
				Log(INFO, "User started CAN Relay with client mode option");
				break;			
			default:
				printf("\nInvalid Option = %s\n", argv[argIndex]);	//if the user inputs an invalid command.
				validParsing = false;
			    break;
		}
	}

    //If the user inputted bin/[EXE] s or bin/[EXE] t, still allow running the program
    if(opt == NO_MORE_OPTIONS)
    {   
        for (int loopIndex = 1; loopIndex < argc; loopIndex++) {
			//Since strcmp returns 0 (which is false), check if function returned "false"
            if(strcmp(argv[loopIndex], "s") == EQUAL_STRING){serverFlag = true;}
            if(strcmp(argv[loopIndex], "c") == EQUAL_STRING){clientFlag = true;}

        }
    }

    //If either flags were not signalled, print a help message to the user
    if(serverFlag || clientFlag)
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
			"-c	Run socketCAN Client to Dashboard\n"
			"	This will run the program as a client that will transmit messages from the socketCAN network to the Dashboard\n"		
	        "Example:\n"
			//Running the socket server
	        "\tbin/Reader [-s]\n"
			"\tbin/Reader [-c]\n"
			"\tbin/Reader [-s][-c]\n"
			"\tbin/Reader [-t][-c]\n");
}


/*
* Function      : bool getIP(char* userInputBuffer)
* Parameters    : char* userInputBuffer - Buffer used to acuqire the user's input
* Returns       : bool validIP
* Description   : Displays the necessary commands in order to run the program appropriately
*/
bool getIP(char* userInputBuffer)
{
	bool validIP = false;
	int ipAddressBuf[IP_ARR_LENGTH] = {0};	//Buffer to acquire the IP address in bits

	/* use fgets() to get a string from the keyboard */
	fgets(userInputBuffer, MID_BUFSIZ, stdin); /* read IP into buf */
	/* extract the letter from the string; sscanf() returns a number
	* corresponding with the number of items it found in the string */
	if (sscanf(userInputBuffer, "%d.%d.%d.%d", &ipAddressBuf[FIRST_IP_GRP], &ipAddressBuf[SCND_IP_GRP],  
												&ipAddressBuf[THIRD_IP_GRP],  &ipAddressBuf[FOURTH_IP_GRP]) != IP_ARR_LENGTH)
	{
		/* if the user did not enter a char recognizable by
		* the system, set number to NULL */
		Log(WARN, "Invalid Input! Remember IPv4 is in XXX.XXX.XXX.XXX Format...Try Again...");
		printf("Error! See Logs...\n");
	}

	else
	{
		validIP = true;
	}
	
	return validIP;
}



/*
* FUNCTION:		getPortNum
* DESCRIPTION:	This function gets input from a user and 
*				helps to assign it to a variable.
* AUTHOR:		Foundation of this function was provided by Sean Clarke.	
* PARAMETERS:	N/A.
* RETURNS:		The input provided by the user.
*/
int getPortNum(void)
{
	/* function limitation: only accepts 120 characters of input */
	char record[MID_BUFSIZ] = { 0 };
	int number = 0;
	/* NOTE to student:
	* indent and brace this function consistent with your other functions */

	/* use fgets() to get a string from the keyboard */
	fgets(record, MID_BUFSIZ, stdin);
	/* extract the number from the string; sscanf() returns a number
	* corresponding with the number of items it found in the string */
	if (sscanf(record, "%d", &number) != 1)
	{
		Log(WARN, "Invalid Input...Try Again...");
		/* if the user did not enter a number recognizable by
		* the system, set number to NO_MORE_OPTIONS */
		number = INVALID_INPUT;
	}
	return number;
}
