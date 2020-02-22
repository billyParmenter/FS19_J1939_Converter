/*
* FILE        : porgramDisplay.h
* PROJECT     : CAN Reader
* DATE        : 2020-02-21
* DESCRIPTION : Contains definitions needed for the programDisplay class. 
*/

#include <stdio.h>
#include <stdlib.h>

/*
* NAME :    programDisplay
* PURPOSE : The programDisplay class has been created to accurately model the behavior of a information 
*           exhibitor. It displays needed information for the user on the operation of the CAN reader
*/
class ProgramDisplay{
    public:
        static  ProgramDisplay* getInstance();
        void StartupInfo();

    private:
        ProgramDisplay();
        static ProgramDisplay* dislayInstance;


};