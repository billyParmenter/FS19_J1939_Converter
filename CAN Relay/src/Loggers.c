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

#include "../inc/Loggers.h"

static bool initialized;
char* errorLevelString = NULL;
char* filePath = NULL;  //String used to how signify how serious the error is
char* logMessage = NULL;
FILE* logFilePtr = NULL; //a pointer of type file
const int InitializeLog()
{
    errorLevelString = (char*)malloc(sizeof(char) * SML_BUFFSIZ); 
    filePath = (char*)malloc(sizeof(char) * SML_BUFFSIZ);
    logMessage = (char*)malloc(sizeof(char)* SML_BUFFSIZ);
    //If one of the mallocs did not work, exit the program
    if((errorLevelString != NULL) && (filePath != NULL) && (logMessage != NULL))
    {
        strcpy(filePath, "./LogFile.txt"); //Saving the filePath to save logs to
        logFilePtr = fopen(filePath, "a");
        if(logFilePtr != NULL)  //if there was no error opening the file, logger is ready
        {
            initialized = OK_SIG;
   
        }
    }

    return initialized;
}

const void ShutDownLogger()
{
    Log(INFO, "Cleaned up all log resources");
    //Clearing up all memory before closing application
    free(errorLevelString);
    free(filePath);
    free(logMessage);
    // fclose(logFilePtr);
}

const void Log(ErrorLevel currentErrorLevel, const char* logBufferMsg)
{  
    char currentDate[1280];
    logFilePtr = fopen(filePath, "a");//Open file to write
    //Formatting date
    time_t t = time(NULL);
    struct tm localTime = *localtime(&t);

    if(CheckLevel(currentErrorLevel) && (logBufferMsg != NULL))
    {
        sprintf(currentDate, "%d-%02d-%02d %02d:%02d:%02d", localTime.tm_year + 1900, localTime.tm_mon + 1, localTime.tm_mday, 
                                                     localTime.tm_hour, localTime.tm_min, localTime.tm_sec);
        //Getting the appopriate error level to log
        GetErrorLevelString(currentErrorLevel, errorLevelString);
        //Formatting the message
        sprintf(logMessage, "[%s]:\t[%s]:\t%s\n",currentDate, errorLevelString, logBufferMsg);
        //Save to File
        fprintf(logFilePtr,"%s", logMessage);
        fclose(logFilePtr);
    }
}








bool CheckLevel(ErrorLevel levelToCheck)
{
    bool validLevel = false;
    if (levelToCheck == ALL || levelToCheck == OFF)
    {
        Log(FATAL, "A message being logged can not have an error level equal to OFF or ALL.");
    }

    //Checking that the error level is within the desired capture levels
    else if (OFF <= levelToCheck && levelToCheck <= ALL)
    {
        validLevel = true;
    }
    
    return validLevel;
}

const void GetErrorLevelString(ErrorLevel errorLevel, char* errorLevelString)
{
    int errorIntValue = (int)errorLevel;
    switch (errorIntValue)
    {
        case DEBUG:
            strncpy(errorLevelString ,"DEBUG", sizeof(errorLevel));
            break;
        case INFO:

            strcpy(errorLevelString ,"INFO");
            break;
        case WARN:
            strcpy(errorLevelString ,"WARN");
            break;
        case ERROR:
            strcpy(errorLevelString ,"ERROR");
            break;
        case FATAL:
            strcpy(errorLevelString ,"FATAL");
            break;
    }
}