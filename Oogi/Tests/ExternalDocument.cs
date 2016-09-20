using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oogi;

namespace Tests
{
    [TestClass]
    public class ExternalDocument
    {
        public class Dummy : BaseEntity
        {
            private const string _entity = "oogi/tests";

            public override string Id { get; set; }
            public override string Entity { get; set; } = _entity;

            public string Name { get; set; }
        }

        [TestMethod]
        public void UpsertExternalDocument()
        {
            var file = File.ReadAllText("document.json");
            var con = new Connection();
            con.UpsertJson(file);

            var repo = new Repository<Dummy>();
            var dummy = repo.GetFirstOrDefault();

            Assert.AreNotEqual(dummy, null);
            Assert.AreEqual(dummy.Entity, "oogi/tests");

            repo.Delete(dummy);

            dummy = repo.GetFirstOrDefault();

            Assert.AreEqual(dummy, null);
        }
    }
}
