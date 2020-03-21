/*
 * FILE          : MessageInfo.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : March 16 2020
 */


namespace J1939Converter
{


    /*
     * NAME    : MessageInfo
     * PURPOSE : The MessageInfo class has been created to hold the information
     *              that is being displayed back to the user on the MainPageView
     */
    public class MessageInfo
    {
        public SPN Spn { get; set; }
        public CANid Can { get; set; }
        public string Message { get; set; }
        public bool ErrorFound { get; set; }





        /*
         * FUNCTION    : MessageInfo
         * DESCRIPTION : Default constructor, creates an empty MessageInfo with 
         *                  a message of "Testing"
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public MessageInfo()
        {
            Spn = new SPN();
            Can = new CANid(new DBEntity.GetSPNInfo_Result());
            Message = "Testing";
            ErrorFound = false;
        }





        /*
         * FUNCTION    : MessageInfo
         * DESCRIPTION : A constructor that creates a MessageInfo with the given values
         * PARAMETERS  : SPN spn        - The spn to be added to the message info
         *               CANid can      - The CANid to be added to the message info
         *               string message - The message being sent
         *               bool error     - If there was an error (Default valule is false)
         * RETURNS     : NONE
         */
        public MessageInfo(SPN spn, CANid can, string message, bool error = false)
        {
            Spn = spn;
            Can = can;
            Message = message;
            ErrorFound = error;
        }





        /*
         * FUNCTION    : FillInfo
         * DESCRIPTION : Fills and empty Message info with the given values
         * PARAMETERS  : SPN spn        - The spn to be added to the message info
         *               CANid can      - The CANid to be added to the message info
         *               string message - The message being sent
         *               bool error     - If there was an error (Default valule is false)
         * RETURNS     : NONE
         */
        public void FillInfo(SPN spn, CANid can, string message, bool error = false)
        {
            Spn = spn;
            Can = can;
            Message = message;
            ErrorFound = error;
        }





        /*
         * FUNCTION    : Error
         * DESCRIPTION : Sets the message of a MessageInfo to the given string and then
         *                  sets the ErrorFound value to true
         * PARAMETERS  : string message - the error message to be saved
         * RETURNS     : NONE
         */
        public void Error(string message)
        {
            Message = message;
            ErrorFound = true;
        }
    }
}
