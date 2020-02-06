
namespace J1939Converter.Communication
{
    class Database
    {
        public static CANid GetSPN(ref SPN spn)
        {
            spn.length = 2;     //As is in the DB
            spn.position = 2;   //2-3 in DB just need first number

            CANid canID = new CANid
            {
                PGN = 65097,
                sourceAddress = 240,
                pduSpecific = 73,
                pduFormat = 254,
                dataPage = 0,
                reserved = 0,
                priority = 3,
                resolution = (1.0 / 256.0) // 1/256 km/h per bit

            };

            return canID;
        }
    }
}
