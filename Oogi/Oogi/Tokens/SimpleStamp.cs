using System;
using Sushi;

namespace Oogi.Tokens
{
    public class SimpleStamp : IStamp, IComparable, IFormattable, IComparable<DateTime>, IComparable<IStamp>, IEquatable<DateTime>
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

        public override string ToString()
        {
            return DateTime.ToString();
        }

        public int CompareTo(object obj)
        {
            var stamp = obj as IStamp;

            return stamp != null ? DateTime.CompareTo(stamp.DateTime) : CompareTo(obj);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return DateTime.ToString(format, formatProvider);
        }

        public int CompareTo(DateTime other)
        {
            return DateTime.CompareTo(other);
        }

        public bool Equals(DateTime other)
        {
            return DateTime.Equals(other);
        }

        public override bool Equals(object obj)
        {
            var stamp = obj as IStamp;

            return stamp != null ? DateTime.Equals(stamp.DateTime) : Equals(obj);
        }

        public int CompareTo(IStamp other)
        {
            return DateTime.CompareTo(other);
        }
    }
}