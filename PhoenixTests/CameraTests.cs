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
    public class CameraTests
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
            var worldView = new WorldView(world, CameraClampMode.NoClamp, new InputHandler());
            _camera = worldView.Camera;
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_0()
        {
            // Act
            _camera.LookAtCell(PointI.Zero);

            // Assert
            Assert.AreEqual(Vector2.Zero, _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_1_0()
        {
            // Act
            _camera.LookAtCell(new PointI(1, 0));

            // Assert
            Assert.AreEqual(new Vector2(110.85125f, 0.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_2_0()
        {
            // Act
            _camera.LookAtCell(new PointI(2, 0));

            // Assert
            Assert.AreEqual(new Vector2(221.7025f, 0.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_1()
        {
            // Act
            _camera.LookAtCell(new PointI(0, 1));

            // Assert
            Assert.AreEqual(new Vector2(55.425625f, 96.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_2()
        {
            // Act
            _camera.LookAtCell(new PointI(0, 2));

            // Assert
            Assert.AreEqual(new Vector2(0, 192), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_1_1()
        {
            // Act
            _camera.LookAtCell(new PointI(1, 1));

            // Assert
            Assert.AreEqual(new Vector2(166.27687f, 96.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_2_2()
        {
            // Act
            _camera.LookAtCell(new PointI(2, 2));

            // Assert
            Assert.AreEqual(new Vector2(221.7025f, 192.0f), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_pixel_220_192()
        {
            // Act
            _camera.LookAtPixel(new PointI(220, 192));

            // Assert
            Assert.AreEqual(new Vector2(220, 192), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new PointI(2, 2), _camera.CameraFocusHexInWorld.ToPointI());
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_pixel_3327_1936()
        {
            // Act
            _camera.LookAtPixel(new PointI(3327, 1936));

            // Assert
            Assert.AreEqual(new Vector2(3327, 1936), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new PointI(30, 20), _camera.CameraFocusHexInWorld.ToPointI());
            Assert.AreEqual(new Rectangle(2487, 1396, 1680, 1080), _camera.CameraRectangleInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_pixel_835_540()
        {
            // Arrange
            _camera.Zoom = 1.0f;

            // Act
            _camera.LookAtPixel(new PointI(840, 540));

            // Assert
            Assert.AreEqual(new Vector2(840, 540), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new PointI(8, 6), _camera.CameraFocusHexInWorld.ToPointI());
            Assert.AreEqual(new Rectangle(0, 0, 1680, 1080), _camera.CameraRectangleInWorld);

            Assert.AreEqual(new PointI(0, 0), _camera.CameraTopLeftHex);
            Assert.AreEqual(new PointI(15, 11), _camera.CameraBottomRightHex);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_in_and_looking_at_pixel_835_540()
        {
            // Arrange
            _camera.Zoom = 2.0f;

            // Act
            _camera.LookAtPixel(new PointI(840, 540));

            // Assert
            Assert.AreEqual(new Vector2(840, 540), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new PointI(8, 6), _camera.CameraFocusHexInWorld.ToPointI());
            Assert.AreEqual(new Rectangle(420, 270, 840, 540), _camera.CameraRectangleInWorld);

            Assert.AreEqual(new PointI(3, 3), _camera.CameraTopLeftHex);
            Assert.AreEqual(new PointI(11, 9), _camera.CameraBottomRightHex);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_out_and_looking_at_pixel_835_540()
        {
            // Arrange
            _camera.Zoom = 0.5f;

            // Act
            _camera.LookAtPixel(new PointI(840, 540));

            // Assert
            Assert.AreEqual(new Vector2(840, 540), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new PointI(8, 6), _camera.CameraFocusHexInWorld.ToPointI());
            Assert.AreEqual(new Rectangle(-840, -540, 3360, 2160), _camera.CameraRectangleInWorld);

            Assert.AreEqual(new PointI(-8, -6), _camera.CameraTopLeftHex);
            Assert.AreEqual(new PointI(22, 17), _camera.CameraBottomRightHex);
        }
    }
}