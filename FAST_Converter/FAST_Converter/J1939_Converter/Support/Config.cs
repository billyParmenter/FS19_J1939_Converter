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
         * FUNCTION    : 
         * DESCRIPTION : 
         * PARAMETERS  : 
         * RETURNS     : 
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
         * FUNCTION    : 
         * DESCRIPTION : 
         * PARAMETERS  : 
         * RETURNS     : 
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
