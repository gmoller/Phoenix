﻿using NUnit.Framework;
using PhoenixGameData;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using PhoenixGamePresentation.Handlers;
using PhoenixGamePresentation.Views;
using Zen.Input;
using Zen.Utilities;

namespace PhoenixTests
{
    [TestFixture]
    public class MinimapTests
    {
        private WorldView _worldView;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var gameMetadata = new GameConfigCache();
            var presentationContext = new GlobalContextPresentation();
            CallContext<GameConfigCache>.SetData("GameMetadata", gameMetadata);
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);

            var world = new World(60, 40, new Faction(1));
            _worldView = new WorldView(world, CameraClampMode.AutoClamp, new InputHandler());
        }

        [Test]
        public void MinimapViewedRectangle_will_be_set__correctly_when_looking_at_cell_0_0()
        {
            _worldView.Camera.LookAtCell(new PointI(0, 0));

            var minimapSize = new PointI(200, 116);
            var minimapViewedRectangle = MinimapHandler.GetViewedRectangle(_worldView, minimapSize);

                Assert.AreEqual(0, minimapViewedRectangle.X);
            Assert.AreEqual(0, minimapViewedRectangle.Y);
            Assert.AreEqual(50, minimapViewedRectangle.Width);
            Assert.AreEqual(32, minimapViewedRectangle.Height);
        }

        [Test]
        public void MinimapViewedRectangle_will_be_set__correctly_when_looking_at_cell_0_0_smaller_minimap()
        {
            _worldView.Camera.LookAtCell(new PointI(0, 0));

            var minimapSize = new PointI(100, 100);
            var minimapViewedRectangle = MinimapHandler.GetViewedRectangle(_worldView, minimapSize);

            Assert.AreEqual(0, minimapViewedRectangle.X);
            Assert.AreEqual(0, minimapViewedRectangle.Y);
            Assert.AreEqual(25, minimapViewedRectangle.Width);
            Assert.AreEqual(27, minimapViewedRectangle.Height);
        }

        [Test]
        public void MinimapViewedRectangle_will_be_set__correctly_when_looking_at_cell_60_40()
        {
            _worldView.Camera.LookAtCell(new PointI(60, 40));

            var minimapSize = new PointI(200, 116);
            var minimapViewedRectangle = MinimapHandler.GetViewedRectangle(_worldView, minimapSize);

            Assert.AreEqual(149, minimapViewedRectangle.X);
            Assert.AreEqual(83, minimapViewedRectangle.Y);
            Assert.AreEqual(50, minimapViewedRectangle.Width);
            Assert.AreEqual(32, minimapViewedRectangle.Height);
        }

        [Test]
        public void MinimapViewedRectangle_will_be_set__correctly_when_looking_at_cell_60_40_smaller_minimap()
        {
            _worldView.Camera.LookAtCell(new PointI(60, 40));

            var minimapSize = new PointI(100, 100);
            var minimapViewedRectangle = MinimapHandler.GetViewedRectangle(_worldView, minimapSize);

            Assert.AreEqual(75, minimapViewedRectangle.X);
            Assert.AreEqual(73, minimapViewedRectangle.Y);
            Assert.AreEqual(25, minimapViewedRectangle.Width);
            Assert.AreEqual(27, minimapViewedRectangle.Height);
        }
    }
}