using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oogi;
using Sushi;

namespace Tests
{
    [TestClass]
    public class Various
    {
        
        [TestMethod]
        public void Stamp()
        {
            var now = DateTime.Now;
            var stamp = new Stamp(now);

            Assert.AreEqual(now.Year, stamp.Year);
            Assert.AreEqual(now.Month, stamp.Month);
            Assert.AreEqual(now.Day, stamp.Day);
            Assert.AreEqual(now.Hour, stamp.Hour);
            Assert.AreEqual(now.Minute, stamp.Minute);
            Assert.AreEqual(now.Second, stamp.Second);
            Assert.AreEqual(now.ToEpoch(), stamp.Epoch);            
        }
    }
}
