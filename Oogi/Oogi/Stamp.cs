using System;
using Sushi;

namespace Oogi
{
    public class Stamp
    {
        public DateTime DateTime { get; set; }
        public int Epoch => DateTime.ToEpoch();
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
