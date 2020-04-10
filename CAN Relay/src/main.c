/*
* Filename      :   main.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the source code for main entry point of the application
*/



#include "../inc/includes.h"


int main(int argc, char *argv[])
{
	//Start logging everything
	if(!InitializeLog())
	{
		perror("Error: Cannot initliaze log! ");
		exit(0);
	}
    Log(INFO, "Logger initialized");

	//Cannot run the program with just one arg, so exit.
	if(argc <= 1 || argc > 3)
    {
        startupInfo();
		Log(FATAL, "Invalid number of command line arguments");
		ShutDownLogger();
		exit(0);
    }
	

	//Arg parsing
	if(!ArgParsing(argc, argv))
	{
		//Log Error Here
		Log(FATAL, "Invalid Argument to start CAN Relay!");
        ShutDownLogger();
		exit(0);
	}

	//Acquiring user input if need be and start threads to drive functionality
	if(!relayController()){
		ShutDownLogger();
		exit(0);
		//Log error here
	}
	
	Log(INFO, "Shutting down CAN Relay...");
	ShutDownLogger();
    return 0;
}
