using J1939Converter.Support;
using System;
using System.Collections.Generic;

namespace J1939Converter
{
    class SPNs
    {
        public static List<SPN> spnList = new List<SPN>();

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
