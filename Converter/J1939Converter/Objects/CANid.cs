/*
 * FILE          : CANid.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */


using DBEntity;
using System;

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
        public ResolutionRatio testResolution;


        public CANid(GetSPNInfo_Result result)
        {
            if (result.PGN != null)
            {

                PGN = (int)result.PGN;
   
            }
            else
            {
                PGN = 0;
            }
            sourceAddress = 240; //hardcoded tractor ECU source address ID 

            if (result.PDU_Specifics != null)
            {
                if (result.PDU_Specifics == "DA")
                {
                    pduSpecific = 0;
                }
                else
                {
                    pduSpecific = int.Parse(result.PDU_Specifics);

                }

            }
            else
            {
                pduSpecific = 0;
            }
            //values are whole numbers and null
            if (result.PDU_Format != null)
            {
                pduFormat = (int)result.PDU_Format;
            }
            else
            {
                pduFormat = 0;
            }
            //values are 0 and NULL this seems odd
            if (result.Data_Page != null)
            {
                dataPage = (int)result.Data_Page;
            }
            else
            {
                //0 and Null are dfferent in the DB find elegant solution
                dataPage = 0;
            }
            //values are 0 to 7 and NULL
            reserved = 0;
            if (result.Default_Priority != null)
            {
                priority = (int)result.Default_Priority;
            }
            else
            {
                //0 and Null are dfferent in the DB find elegant solution
                priority = 0;
            }


                testResolution = new ResolutionRatio(result.Resolution);

        }
        public CANid()
        {

        }



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
                top += "─";
                top = top.PadRight(top.Length + 15, '─');
            }
            top += "┐\n";

            string tableName = ("CAN ID Info");
            tableName = tableName.PadLeft((top.Length / 2) + (tableName.Length / 2) - 2, ' ');
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
