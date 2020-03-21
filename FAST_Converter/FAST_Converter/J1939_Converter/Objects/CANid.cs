/*
 * FILE          : CANid.cs
 * PROJECT       : J1939Converter
 * PROGRAMMERS   : Billy Parmenter
 *                 Connor Lynch
 * FIRST VERSION : Jan 27 2020
 */


using DBEntity;


namespace J1939Converter
{


    /*
     * NAME    : CANid
     * PURPOSE : The CANid class has been created to hold the information regarding
     *              an SPNs CANid
     */
    public class CANid
    {
        public int Priority { get; set; }
        public int Reserved { get; set; }
        public int DataPage { get; set; }
        public int PduFormat { get; set; }
        public int PduSpecific { get; set; }
        public int SourceAddress { get; set; }
        public int PGN { get; set; }
        public ResolutionRatio Resolution { get; set; }


        /*
         * FUNCTION    : CANid
         * DESCRIPTION : Default constructor used for when database is unavailable
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public CANid()
        {
            Priority = 6;
            Reserved = 0;
            DataPage = 0;
            PduFormat = 254;
            PduSpecific = 241;
            SourceAddress = 240;
            PGN = 65265;
            Resolution = new ResolutionRatio("1/256 km/h per bit");
        }





        /*
         * METHOD      : CANid
         * DESCRIPTION : Creates a CANid with the values from a GetSPNInfo_Result
         * PARAMETERS  : GetSPNInfo_Result result - A class to hold the values 
         *                  gotten from the database
         * RETURNS     : NONE
         */
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
            SourceAddress = 240; //hardcoded tractor ECU source address ID 

            if (result.PDU_Specifics != null)
            {
                if (result.PDU_Specifics == "DA")
                {
                    PduSpecific = 0;
                }
                else
                {
                    PduSpecific = int.Parse(result.PDU_Specifics);

                }

            }
            else
            {
                PduSpecific = 0;
            }
            //values are whole numbers and null
            if (result.PDU_Format != null)
            {
                PduFormat = (int)result.PDU_Format;
            }
            else
            {
                PduFormat = 0;
            }
            //values are 0 and NULL this seems odd
            if (result.Data_Page != null)
            {
                DataPage = (int)result.Data_Page;
            }
            else
            {
                //0 and Null are dfferent in the DB find elegant solution
                DataPage = 0;
            }
            //values are 0 to 7 and NULL
            Reserved = 0;
            if (result.Default_Priority != null)
            {
                Priority = (int)result.Default_Priority;
            }
            else
            {
                //0 and Null are dfferent in the DB find elegant solution
                Priority = 0;
            }


            Resolution = new ResolutionRatio(result.Resolution);

        }
    }
}
