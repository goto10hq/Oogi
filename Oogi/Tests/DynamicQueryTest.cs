using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oogi;
using Oogi.Queries;
using Oogi.Tokens;

namespace Tests
{
    [TestClass]
    public class DynamicQueryTest
    {        
        [TestMethod]
        public void Classic()
        {
            var q = new DynamicQuery<BaseEntity>("a = @a, b = @b, c = @c, d = @d, e = @e, f = @f", new { a = "!''!", b = "x", c = (string)null, d = true, e = 13, f = 13.99 });
                            
            var sql = q.ToSqlQuery();
            
            Assert.AreEqual("a = '!\\'\\'!', b = 'x', c = null, d = true, e = 13, f = 13.99", sql);            
        }

        public enum State
        {
            Ready = 10,
            Processing = 20,
            Finished = 30
        }

        [TestMethod]
        public void Enum()
        {
            var q = new DynamicQuery<BaseEntity>("state = @state", new { state = State.Processing });
                
            var sql = q.ToSqlQuery();

            Assert.AreEqual("state = 20", sql);
        }

        [TestMethod]
        public void Stamps()
        {
            var q = new DynamicQuery<BaseEntity>("epoch = @stamp, epoch2 in (@stamp2)", new
                                                                                        {
                                                                                            stamp = new Stamp(new DateTime(2000, 1, 1)),
                                                                                            stamp2 = new SimpleStamp(new DateTime(2001, 1, 1))
                                                                                        });
                
            var sql = q.ToSqlQuery();

            Assert.AreEqual("epoch = 946684800, epoch2 in (9466848002)", sql);
        }

        [TestMethod]
        public void ListOfInts()
        {
            var ids = new List<int> { 4, 5, 2 };

            var q = new DynamicQuery<BaseEntity>("items in @ids", new { ids = ids });
                
            var sql = q.ToSqlQuery();

            Assert.AreEqual("items in (4,5,2)", sql);
        }

        
        [TestMethod]
        public void EmptyList()
        {
            var q = new DynamicQuery<BaseEntity>("items in @ids", new { ids = new List<int>() });
                
            var sql = q.ToSqlQuery();

            Assert.AreEqual("items in (null)", sql);
        }
    }
}
