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

#include "../inc/Loggers.h" //Include

#define PROGNAME "canRelay" //Definition for the program name
static const char optstring[] = "-s-c"; //String containing the legitimate option characters.#include <ctype.h>

bool ArgParsing(int argc, char* argv[]);
bool relayStartup();
void startupInfo();
// void startupInfo(char* optarg);
#endif /* !RELAYCON_H */