/*
 * FILE          : SPN.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */


using J1939Converter.Support;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace J1939Converter
{


    /*
     * NAME    : SPN
     * PURPOSE : The SPN class inherits from the IConfig interface, allowing for
     *              config file interaction. The SPN clas has been created to
     *              hold the values that are related to an SPN
     */
    public class SPN : IConfig
    {
        public string SpnKey { get; set; }
        public int SpnNumber { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public double Value { get; set; }
        public SPNLength SPNLength { get; set; }
        string IConfig.fileName => ConfigurationManager.AppSettings["SpnFile"];
        string IConfig.objectName => "SPN";





        /*
         * METHOD      : SPN
         * DESCRIPTION : Default constructor
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public SPN()
        {
            Position = 2;
            Length = 2;
            SPNLength = new SPNLength("2 bytes");
        }





        /*
         * METHOD      : Convert
         * DESCRIPTION : Converts a key value pair to a value
         * PARAMETERS  : KeyValuePair<string, string> pair - the key is the spn key,
         *                  value is the spn number
         * RETURNS     : object - the spn object
         */
        public object Convert(KeyValuePair<string, string> pair)
        {
            SPN spn = new SPN
            {
                SpnKey = pair.Key
            };
            if (int.TryParse(pair.Value, out int spnNumber) == false)
            {
                throw new Exception("Could not parse SPN number" + pair.Key + "=" + pair.Value);
            }

            spn.SpnNumber = spnNumber;

            return spn;
        }





        /*
         * METHOD      : GetSPNs
         * DESCRIPTION : Gets the spn objects from the config file
         * PARAMETERS  : NONE
         * RETURNS     : List<SPN> - a list of spns
         */
        public static List<SPN> GetSPNs()
        {
            return Config.GetObjectsFromConfig(new SPN()).Cast<SPN>().ToList();
        }
    }
}
