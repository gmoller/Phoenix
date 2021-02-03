using System;
using NUnit.Framework;
using PhoenixGameConfig;
using Zen.Utilities;

namespace PhoenixTests
{
    public class GameConfigTests
    {
        private GameConfigRepository _repo;

        [SetUp]
        public void Setup()
        {
            if (_repo != null) return;

            _repo = new GameConfigRepository();

            CallContext<GameConfigRepository>.SetData("GameConfigRepository", _repo);
        }

        [Test]
        public void Action_can_be_fetched()
        {
            var actions1 = _repo.GetEntities("Action");

            var action1 = actions1.GetById(1);
            var action2 = actions1.GetByName("Patrol");
            //var actions2 = _repo.GetActionsThatApplyToAll();

            Assert.AreEqual("Done", action1.Name);
            Assert.AreEqual("Patrol", action2.Name);
            Assert.AreEqual(7, actions1.Count);
            //Assert.AreEqual(5, actions2.Count);

            foreach (var item in actions1)
            {
                Console.WriteLine($"Action name: {item.Name}");
            }
        }

        [Test]
        public void Race_can_be_fetched()
        {
            var races = _repo.GetEntities("Race");

            var race1 = races.GetById(1);
            var race2 = races.GetByName("High Men");

            Assert.AreEqual("Barbarians", race1.Name);
            Assert.AreEqual("High Men", race2.Name);
        }

        [Test]
        public void Unit_can_be_fetched()
        {
            var units = _repo.GetEntities("Unit");

            var unit = units.GetById(1);

            Assert.AreEqual("Barbarian Settlers", unit.Name);
        }
    }
}