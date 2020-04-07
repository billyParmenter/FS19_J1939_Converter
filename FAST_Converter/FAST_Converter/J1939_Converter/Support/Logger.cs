/*
* FILE          : Logging.cs
* PROJECT       : J1939Converter
* PROGRAMMER    : Billy Parmenter
* FIRST VERSION : November 1, 2018
* UPDATED       : December 2, 2019
* VERSION       : 2.0
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace J1939Converter.Support
{
    /**
      * NAME    : Logging
      * PURPOSE : The Logging class will log a message to a file that is defined in the 
      *             LoggingInfo class. It will log the ErrorLevel (see LoggingInfo.ErrorLevel),
      *             the date and time it was logged, the line in the file it was logged, 
      *             the method it was called in, the given message, and if present the exception
      *             caught.
      */
    public class Logger
    {
        private static readonly string logFilePath = @"LogFile.txt";
        private static bool initialized = false;

        /// <summary>
        /// The error levels of the logs
        /// </summary>
        public enum ErrorLevel
        {
            OFF,
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
            ALL
        }


        //File to save logs to
        private static string logFile = "";

        //Max error level to log
        private static ErrorLevel MaxLevel = ErrorLevel.ALL;

        //Min error level to log
        private static ErrorLevel MinLevel = ErrorLevel.OFF;

        private static List<ErrorLevel> errorLevels = new List<ErrorLevel>();





        /**
          * METHOD      : Logging
          * DESCRIPTION : Initalizes a logging class with no parameters, 
          *                 the default max is ALL and min is OFF
          * PARAMETERS  : NONE
          * RETURNS     : NONE
          */
        public static void InitializeLogger()
        {
            //get the log file path from logging info
            logFile = logFilePath;
            initialized = true;
        }





        /**
          * METHOD      : Logging
          * DESCRIPTION : Initializes a logging class with two parameters 
          *                 only error levels equal to the max(first parameter) 
          *                 and min(second parameter) are logged. If the 
          *                 max is greater than the min then throw an exception
          * PARAMETERS  : LoggingInfo.ErrorLevel max : The max error level to be logged
          *               LoggingInfo.ErrorLevel min : The min error level to be logged
          * RETURNS     : NONE
          */
        public static void InitializeLogger(ErrorLevel max, ErrorLevel min)
        {
            if (max < min)
            {
                throw new System.ArgumentException("The first argument value cannot be greater than the second argument, " +
                    "see LoggingInfo for LoggingInfo.ErrorLevel values.");
            }

            logFile = logFilePath;

            MaxLevel = max;

            MinLevel = min;
        }





        /**
          * METHOD      : Logging
          * DESCRIPTION : Initalizes a loggin class with one parameter, only that 
          *                 error level will be logged.
          * PARAMETERS  : LoggingInfo.ErrorLevel single : The only error level to be logged
          * RETURNS     : NONE
          */
        public static void InitializeLogger(ErrorLevel single)
        {
            logFile = logFilePath;

            MaxLevel = single;

            MinLevel = single;
        }

        /**
          * METHOD      : Logging
          * DESCRIPTION : Initalizes a loggin class with one parameter, only that 
          *                 error level will be logged.
          * PARAMETERS  : LoggingInfo.ErrorLevel single : The only error level to be logged
          * RETURNS     : NONE
          */
        public static void InitializeLogger(List<ErrorLevel> errorList)
        {
            logFile = logFilePath;

            errorLevels = errorList;
        }




        /**
          * METHOD      : CheckFile
          * DESCRIPTION : Check if the file path exists, if not then create it
          * PARAMETERS  : NONE
          * RETURNS     : NONE
          */
        private void CheckFile()
        {
            if (!File.Exists(logFile))
            {
                File.Create(logFile);
                Console.WriteLine("Logging file created at" + logFile);
            }
        }






        /**
          * METHOD      : Log
          * DESCRIPTION : Log when there is an exception and will not log if the message is blank or spaces.
          * PARAMETERS  : LoggingInfo.ErrorLevel errorLevel : The error level of the log to be logged
                          string message : The messeage of the log
                          Exception ex : The exception being logged
          * RETURNS     : NONE
          */
        public static void Log(ErrorLevel errorLevel,
                        string message,
                        Exception ex)
        {
            if (string.IsNullOrWhiteSpace(message) == false && CheckLevel(errorLevel) == true)
            {
                message += message + Environment.NewLine + "\t Exception caught:" + Environment.NewLine;
                string exMessage = ex.ToString();
                string[] exLines = exMessage.Split('\n');

                foreach (string line in exLines)
                {
                    message += "\t\t" + line;
                }

                Log(errorLevel, message);
            }
        }






        /**
          * METHOD      : Log
          * DESCRIPTION : Log a message, and will not log if the message is blank or spaces.
          * PARAMETERS  : LoggingInfo.ErrorLevel errorLevel : The error level of the log to be logged
                          string message : The messeage of the log
          * RETURNS     : NONE
          */
        public static void Log(ErrorLevel errorLevel,
                        string message)
        {
            if (string.IsNullOrWhiteSpace(message) == false && CheckLevel(errorLevel) == true)
            {
                string logMessage = string.Format("{0} [{1}] - {2} \n\t{3}\n", GetErrorLevelString(errorLevel), DateTime.Now, GetStackTrace(), message);
                SaveToFile(logMessage);
            }
        }





        /**
          * METHOD      : SaveToFile
          * DESCRIPTION : Save the message to the log file
          * PARAMETERS  : NONE
          * RETURNS     : NONE
          */
        private static void SaveToFile(string logMessage)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine(logMessage);
                }
            }
            catch
            {
                if (initialized == false)
                {
                    InitializeLogger();
                    SaveToFile(logMessage);
                }
            }
        }





        /**
          * METHOD      : CheckLevel
          * DESCRIPTION : Check that the error level is within the desired capture levels
          * PARAMETERS  : LoggingInfo.ErrorLevel level : The error level to be tested against
          *                 the max and min error levels. If the logs error level is ALL or
          *                 OFF then the method will throw an exception
          * RETURNS     : bool : true if the error level is between or equal to the max and min error levels
          *                    : false if it is outside the max and min values
          */
        private static bool CheckLevel(ErrorLevel level)

        {
            bool returnValue = false;

            if (errorLevels.Count != 0)
            {

                if (errorLevels.Contains(level))
                {
                    returnValue = true;
                }
            }
            else
            {
                if (level == ErrorLevel.ALL || level == ErrorLevel.OFF)
                {
                    throw new System.ArgumentException("A message being logged can not have an error level equal to OFF or ALL.");
                }

                else if (MinLevel <= level && level <= MaxLevel)
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }



        /// <summary>
        /// Determines the stacktrace of the calling method's caller.
        /// </summary>
        /// <returns>The stacktrace</returns>
        private static string GetStackTrace()
        {
            System.Diagnostics.StackFrame[] frame = new System.Diagnostics.StackTrace(true).GetFrames();

            int i = 0;

            while (i < frame.Length && frame[i].GetFileName().Contains("Logging.cs"))
            {
                i++;
            }

            return frame[i + 2].ToString();
        }



        /**
          * METHOD      : GetErrorLevelString
          * DESCRIPTION : Used for generating the log message, gets a string for the corresponding error level
          * PARAMETERS  : LoggingInfo.ErrorLevel errorLevel : The error level that is to be converted to a string
          * RETURNS     : String : The string version of errorLevel
          */
        private static string GetErrorLevelString(ErrorLevel errorLevel)
        {
            string returnString = "";

            int errorIntValue = (int)errorLevel;

            switch (errorIntValue)
            {
                case 1:
                    returnString = "DEBUG";
                    break;
                case 2:
                    returnString = "INFO ";
                    break;
                case 3:
                    returnString = "WARN ";
                    break;
                case 4:
                    returnString = "ERROR";
                    break;
                case 5:
                    returnString = "FATAL";
                    break;
            }

            return returnString;
        }
    }//Class
}//Namespace