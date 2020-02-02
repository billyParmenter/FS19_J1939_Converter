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
    }
}
