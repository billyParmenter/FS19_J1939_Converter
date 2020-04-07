/*
 * FILE          : Converter.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */


using J1939Converter.Communication;
using J1939Converter.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading;


namespace J1939Converter
{


    /*
     * NAME    : Converter
     * PURPOSE : The Converter class has been created to convert given SPN 
     *              info, CANDid info, and a value into a J1939 message. This
     *              class will get a key value pair from the output file of
     *              the farming simulator mod and query the database for the 
     *              corresponding SPN and PGN info. Using that info it will
     *              then convert it to a J1939 protocal message.
     */
    internal class Converter
    {
        private static SPN _spn;
        private static CANid _canID;
        public static List<SPN> spnList = new List<SPN>();
        public static SocketClient socketClient;

        // the lengths of each part of the canid
        private static readonly int[] dataLengths = new int[7] { 3, 1, 1, 8, 8, 8, 8 };

        private enum DataNames // The canID parts
        {
            priority = 0,
            reserved = 1,
            dataPage = 2,
            pduFormat = 3,
            pduSpecific = 4,
            sourceAddress = 5,
            total = 6
        };



        /*
         * FUNCTION    : Converter
         * DESCRIPTION : This is the default custructor, it calls the init method, if
         *                  the init is not successfull then this constructor will
         *                  throw an exception
         * PARAMETERS  : NONE
         * RETURNS     : NONE
         */
        public Converter()
        {
            

        }

        public void Stop(bool close = false)
        {
            SocketClient.Stop();
            if (socketClient != null && close == true)
            {
                socketClient.Close();
            }
        }





        /*
         * METHOD      : ConvertToJ1939
         * DESCRIPTION : Takes the spn number and value, gets the required info from the database,
         *                  and converts them to a J1939 hex value string
         * PARAMETERS  : SPN spn - the spn to convert
         * RETURNS     : string - the converted string
         */
        public static string ConvertToJ1939(SPN spn, ref CANid canID)
        {
            Logger.Log(Logger.ErrorLevel.INFO, "Generating J1939 Message...");
            _spn = spn;

            Logger.Log(Logger.ErrorLevel.INFO, "Getting spn and can id info from database...");
            _canID = canID;

            Logger.Log(Logger.ErrorLevel.INFO, "Generating CANid...");
            string canIdString = GenerateCanID();

            Logger.Log(Logger.ErrorLevel.INFO, "Generating data message...");
            string data = GenerateDataMessage();

            canID = _canID;

            Logger.Log(Logger.ErrorLevel.INFO, "Message generated: " + canIdString + " " + data);
            return canIdString + " " + data;
        }





        /*
         * METHOD      : GenerateMessage
         * DESCRIPTION : Converts the spn value into a hex string data string
         * PARAMETERS  : NONE
         * RETURNS     : string - the Hex value string
         */
        private static string GenerateDataMessage()
        {
            int val = Convert.ToInt32(_spn.Value / _canID.Resolution.value);
            string hex = val.ToString("X").PadLeft(_spn.SPNLength.value * 2, '0');

            hex = hex.PadLeft((_spn.Position - 1 + _spn.SPNLength.value) * 2, 'F');
            hex = hex.PadRight(16, 'F');

            Logger.Log(Logger.ErrorLevel.INFO, "Data generated: " + hex);
            return hex;
        }





