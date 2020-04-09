/*
* Filename      :   main.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the source code for the relayController functions.
*                   These functions create and properly close threads with a signal handler.
*/

#include "../inc/relayController.h" //Include

static bool serverFlag, clientFlag; //Variables used to grant certain CAN relay functionalities

/*
* Function      : relayStartup
* Parameters    : N/A
* Returns       : N/A
* Description   : Will obtain port number and IP address in order to start the CAN Relay
*/
bool relayStartup()
{
	bool runWithDefaults, stopAsking = false; //Variable used to check if the user wants to run with server with defaults
	int ipAddressBuf[IP_ARR_LENGTH] = {0};
	char userInputBuffer[MID_BUFSIZ] = { 0 };
	char* ipAddress = (char*)malloc(sizeof(char)* MID_BUFSIZ);


	//NEW STUFF
	pthread_t readingCanNtwrkThread;
	//
	//Start the program with defaults, in a loop to avoid invalid input 
	while(!stopAsking)
	{
		//If the server is supposed to run, obtain port number
		if(serverFlag)
		{
			printf("Enter port number to run server: ");
			if(getNum() > 0)
			{
				printf("Valid Input\n");
				stopAsking= true;

				if(pthread_create(&readingCanNtwrkThread, NULL, socCANRead, NULL) < 0) 
				{	printf("Error: Cannot Read from SocketCAN\n"); }
				ServerFunc();
				pthread_join(readingCanNtwrkThread, NULL); 
				printf("socketCAN Transmitter Done...\n");
			}
		}
		//If the client is supposed to run, obtain ip address
		if(clientFlag)
		{
			printf("Enter IP Address to connect to Dashboard(xxx.xxx.xxx.xxx): ");

			/* use fgets() to get a string from the keyboard */
			fgets(userInputBuffer, 121, stdin); /* read IP into buf */
			/* extract the letter from the string; sscanf() returns a number
			* corresponding with the number of items it found in the string */
			if (sscanf(userInputBuffer, "%d.%d.%d.%d", &ipAddressBuf[FIRST_IP_GRP], &ipAddressBuf[SCND_IP_GRP],  
														&ipAddressBuf[THIRD_IP_GRP],  &ipAddressBuf[FOURTH_IP_GRP]) != 4)
			{
				/* if the user did not enter a char recognizable by
				* the system, set number to NULL */
				Log(WARN, "Sscanf returned -1...Try Again...");
				printf("Error! See Logs...\n");
			}
			sprintf(ipAddress, "%s", userInputBuffer);
			//If the user's input is an invalid IP address
			if(inet_pton(AF_INET, userInputBuffer, userInputBuffer))
			{
				stopAsking= true;
			}
			else
			{
				perror("Error: ");
				Log(WARN, "Invalid IP Address Input...Try Again...");
				printf("Error! See Logs...\n");
			}
			
		}

	}
	// MenuInfo();	
	// while(keepRunning)
	// {
	// 	//Checking if the user wants to run with defaults or not
	// 	switch(getNum())
	// 	{
	// 		case 1:
	// 			if(serverFlag)
	// 			{
	// 				printf("Run w/ defaults? [Y / N]")
	// 			}
	// 			break;
	// 		case 2:
	// 			if(clientFlag)
	// 			{
					
	// 			}
	// 			break;	
	// 		case 3:
	// 			break;
	// 		case 4:
	// 			keepRunning = false;
	// 			break;
			
	// 	}
	// }
	// pthread_t readingCanNtwrkThread; char buffer[256];
	// void* resultFromThread;
	// signal(SIGINT, sigintHandler);

    // printf("Starting CAN Relay Server");
    // printf("\n Does user want server? %d\n", serverFlag);
    // printf("\n Does user want client? %d\n", transmitterFlag);

    return stopAsking;
	// printf("socketCAN Transmitter Starting...\n");
	// 			//Creating a seperate thread to read from SocketCAN network
	// 			if(pthread_create(&readingCanNtwrkThread, NULL, socCANRead, NULL) < 0) 
	// 			{	printf("Error: Cannot Read from SocketCAN\n"); }
	// 			pthread_join(readingCanNtwrkThread, &resultFromThread); 
	// 			printf("socketCAN Transmitter Done...\n");
}


// void ThreadExitHandler(int sysSignal)
// {
// 	thread_t threadID;
// 	threadID = thr_self();
// }
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
				Log(INFO, "User started CAN Relay with server mode option");
				break;
			case 'c':
				clientFlag = true;
                //Log here
				Log(INFO, "User started CAN Relay with client mode option");
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
            if(strcmp(argv[loopIndex], "c") == 0){clientFlag = true;}

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

void MenuInfo()
{
	printf ("CAN Relay Menu\n"
			"1. Run Server\n"
			"2. Run Client\n"
			"3. Change socket/socketCAN Settings\n"
			"4. Exit\n"
			"Input An Option To Continue: "
	);	
}


// FUNCTION:		getNum
// DESCRIPTION:		This function gets input from a user and 
//					helps to assign it to a variable.
// AUTHOR:			This function was provided by Sean Clarke.	
// PARAMETERS:		N/A.
// RETURNS:			The input provided by the user.

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
		* the system, set number to -1 */
		number = -1;
	}
	return number;
}
