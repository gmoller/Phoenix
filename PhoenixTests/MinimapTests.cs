﻿using Microsoft.Xna.Framework;
using NUnit.Framework;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using PhoenixGamePresentation.Handlers;
using PhoenixGamePresentation.Views;
using Utilities;

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
            var gameMetadata = new GameMetadata();
            var presentationContext = new GlobalContextPresentation();
            CallContext<GameMetadata>.SetData("GameMetadata", gameMetadata);
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);

            var world = new World(60, 40);
            _worldView = new WorldView(world, null);
            var viewport = new Rectangle(0, 0, 1670, 1080);
            var camera = new Camera(_worldView, viewport, CameraClampMode.AutoClamp, null);
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
        public void MinimapViewedRectangle_will_be_set__correctly_when_looking_at_cell_0_0_()
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
        public void MinimapViewedRectangle_will_be_set__correctly_when_looking_at_cell_60_40_()
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