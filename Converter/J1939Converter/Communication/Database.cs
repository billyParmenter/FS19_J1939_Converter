/*
* FILE			:	Database.cs
* PROJECT		:   Ji1939Convereter
* PROGRAMMER	:   Connor Lynch and Billy Parmenter
* FIRST VERSION :   
* DESCRIPTION	:   Establishes communications to the DB to gatehr data
*                   
*/
using System.Text.RegularExpressions;
using DBEntity;

namespace J1939Converter.Communication
{
    class Database
    {
        public static CANid GetSPN(ref J1939Converter.SPN spn)
        {



             System.Data.Entity.Core.Objects.ObjectResult<GetSPNInfo_Result> result;
            CANid canID = null;
            using (Entities db = new Entities())

                result = db.GetSPNInfo(spn.spnNumber);

                //janky and will need revisions on both ends
                foreach (GetSPNInfo_Result test in result)
                {
                    if (test.SPN_Position.Contains("-") || test.SPN_Position.Contains("."))
                    {
                        //regex expression means dash OR period (backslash is escape character)
                        string[] elements = System.Text.RegularExpressions.Regex.Split(test.SPN_Position,@"-|\.");
                        spn.position = int.Parse(elements[0]); 
                    }
                    spn.length = 1;
                    spn.testSPNLength = new SPNLength(test.SPN_Length);
                    canID = new CANid(test);

                }

            return canID;

        }

    }
}
