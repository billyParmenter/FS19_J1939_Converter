
/*
* FILE        : startup.cpp
* PROJECT     : CAN Reader
* DATE        : 2020-02-21
* DESCRIPTION : Controls command line parsing,
*/

#include "programDisplay.h"

/*
* NAME :    programDisplay
* PURPOSE : The programDisplay class has been created to accurately model the behavior of a information 
*           exhibitor. It displays needed information for the user on the operation of the CAN reader
*/
class ProgramDisplay{

    // Global static pointer used to ensure a single instance of the class.
    programDisplay* ProgramDisplay::dislayInstance = NULL;

    ProgramDisplay* ProgramDisplay::getInstance(){
        if (!dislayInstance)   // Only allow one instance of class to be generated.

            dislayInstance = new programDisplay;

        return dislayInstance;
    }

    void StartupInfo()
    {
        printf("\n\t -HOW TO START READER-\n");
        printf("\t\t Reader [-r] (RAW CAN DATA Wrapper mode\n");
        printf("\t\t        [-j] (J1939 CAN DATA Wrapper mode\n");
        printf("\t\t        [-s] (Needed actions to have both modes working\n");
    }
}