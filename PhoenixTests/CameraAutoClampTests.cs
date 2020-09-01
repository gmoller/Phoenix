    using Microsoft.Xna.Framework;
using NUnit.Framework;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using PhoenixGamePresentation.Views;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixTests
{
    [TestFixture]
    public class CameraAutoClampTests
    {
        private Camera _camera;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var gameMetadata = new GameMetadata();
            var presentationContext = new GlobalContextPresentation();
            CallContext<GameMetadata>.SetData("GameMetadata", gameMetadata);
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);

            var world = new World(60, 40);
            var worldView = new WorldView(world, null);
            var viewport = new Rectangle(0, 0, 1670, 1080);
            _camera = new Camera(worldView, viewport, CameraClampMode.AutoClamp);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_0()
        {
            // Act
            _camera.LookAtCell(Point.Zero);

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Point(835, 540), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_60_0()
        {
            // Act
            _camera.LookAtCell(new Point(60, 0));

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(6655, _camera.CameraRectangleInWorld.Right);
            Assert.AreEqual(new Point(5820, 540), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_40()
        {
            // Act
            _camera.LookAtCell(new Point(0, 40));

            // Assert
            Assert.AreEqual(3872, _camera.CameraRectangleInWorld.Bottom);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Point(835, 3332), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_60_40()
        {
            // Act
            _camera.LookAtCell(new Point(60, 40));

            // Assert
            Assert.AreEqual(3872, _camera.CameraRectangleInWorld.Bottom);
            Assert.AreEqual(6655, _camera.CameraRectangleInWorld.Right);
            Assert.AreEqual(new Point(5820, 3332), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_in_and_looking_at_cell_0_0()
        {
            // Act
            _camera.Zoom = 2.0f;
            _camera.LookAtCell(Point.Zero);

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Point(417, 270), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_out_and_looking_at_cell_0_0()
        {
            // Act
            _camera.Zoom = 0.5f;
            _camera.LookAtCell(Point.Zero);

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Point(1670, 1080), _camera.CameraFocusPointInWorld);
        }
    }
}