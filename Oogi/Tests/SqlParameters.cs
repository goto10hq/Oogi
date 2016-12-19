using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oogi;

namespace Tests
{
    [TestClass]
    public class SqlParameters
    {        
        [TestMethod]
        public void Classic()
        {
            var q = new SqlQuerySpec("a = @a, b = @b, c = @c, d = @d, e = @e, f = @f",
                new SqlParameterCollection
                {
                    new SqlParameter("@a", "!''!"),
                    new SqlParameter("@b", 'x'),
                    new SqlParameter("@c", null),
                    new SqlParameter("@d", true),
                    new SqlParameter("@e", 13),
                    new SqlParameter("@f", 13.99)
                });

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
        public void ClassicEnum()
        {
            var q = new SqlQuerySpec("state = @state",
                new SqlParameterCollection
                {
                    new SqlParameter("@state", State.Processing),                    
                });

            var sql = q.ToSqlQuery();

            Assert.AreEqual("state = 20", sql);
        }
    }
}
