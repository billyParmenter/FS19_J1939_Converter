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

static pthread_t canNtwrkThread, socketNtwrkThread;  //Threads for running a server and reading from the CAN network

/*
* Function      : relayController
* Parameters    : N/A
* Returns       : N/A
* Description   : Will obtain port number and IP address in order to start the CAN Relay
*/
int relayController()
{
	int settingStatus = false;
	//Variables needed to start the two seperate functions
	char* ipAddress = (char*)malloc(sizeof(char)* MID_BUFSIZ);
	int portNumber;
	int threadResult; 
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
	//Getting connection settings 
	do
	{
		if (serverFlag)
		{
			do
			{
				printf("Enter port number to run server: ");
				portNumber = getNum();
				//If the port number is invalid, let the user know and log it 
				if(portNumber < MINIMAL_PORT_LIMIT) {			
					printf("Invalid Input: Port must be greater than or equal to %d\n", MINIMAL_PORT_LIMIT);
					Log(WARN, "Invalid Input: Port must be greater than or equal to MINIMAL_PORT_LIMIT" );
				}
			} while (portNumber < MINIMAL_PORT_LIMIT);
			
			settingStatus = OK_SIG;
		}

		if(clientFlag)
		{
			do
			{
				printf("Enter IP Address to connect to Dashboard(xxx.xxx.xxx.xxx): ");
				settingStatus = getIP(ipAddress);
				if(settingStatus == ERROR)
				{
					printf("Invalid Input: %s Not valid IP Address format\n", ipAddress);
					Log(WARN, "Invalid Input: Not valid IP Address format" );
				}
			} while (settingStatus != OK_SIG);
			
		}
	}while(!settingStatus);

	//Running the server
	if(serverFlag){
		if(pthread_create(&socketNtwrkThread, NULL, serverThread, (void*)&portNumber) < 0) 
		{
			// pthread_kill(readingCanNtwrkThread, SIGTERM);
			printf("Fatal Error: Cannot write from SocketCAN\n");
			threadResult = ERROR;
		}

	}
	//Running the client
	if(clientFlag){
		if(pthread_create(&canNtwrkThread, NULL, socCANRead, (void*)ipAddress) < 0) 
		{
			// pthread_kill(readingCanNtwrkThread, SIGTERM);
			printf("Fatal Error: Cannot read from SocketCAN\n");
			threadResult = ERROR;
		}

	}
	pthread_join(socketNtwrkThread, NULL);
	return OK_SIG;
}

/*
* Function      : CleanupHandler
* Parameters    : int id
* Returns       : void
* Description   : Provides an exit point when the user's inputs Ctrl+C
*/
void CleanupHandler(int id)
{
	Log(WARN,"CAN Relay recieved SIGINT as a signal to close program");
	Log(INFO,"CAN Relay is shutting down...");
	exit(0);
}

/*
* Function      : ArgParsing
* Parameters    : int argc, char* argv[]
* Returns       : int validParsing
* Description   : Parses arguments once the program starts up
*/
//From https://www.gnu.org/software/libc/manual/html_node/Example-of-Getopt.html
int ArgParsing(int argc, char* argv[])
{	
	
    int opt;	//Variable used to parse the command line argument
    int argIndex = 0;
	int validParsing; //Variables used to determine if the parsing was valid
    //Argument parsing for "-" used with option
	while ((opt = getopt(argc, argv, optstring)) != NO_MORE_OPTIONS)
	{
        argIndex++;
		switch (opt) 
		{
			//Two Modes used to control the relay's behaviour (SEE StartupInfo())
	        case 's':
				serverFlag = OK_SIG;
                //Log here
				Log(INFO, "User started CAN Relay with server mode option");
				break;
			case 'c':
				clientFlag = OK_SIG;
                //Log here
				Log(INFO, "User started CAN Relay with client mode option");
				break;			
			default:
				//if the user inputs an invalid command.
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
        validParsing = OK_SIG;
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
int getIP(char* userInputBuffer)
{
	int validIP ;
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
		Log(ERROR, "Sscanf detected invalid IP format! Try again");
		validIP = ERROR;
	}

	else
	{
		validIP = OK_SIG;
	}
	
	return validIP;
}



/*
* FUNCTION:		getNum
* DESCRIPTION:	This function gets input from a user and 
*				helps to assign it to a variable.
* AUTHOR:		Foundation of this function was provided by Sean Clarke.	
* PARAMETERS:	N/A.
* RETURNS:		The input provided by the user.
*/
int getNum(void)
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
