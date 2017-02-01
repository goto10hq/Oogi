using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oogi;
using Oogi.Tokens;

namespace Tests
{
    [TestClass]
    public class BasicOperation
    {
        private const string _entity = "oogi/robot";
        private static readonly Repository<Robot> _repo = new Repository<Robot>();

        private readonly List<Robot> _robots = new List<Robot>
                         {
                            new Robot("Alfred", 100, true, new List<string> { "CPU", "Laser" }),
                            new Robot("Nausica", 220, true, new List<string> { "CPU", "Bio scanner", "DSP" }),
                            new Robot("Kosuna", 190, false, new List<string>()) { Message = @"\'\\''" }
                         };

        public class Robot : BaseEntity
        {            
            public override string Id { get; set; }
            public override string Entity { get; set; } = _entity;

            public string Name { get; set; }
            public int ArtificialIq { get; set; }
            public Stamp Created { get; set; } = new Stamp();
            public bool IsOperational { get; set; }
            public List<string> Parts { get; set; } = new List<string>();
            public string Message { get; set; }

            public Robot()
            {                
            }

            public Robot(string name, int artificialIq, bool isOperational, List<string> parts)
            {
                Name = name;
                ArtificialIq = artificialIq;                
                IsOperational = isOperational;
                Parts = parts;
            }
        }

        [TestInitialize]
        public void CreateRobots()
        {               
            foreach (var robot in _robots)
                _repo.Create(robot);
        }

        [TestCleanup]
        public void DeleteRobots()
        {
            var robots = _repo.GetAll();

            foreach (var robot in robots)
            {
                _repo.Delete(robot);
            }
        }

        [TestMethod]
        public void SelectAll()
        {                                    
            var robots = _repo.GetAll();
           
            Assert.AreEqual(_robots.Count, robots.Count);            
        }

        [TestMethod]
        public void SelectList()
        {            
            var q = new SqlQuerySpec("select * from c where c.entity = @entity and c.artificialIq > @iq",
                new SqlParameterCollection
                {
                    new SqlParameter("@entity", _entity),
                    new SqlParameter("@iq", 120)
                });
            var robots = _repo.GetList(q);

            Assert.AreEqual(_robots.Count(x => x.ArtificialIq > 120), robots.Count);            
        }

        [TestMethod]
        public void SelectListDynamic()
        {            
            var robots = _repo.GetList("select * from c where c.entity = @entity and c.artificialIq > @iq",
                new
                {
                    entity = _entity,
                    iq = 120
                });

            Assert.AreEqual(_robots.Count(x => x.ArtificialIq > 120), robots.Count);
        }

        [TestMethod]
        public void SelectFirstOrDefault()
        {
            var robot = _repo.GetFirstOrDefault();

            Assert.AreNotEqual(robot, null);
            Assert.AreEqual(100, robot.ArtificialIq);

            var q = new SqlQuerySpec("select * from c where c.entity = @entity and c.artificialIq = @iq")
            {
                Parameters = new SqlParameterCollection
                                     {
                                         new SqlParameter("@entity", _entity),
                                         new SqlParameter("@iq", 190)
                                     }
            };

            robot = _repo.GetFirstOrDefault(q);

            Assert.AreNotEqual(robot, null);
            Assert.AreEqual(190, robot.ArtificialIq);
            
            robot = _repo.GetFirstOrDefault("select * from c where c.entity = @entity and c.artificialIq = @iq",
                new
                {
                    entity = _entity,
                    iq = 190
                });

            Assert.AreNotEqual(robot, null);
            Assert.AreEqual(190, robot.ArtificialIq);
        }

        [TestMethod]
        public void SelectEscaped()
        {
            var q = new SqlQuerySpec("select * from c where c.entity = @entity and c.message = @message")
                    {
                        Parameters = new SqlParameterCollection
                                     {
                                         new SqlParameter("@entity", _entity),
                                         new SqlParameter("@message", @"\'\\''")
                                     }
                    };


            var robot = _repo.GetFirstOrDefault(q);            
            Assert.AreNotEqual(robot, null);

            if (robot != null)
            {
                var oldId = robot.Id;
                _repo.GetFirstOrDefault(oldId);                

                Assert.AreNotEqual(robot, null);
                Assert.AreEqual(robot.Id, oldId);
                
            }
        }

        [TestMethod]
        public void Delete()
        {            
            var robots = _repo.GetList("select * from c where c.entity = @entity order by c.artificialIq", new { entity = _entity });

            Assert.AreEqual(_robots.Count, robots.Count);

            var dumbestRobotId = robots[0].Id;

            _repo.Delete(dumbestRobotId);

            var smartestRobot = robots[robots.Count - 1];

            _repo.Delete(smartestRobot);

            robots = _repo.GetAll();

            Assert.AreEqual(1, robots.Count);
            Assert.AreEqual(_robots.OrderBy(x => x.ArtificialIq).Skip(1).First().ArtificialIq, robots[0].ArtificialIq);
        }
    }
}
