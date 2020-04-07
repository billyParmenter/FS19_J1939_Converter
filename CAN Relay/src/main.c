/*
* Filename      :   main.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the source code for main entry point of the application
*/


#include "../inc/includes.h"
#include <signal.h>
static const char optstring[] = "st:"; //String containing the legitimate option characters.

int main(int argc, char *argv[])
{
    int opt;	//Variable used to parse the command line argument
	bool serverFlag, transmitterFlag //Variables used to grant certain CAN relay functionalities
    if(argc <= 1)
    {
        startupInfo("N/A");
    }
	// pthread_t readingCanNtwrkThread; char buffer[256];
	// void* resultFromThread;
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
    
	relayStartup();
    return 0;
}

void relayStartup()
{
	printf("I'm heren\n");
}
/*
* Function      : startupInfo
* Parameters    : N/A
* Returns       : Prints messages onto the screen.
* Description   : Displays the necessary commands in order to run the program appropriately
*/
void startupInfo(char* optarg)
{
	system("clear") /*clear output screen*/;
	printf("\nInvalid Option = %s\n", optarg);
   	printf(	PROGNAME " Usage: " PROGNAME " [MODES]\n"
			"Modes:" "\tOptions:\n"
	        "-s	Run Socket Server\n"
			"	This will run the program as a server that will broadcast received message onto the socketCAN network\n"
			"-t	Run socketCAN Transmitter\n"
			"	This will run the program as a client that will transmit messages from the socketCAN network to the Dashboard\n"		
	        "Example:\n"
			//Running the socket server
	        "\tbin/Reader [-s]\n"
			"\tbin/Reader [-t]\n"
			"\tbin/Reader [-s][-t]\n"
			"\tbin/Reader [-t][-s]\n");
}
