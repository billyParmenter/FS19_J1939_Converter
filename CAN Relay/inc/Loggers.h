/*
* FILE          : Loggers.c
* PROJECT       : CAN Relay
* PROGRAMMER    : Oloruntoba Samuel Lagunju
* DATE          : April 6th 2020
* DESCRIPTION   : Contains methods that will log a message to a file that is defined in the 
*                 LoggingInfo class. It will log the ErrorLevel, the date and time it was logged, 
*                 the line in the file it was logged, the method it was called in, the given 
*                 message, and if present the exception caught.
*/

#ifndef LOGGERS_H
#define LOGGERS_H
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
#include <time.h>   //Time Include
//Includes for creating a directory
#include <sys/stat.h> 
#include <sys/types.h> 

// The error levels of the logs
enum ErrorLevel
{
    OFF,
    DEBUG,
    INFO,
    WARN,
    ERROR,
    FATAL,
    ALL
};

#define SML_BUFFSIZ 64  //Constant for a smaller buffer size
#define RW_MODE 0777   // Mode Bits for Access Permission


// static bool initialized;
extern char* errorLevelString;  //String used to how signify how serious the error is
extern char* filePath;  //String used to how signify how serious the error is
extern char* logMessage;
extern FILE* logFilePtr; //a pointer of type file



void InitializeLogger();
const void Log(ErrorLevel errorLevel, const char* logMessage);
const void GetErrorLevelString(ErrorLevel errorLevel, char* errorLevelString);
const bool InitializeLog();
const void ShutDownLogger();
bool CheckLevel(ErrorLevel levelToCheck);

#endif /* !LOGGERS_H */