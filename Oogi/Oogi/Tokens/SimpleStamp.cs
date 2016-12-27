using System;
using Sushi;

namespace Oogi.Tokens
{
    public class SimpleStamp
    {
        public DateTime DateTime { get; set; }
        public int Epoch => DateTime.ToEpoch();

        public SimpleStamp()
        {
            DateTime = DateTime.Now;
        }

        public SimpleStamp(DateTime dt)
        {
            DateTime = dt;
        }
    }
}