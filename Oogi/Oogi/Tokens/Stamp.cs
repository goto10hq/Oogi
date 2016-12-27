﻿using System;

namespace Oogi.Tokens
{
    public class Stamp : SimpleStamp
    {
        public int Year => DateTime.Year;
        public int Month => DateTime.Month;
        public int Day => DateTime.Day;
        public int Hour => DateTime.Hour;
        public int Minute => DateTime.Minute;
        public int Second => DateTime.Second;

        public Stamp()
        {
            DateTime = DateTime.Now;
        }

        public Stamp(DateTime dt)
        {
            DateTime = dt;
        }
    }
}
