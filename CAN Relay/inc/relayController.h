/*
* FILE          : relayController.h
* PROJECT       : CAN Relay
* PROGRAMMER    : Oloruntoba Samuel Lagunju
* DATE          : April 6th 2020
* DESCRIPTION   : Contains methods that controls what goes on with the CAN Relay once arguments have been parsed and validated
*/

#ifndef RELAYCON_H
#define RELAYCON_H
//Default includes
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <stdbool.h>
#include <err.h>
#include <signal.h>
#include <getopt.h>
#include <pthread.h>
#include <signal.h>
#include <arpa/inet.h>


#include "SocketFunc.h"
#include "../inc/Loggers.h" //Include

#define PROGNAME "canRelay" //Definition for the program name
#define MID_BUFSIZ 121  //Definition used for a mid size array
#define NO_MORE_OPTIONS  -1//Defintion used to indicate getopt return
#define INVALID_INPUT -1    //Definition for getPortNum to signify an invalid input from the user

#define SIGNAL_ERROR -1 //Definition used to recognise the return value from sig functions
#define OK_SIG  1 //Signal used to indicate everything is running okay
//IP Location defintions
#define FIRST_IP_GRP 0
#define SCND_IP_GRP 1
#define THIRD_IP_GRP 2
#define FOURTH_IP_GRP 3

#define MINIMAL_PORT_LIMIT 1000 //Server can only listen on ports higher or equal to this value
#define IP_ARR_LENGTH 4 //Defines number of parts to an IP address
#define EQUAL_STRING 0  //Define used to signifiy both strings are equal in strcmp
static const char optstring[] = "sc"; //String containing the legitimate option characters.#include <ctype.h>
//Program Declarations
int ArgParsing(int argc, char* argv[]);
int relayController();
void startupInfo();
int getNum(void);
int getIP(char* userInputBuffer);

void CleanupHandler(int id);

#endif /* !RELAYCON_H */