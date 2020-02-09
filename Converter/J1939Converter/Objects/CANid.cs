/*
 * FILE          : CANid.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */
using DBEntity;

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
            sourceAddress = 240; //figure this out later

            if (result.PDU_Specifics != null)
            {
                if (result.PDU_Specifics == "DA")
                {
                    pduSpecific = 0;
                }
                else
                {
                    pduSpecific = int.Parse(result.PDU_Specifics);//type needs to change to string
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
    
    }
}
