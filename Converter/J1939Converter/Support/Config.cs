/*
 * FILE          : Config.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


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
        public static Dictionary<string, string> configValues = new Dictionary<string, string>();
        private static readonly string[] configRequired = new string[] { "modOutputFile", "readerLogLocation", "spnFile" };


        /*
         * FUNCTION    : Init
         * DESCRIPTION : Reads the config files andthrows an exception if there is an error
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public static void Init()
        {
            configValues = ReadFromFile(configFile).ToDictionary(_ => _.Key, _ => _.Value);
            bool configRead = CheckRequiredValues();

            if (configRead == false)
            {
                throw new Exception("Error reading Config file");
            }
        }





        //
        public static List<Object> GetObjectsFromConfig(IConfig objectModel)
        {
            List<Object> objects = new List<Object>();
            List<KeyValuePair<string, string>> pairs = ReadFromFile(objectModel.fileName).ToList();
            
            foreach(KeyValuePair<string, string> pair in pairs)
            {
                objects.Add( objectModel.Convert(pair));
            }

            return objects;
        }





        //
        private static IEnumerable<KeyValuePair<string, string>> ReadFromFile(string filePath)
        {
            Logger.Log(Logger.ErrorLevel.INFO, "Reading config file...");
            List<string> lines = new List<string>();
            try
            {
                lines = File.ReadAllLines(filePath).ToList();
            }
            catch (Exception e)
            {
                Logger.Log(Logger.ErrorLevel.FATAL, "Exception caught trying to read config file. File should be in same directory as .exe ", e);
            }
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');

                yield return new KeyValuePair<string, string>(parts[0], parts[1]);
            }
        }





        /*
         * FUNCTION    : CheckRequiredValues
         * DESCRIPTION : Checks that the required values are in the list
         * PARAMETERS  : Dictionary<string, string> keyValuePairs - The list to check
         *               string[] requiredValues - The required values for the program to run
         * RETURNS     : bool - True if all required values are present in the list
         */
        private static bool CheckRequiredValues()
        {
            bool requiredPresent = true;

            foreach (string value in configRequired)
            {
                if (configValues.ContainsKey(value) == false)
                {
                    Console.WriteLine("Required value: " + value + " not found in config file");
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
    }//Class
}//Namespace
