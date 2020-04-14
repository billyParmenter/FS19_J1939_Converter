/*
* FILE			:	Database.cs
* PROJECT		:   Ji1939Convereter
* PROGRAMMER	:   Connor Lynch and Billy Parmenter
* FIRST VERSION :   
* DESCRIPTION	:   Establishes communications to the DB to gatehr data
*                   
*/
using System;
using System.Text.RegularExpressions;
using DBEntity;

namespace J1939Converter.Communication
{
    class Database
    {

        /*
         * METHOD      : GetSPN
         * DESCRIPTION : Gets the SPN and CANid info from the database
         * PARAMETERS  : SPN - The spn to search for
         * RETURNS     : CANid - The canID info
         */
        public static CANid GetSPN(ref J1939Converter.SPN spn)
        {
            System.Data.Entity.Core.Objects.ObjectResult<GetSPNInfo_Result> result;
            CANid canID = null;
            using (Entities db = new Entities())
            {
                result = db.GetSPNInfo(spn.SpnNumber);

                //janky and will need revisions on both ends
                foreach (GetSPNInfo_Result test in result)
                {
                    if (test.SPN_Position.Contains("-") || test.SPN_Position.Contains("."))
                    {
                        //regex expression means dash OR period (backslash is escape character)
                        string[] elements = System.Text.RegularExpressions.Regex.Split(test.SPN_Position, @"-|\.");
                        spn.Position = int.Parse(elements[0]);
                    }
                    else 
                    {
                        spn.Position = int.Parse(test.SPN_Position);
                    }
                    spn.SPNLength = new SPNLength(test.SPN_Length);
                    canID = new CANid(test);

                }
            }

            return canID;

        }





        /*
         * METHOD      : Test
         * DESCRIPTION : Test the connection to the database
         * PARAMETERS  : NONE
         * RETURNS     : string - null if no error message
         */
        public static string Test()
        {
            try
            {
                SPN spn = new SPN();
                System.Data.Entity.Core.Objects.ObjectResult<GetSPNInfo_Result> result;
                CANid canID = null;
                using (Entities db = new Entities())
                {
                    result = db.GetSPNInfo(spn.SpnNumber);

                    //janky and will need revisions on both ends
                    foreach (GetSPNInfo_Result test in result)
                    {
                        if (test.SPN_Position.Contains("-") || test.SPN_Position.Contains("."))
                        {
                            //regex expression means dash OR period (backslash is escape character)
                            string[] elements = System.Text.RegularExpressions.Regex.Split(test.SPN_Position, @"-|\.");
                            spn.Position = int.Parse(elements[0]);
                        }
                        spn.SPNLength = new SPNLength(test.SPN_Length);
                        canID = new CANid(test);
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }

    }
}
