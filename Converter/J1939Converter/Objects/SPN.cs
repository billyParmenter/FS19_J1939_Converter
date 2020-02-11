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
        public SPNLength testSPNLength;

        string IConfig.fileName => Config.configValues["spnFile"];

        public object Convert(KeyValuePair<string, string> pair)
        {
            SPN spn = new SPN();
            spn.spnKey = pair.Key;

            if (int.TryParse(pair.Value, out spn.spnNumber) == false)
            {
                throw new Exception("Could not parse SPN number" + pair.Key + "=" + pair.Value);
            }

            return spn;
        }

        public static List<SPN> GetSPNs()
        {
            return Config.GetObjectsFromConfig(new SPN()).Cast<SPN>().ToList();
        }

        public override string ToString() 
        {
            int numberOfValues = 3;
            string labels = "|SPN Key";
            labels = labels.PadRight(15, ' ');
            labels += "|SPN #";
            labels = labels.PadRight(labels.Length + 10, ' ');
            labels += "|Value";
            labels = labels.PadRight(labels.Length + 10, ' ');
            labels += "|\n";

            string values = "|" + spnKey;
            values = values.PadRight(15, ' ');
            values += "|" + spnNumber;
            values = values.PadRight(values.Length - spnNumber.ToString().Length + 15, ' ');
            values += "|" + value;
            values = values.PadRight(values.Length - value.ToString().Length + 15, ' ');
            values += "|\n";


            string top = ("┌");
            top = top.PadRight(15, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                top += "─";
                top = top.PadRight(top.Length + 15, '─');
            }
            top += "┐\n";

            string tableName = ("SPN & PGN Info");
            tableName = tableName.PadLeft((top.Length/2) + (tableName.Length/2) - 2, ' ');
            tableName = tableName.PadRight(top.Length - 3, ' ');
            string title = "|" + tableName + "|\n";

            string mid1 = ("├");
            mid1 = mid1.PadRight(15, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                mid1 += "┬";
                mid1 = mid1.PadRight(mid1.Length + 15, '─');
            }
            mid1 += "┤\n";

            string mid2 = ("├");
            mid2 = mid2.PadRight(15, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                mid2 += "┼";
                mid2 = mid2.PadRight(mid2.Length + 15, '─');
            }
            mid2 += "┤\n";


            string bottom = ("└");
            bottom = bottom.PadRight(15, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                bottom += "┴";
                bottom = bottom.PadRight(bottom.Length + 15, '─');
            }
            bottom += "┘";

            return top + title + mid1 + labels + mid2 + values + bottom;
        }

    }
}
