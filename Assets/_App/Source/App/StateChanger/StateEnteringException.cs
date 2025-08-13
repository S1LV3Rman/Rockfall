using System;

namespace S1LV3Rman.RockFall.App
{
    public class StateEnteringException : Exception
    {
        public StateEnteringException(string message) : base(message)
        {
        }
    }
}