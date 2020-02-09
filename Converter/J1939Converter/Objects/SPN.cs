/*
 * FILE          : SPN.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */
using System;
using System.Collections.Generic;
using System.Linq;
using J1939Converter.Support;

namespace J1939Converter
{
    /*
     * For holding info on specific SPN
     */
    class SPN : IConfig
    {
        public string spnKey;
        public int spnNumber;
        public int position;
        public int length;
        public double value;

        string IConfig.fileName => Config.configValues["spnFile"];

        public IConfig Convert(KeyValuePair<string, string> pair)
        {
            spnKey = pair.Key;

            if (int.TryParse(pair.Value, out spnNumber) == false)
            {
                throw new Exception("Could not parse SPN number" + pair.Key + "=" + pair.Value);
            }

            return this;
        }

        public static List<SPN> GetSPNs()
        {
            return Config.GetObjectsFromConfig(new SPN()).Cast<SPN>().ToList();
        }

    }
}
