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
#define MID_BUFSIZ 121
#define FIRST_IP_GRP 0
#define SCND_IP_GRP 1
#define THIRD_IP_GRP 2
#define FOURTH_IP_GRP 3

#define IP_ARR_LENGTH 4
static const char optstring[] = "sc"; //String containing the legitimate option characters.#include <ctype.h>

bool ArgParsing(int argc, char* argv[]);
bool relayStartup();
void startupInfo();
int getNum(void);
void MenuInfo();

// void startupInfo(char* optarg);
#endif /* !RELAYCON_H */