        /*
         * METHOD      : GenerateCanID
         * DESCRIPTION : Converts the info from the database for the canID to A Hex value string
         * PARAMETERS  : NONE
         * RETURNS     : string - The converted Hex canID
         */
        private static string GenerateCanID()
        {
            bool isLittleEndian = BitConverter.IsLittleEndian;

            if (isLittleEndian == false)
            {
                string message = "This devices endian ness is not little endian! Results may vary";
                Console.WriteLine(message);
                Logger.Log(Logger.ErrorLevel.WARN, message);
            }

            string binCANid = Convert.ToString(_canID.Priority, 2).PadLeft(dataLengths[(int)DataNames.priority], '0');
            binCANid += Convert.ToString(_canID.Reserved, 2).PadLeft(dataLengths[(int)DataNames.reserved], '0');
            binCANid += Convert.ToString(_canID.DataPage, 2).PadLeft(dataLengths[(int)DataNames.dataPage], '0');
            binCANid += Convert.ToString(_canID.PduFormat, 2).PadLeft(dataLengths[(int)DataNames.pduFormat], '0');
            binCANid += Convert.ToString(_canID.PduSpecific, 2).PadLeft(dataLengths[(int)DataNames.pduSpecific], '0');
            binCANid += Convert.ToString(_canID.SourceAddress, 2).PadLeft(dataLengths[(int)DataNames.sourceAddress], '0');

            string convertedString = Convert.ToInt32(binCANid, 2).ToString("X").PadLeft(dataLengths[(int)DataNames.total], '0');

            Logger.Log(Logger.ErrorLevel.INFO, "CAN id generated: " + convertedString);
            return convertedString;
        }





        /*
         * FUNCTION    : DoConvert
         * DESCRIPTION : This is the main method of this program. It will get the 
         *                  latest values from the file, check that they are what
         *                  is wanted to track based on the spn list and then 
         *                  convert the message. The method will then send it on
         *                  the socket and add it to the observable collectio to 
         *                  be sent back to the UI
         * PARAMETERS  : NONE
         * RETURNS     : ObservableCollection<MessageInfo> - The messages to be 
         *                  added to the UI
         */
        public ObservableCollection<MessageInfo> DoConvert()
        {
            ObservableCollection<MessageInfo> messageInfos = new ObservableCollection<MessageInfo>();
            Dictionary<string, double> latestValues = FileReader.ReadLatest();
            MessageInfo messageInfo = new MessageInfo();


            if (latestValues.ContainsKey("FNF") == false)
            {
                foreach (KeyValuePair<string, double> pair in latestValues)
                {
                    messageInfo = new MessageInfo();
                    string spnKey = pair.Key;
                    double value = pair.Value;

                    foreach (SPN spn in spnList)
                    {
                        if (spnKey == spn.SpnKey)
                        {
                            int spnNumber = spnList.Find(_ => _.SpnKey == spnKey).SpnNumber;

                            SPN newSpn;
                            CANid canID;

                            if (ConfigurationManager.AppSettings["DatabaseAvailable"] == "true")
                            {
                                Logger.Log(Logger.ErrorLevel.INFO, "Using database");
                                newSpn = new SPN() { SpnKey = spnKey, SpnNumber = spnNumber, Value = value };
                                canID = Database.GetSPN(ref newSpn);
                            }
                            else
                            {
                                Logger.Log(Logger.ErrorLevel.WARN, "Database is not being used, messages are not reliable!");
                                newSpn = new SPN() { SpnKey = spnKey, SpnNumber = spnNumber, Value = value };
                                canID = new CANid();
                            }
                            string J1939string = Converter.ConvertToJ1939(newSpn, ref canID);
                            socketClient.Send(J1939string);
                            messageInfo.FillInfo(newSpn, canID, J1939string);
                            messageInfos.Add(messageInfo);
                        }
                    }

                }
            }
            else
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "File not found");
                messageInfo.Error("File not found");
            }
            Thread.Sleep(1000);

            return messageInfos;

        }





        /*
         * FUNCTION    : Init
         * DESCRIPTION : Initializes the logger and gets the spns
         * PARAMETERS  : NONE
         * RETURNS     : string - Null if successfull or the error message
         */
        public string Init()
        {
            try
            {

                Logger.InitializeLogger();
                Logger.Log(Logger.ErrorLevel.INFO, "Logger initialized");
                Logger.Log(Logger.ErrorLevel.INFO, "Getting spns");
                spnList = SPN.GetSPNs();
                socketClient = new SocketClient();
                return null;
            }
            catch (Exception e)
            {
                Logger.Log(Logger.ErrorLevel.ERROR, "Exception trying to initialize converter", e);
                return e.Message;
            }
        }
    }
}
