/*
 * FILE          : Config.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace J1939Converter.Support
{

    /*
     * NAME    : ConfigFile
     * PURPOSE : This static class will read the config file and
     *              save the expected values into variables
     */
    public class Config
    {
        private static readonly string configFile = @"Config.txt";
        private static readonly string spnFile = @"SPNs.txt";
        public static Dictionary<string, string> configValues = new Dictionary<string, string>();
        public static Dictionary<string, string> spnValues = new Dictionary<string, string>();
        private static readonly string[] configRequired = new string[] { "modOutputFile", "readerLogLocation" };
        public static bool isInitialized = false;


        /*
         * FUNCTION    : Init
         * DESCRIPTION : Reads the config files andthrows an exception if there is an error
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public static void Init()
        {
            bool configRead = ReadConfigFile();
            bool spnsRead = ReadSpnFile();

            if (configRead == false && spnsRead == false)
            {
                throw new Exception("Error reading Config file and SPN file");
            }
            else if (configRead == false)
            {
                throw new Exception("Error reading Config file");
            }
            else if (spnsRead == false)
            {
                throw new Exception("Error reading SPN file");
            }

            isInitialized = true;
        }





        /*
         * FUNCTION    : ReadSpnFile
         * DESCRIPTION : Reads the spn file
         * PARAMETERS  : NONE
         * RETURNS     : bool - if the read was successful
         */
        private static bool ReadSpnFile()
        {
            bool spnsRead = ReadFileValues(spnValues, spnFile);

            if(spnsRead == true)
            {
                Logger.Log(Logger.ErrorLevel.INFO, "SPN file read successfuly");
            }

            return spnsRead;
        }





        /*
         * FUNCTION    : ReadConfigFile
         * DESCRIPTION : Reads the config file and will check that the required values are present
         * PARAMETERS  : NONE
         * RETURNS     : bool - if the read was successful
         */
        private static bool ReadConfigFile()
        {
            bool configRead = ReadFileValues(configValues, configFile);

            bool requiredPresent = CheckRequiredValues(configValues, configRequired);

            return configRead && requiredPresent;
        }





        /*
         * FUNCTION    : CheckRequiredValues
         * DESCRIPTION : Checks that the required values are in the list
         * PARAMETERS  : Dictionary<string, string> keyValuePairs - The list to check
         *               string[] requiredValues - The required values for the program to run
         * RETURNS     : bool - True if all required values are present in the list
         */
        private static bool CheckRequiredValues(Dictionary<string, string> keyValuePairs, string[] requiredValues)
        {
            bool requiredPresent = true;

            foreach (string value in requiredValues)
            {
                if (keyValuePairs.ContainsKey(value) == false)
                {
                    Logger.Log(Logger.ErrorLevel.FATAL, "Required value: " + value + " not found in config file");
                    requiredPresent = false;
                }
            }

            if (requiredPresent == true)
            {
                Logger.Log(Logger.ErrorLevel.INFO, "Config file read successfuly");
            }

            return requiredPresent;
        }





        /*
         * FUNCTION    : ReadConfig
         * DESCRIPTION : Reads the values from the config file
         * PARAMETERS  : string file - The file to read the key value pairs from
         *               Dictionary<string, object> dictionary - The dictionary to save the values to
         * RETURNS     : NONE
         */
        private static bool ReadFileValues(Dictionary<string, string> keyValuePairs, string file)
        {
            bool fileRead = false;

            Logger.Log(Logger.ErrorLevel.INFO, "Reading config file...");

            try
            {
                string[] lines = File.ReadAllLines(file);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');

                    keyValuePairs.Add(parts[0], parts[1]);
                }

                fileRead = true;
            }
            catch (Exception e)
            {
                Logger.Log(Logger.ErrorLevel.FATAL, "Exception caught trying to read config file. File should be in same directory as .exe ", e);
            }

            return fileRead;
        }
    }//Class
}//Namespace
