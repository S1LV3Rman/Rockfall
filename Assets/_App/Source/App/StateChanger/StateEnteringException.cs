using System;

namespace S1LV3Rman.RockFall
{
    public class StateEnteringException : Exception
    {
        public StateEnteringException(string message) : base(message)
        {
        }
    }
}