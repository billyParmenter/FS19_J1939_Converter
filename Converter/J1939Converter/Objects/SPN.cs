/*
 * FILE          : SPN.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public override string ToString() 
        {
            int numberOfValues = 3;
            string labels = "|SPN Key";
            labels = labels.PadRight(11, ' ');
            labels += "|SPN #";
            labels = labels.PadRight(labels.Length + 6, ' ');
            labels += "|Value";
            labels = labels.PadRight(labels.Length + 6, ' ');
            labels += "|\n";

            string values = "|" + spnKey;
            values = values.PadRight(11, ' ');
            values += "|" + spnNumber;
            values = values.PadRight(values.Length - spnNumber.ToString().Length + 11, ' ');
            values += "|" + value;
            values = values.PadRight(values.Length - value.ToString().Length + 11, ' ');
            values += "|\n";


            string top = ("┌");
            top = top.PadRight(11, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                top += "┬";
                top = top.PadRight(top.Length + 11, '─');
            }
            top += "┐\n";


            string mid = ("├");
            mid = mid.PadRight(11, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                mid += "┼";
                mid = mid.PadRight(mid.Length + 11, '─');
            }
            mid += "┤\n";


            string bottom = ("└");
            bottom = bottom.PadRight(11, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                bottom += "┴";
                bottom = bottom.PadRight(bottom.Length + 11, '─');
            }
            bottom += "┘\n";

            return top + labels + mid + values + bottom;
        }

    }
}
