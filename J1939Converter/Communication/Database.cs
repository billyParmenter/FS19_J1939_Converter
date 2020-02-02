
namespace J1939Converter.Communication
{
    class Database
    {
        public static CANid GetSPN(ref SPN spn)
        {
            spn.length = 1;
            spn.position = 4;

            CANid canID = new CANid
            {
                PGN = 65097,
                sourceAddress = 240,
                pduSpecific = 73,
                pduFormat = 254,
                dataPage = 0,
                reserved = 0,
                priority = 3,
                resolution = 0.4
            };

            return canID;
        }
    }
}
