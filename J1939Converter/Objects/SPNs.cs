/*
 * FILE          : SPNs.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */

using J1939Converter.Support;
using System;
using System.Collections.Generic;

namespace J1939Converter
{
    /*
     * Holds the spns from the SPNs.txt file
     * Gets the spns from the file
     * Finds SPN from the spn number
     */
    class SPNs
    {
        public static List<SPN> spnList = new List<SPN>();


        /*
         * FUNCTION    : Init
         * DESCRIPTION : Initilizes the list of SPNs
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public static void Init()
        {
            if (Config.isInitialized == true)
            {
                GetSPNs();
            }
            else
            {
                throw new Exception("Config files not loaded");
            }
        }





        /*
         * FUNCTION    : GetSPNs
         * DESCRIPTION : Gets all SPN keys and numbers from the SPNs.txt file
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        private static void GetSPNs()
        {
            foreach (KeyValuePair<string, string> pair in Config.spnValues)
            {
                if ((int.TryParse(pair.Value, out int number)) == true)
                {
                    SPN spn = new SPN() { spnKey = pair.Key, spnNumber = number };

                    if (number <= 0)
                    {
                        Logger.Log(Logger.ErrorLevel.ERROR, "Error loading SPN: " + pair.Key + " Value should be greater than 0. Value given: " + pair.Value);
                    }
                    else
                    {
                        spnList.Add(spn);
                    }
                }
                else
                {
                    Logger.Log(Logger.ErrorLevel.WARN, "Error loading SPN: " + pair.Key + " Value is not an int. Value given: " + pair.Value);
                }
            }
        }





        /*
         * FUNCTION    : FindSPN
         * DESCRIPTION : Finds an SPN number in the list by the given key string
         * PARAMETERS  : string key - The key to search by
         * RETURNS     : int the SPN number if its found or 0 if not found
         */
        public static int FindSPN(string key)
        {
            int spnNumber = 0;

            foreach(SPN spn in spnList)
            {
                if(spn.spnKey == key)
                {
                    spnNumber = spn.spnNumber;
                    break;
                }
            }

            if (spnNumber <= 0)
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "SPN: " + key + " not found");
            }

            return spnNumber;
        }
    }
}
