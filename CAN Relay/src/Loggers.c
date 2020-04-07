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

#include <stdio.h> 

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
