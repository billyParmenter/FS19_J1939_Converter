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





        /*
         * METHOD      : GetObjectsFromConfig
         * DESCRIPTION : Reads a file and fills a list of objects from the file
         * PARAMETERS  : IConfig objectModel - The interface of the object to be read
         * RETURNS     : List<Object> - The list of objects read from the config file
         */
        public static List<Object> GetObjectsFromConfig(IConfig objectModel)
        {
            List<Object> objects = new List<Object>();
            Logger.Log(Logger.ErrorLevel.INFO, "Getting object config: " + objectModel.objectName);
            List<KeyValuePair<string, string>> pairs = ReadFromFile(objectModel.fileName).ToList();
            
            foreach(KeyValuePair<string, string> pair in pairs)
            {
                objects.Add(objectModel.Convert(pair));
            }

            return objects;
        }





        /*
         * METHOD      : ReadFromFile
         * DESCRIPTION : Reads a file filled with keyvalue pairs
         * PARAMETERS  : string filePath - The path to the file to read
         * RETURNS     : IEnumerable<KeyValuePair<string, string>> - The key value pairs read from the file
         */
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
    }//Class
}//Namespace
