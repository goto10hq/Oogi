using System;
using Sushi;

namespace Oogi.Tokens
{
    public class SimpleStamp : IStamp
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