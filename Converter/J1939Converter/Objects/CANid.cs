/*
 * FILE          : CANid.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */

namespace J1939Converter
{
    /*
     * Holds can id info for a specific SPN
     */
    class CANid
    {
        public int priority;
        public int reserved;
        public int dataPage;
        public int pduFormat;
        public int pduSpecific;
        public int sourceAddress;
        public int PGN;
        public double resolution;


        public override string ToString()
        {
            int numberOfValues = 8;
            string labels = "|Priority";
            labels = labels.PadRight(15, ' ');
            labels += "|Reserved #";
            labels = labels.PadRight(labels.Length + 5, ' ');
            labels += "|Data Page";
            labels = labels.PadRight(labels.Length + 6, ' ');
            labels += "|PDU Format";
            labels = labels.PadRight(labels.Length + 5, ' ');
            labels += "|PDU Specific";
            labels = labels.PadRight(labels.Length + 3, ' ');
            labels += "|Source Address";
            labels = labels.PadRight(labels.Length + 1, ' ');
            labels += "|PGN";
            labels = labels.PadRight(labels.Length + 12, ' ');
            labels += "|Resolution";
            labels = labels.PadRight(labels.Length + 5, ' ');
            labels += "|\n";

            string values = "|" + priority;
            values = values.PadRight(15, ' ');
            values += "|" + reserved;
            values = values.PadRight(values.Length - reserved.ToString().Length + 15, ' ');
            values += "|" + dataPage;
            values = values.PadRight(values.Length - dataPage.ToString().Length + 15, ' ');
            values += "|" + pduFormat;
            values = values.PadRight(values.Length - pduFormat.ToString().Length + 15, ' ');
            values += "|" + pduSpecific;
            values = values.PadRight(values.Length - pduSpecific.ToString().Length + 15, ' ');
            values += "|" + sourceAddress;
            values = values.PadRight(values.Length - sourceAddress.ToString().Length + 15, ' ');
            values += "|" + PGN;
            values = values.PadRight(values.Length - PGN.ToString().Length + 15, ' ');
            values += "|" + resolution;
            values = values.PadRight(values.Length - resolution.ToString().Length + 15, ' ');
            values += "|\n";


            string top = ("┌");
            top = top.PadRight(15, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                top += "┬";
                top = top.PadRight(top.Length + 15, '─');
            }
            top += "┐\n";


            string mid = ("├");
            mid = mid.PadRight(15, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                mid += "┼";
                mid = mid.PadRight(mid.Length + 15, '─');
            }
            mid += "┤\n";


            string bottom = ("└");
            bottom = bottom.PadRight(15, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                bottom += "┴";
                bottom = bottom.PadRight(bottom.Length + 15, '─');
            }
            bottom += "┘\n";

            return top + labels + mid + values + bottom;
        }
    }
}
