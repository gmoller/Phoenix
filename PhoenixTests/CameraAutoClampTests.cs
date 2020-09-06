using Input;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using PhoenixGamePresentation.Views;
using Utilities;

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
            var worldView = new WorldView(world, CameraClampMode.AutoClamp, new InputHandler());
            _camera = worldView.Camera;
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_0()
        {
            // Act
            _camera.LookAtCell(PointI.Zero);

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Vector2(840.0f, 540.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_60_0()
        {
            // Act
            _camera.LookAtCell(new PointI(60, 0));

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(6655, _camera.CameraRectangleInWorld.Right);
            Assert.AreEqual(new Vector2(5815.0f, 540.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_40()
        {
            // Act
            _camera.LookAtCell(new PointI(0, 40));

            // Assert
            Assert.AreEqual(3872, _camera.CameraRectangleInWorld.Bottom);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Vector2(840.0f, 3332.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_60_40()
        {
            // Act
            _camera.LookAtCell(new PointI(60, 40));

            // Assert
            Assert.AreEqual(3872, _camera.CameraRectangleInWorld.Bottom);
            Assert.AreEqual(6655, _camera.CameraRectangleInWorld.Right);
            Assert.AreEqual(new Vector2(5815.0f, 3332.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_in_and_looking_at_cell_0_0()
        {
            // Act
            _camera.Zoom = 2.0f;
            _camera.LookAtCell(PointI.Zero);

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Vector2(420.0f, 270.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_out_and_looking_at_cell_0_0()
        {
            // Act
            _camera.Zoom = 0.5f;
            _camera.LookAtCell(PointI.Zero);

            // Assert
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Top);
            Assert.AreEqual(0, _camera.CameraRectangleInWorld.Left);
            Assert.AreEqual(new Vector2(1680.0f, 1080.0f), _camera.CameraFocusPointInWorld);
        }
    }
}