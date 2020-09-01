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
            var worldView = new WorldView(world, null);
            var viewport = new Rectangle(0, 0, 1670, 1080);
            _camera = new Camera(worldView, viewport, CameraClampMode.NoClamp);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_0()
        {
            // Act
            _camera.LookAtCell(Point.Zero);

            // Assert
            Assert.AreEqual(Point.Zero, _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_1_0()
        {
            // Act
            _camera.LookAtCell(new Point(1, 0));

            // Assert
            Assert.AreEqual(new Point(111, 0), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_2_0()
        {
            // Act
            _camera.LookAtCell(new Point(2, 0));

            // Assert
            Assert.AreEqual(new Point(222, 0), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_1()
        {
            // Act
            _camera.LookAtCell(new Point(0, 1));

            // Assert
            Assert.AreEqual(new Point(55, 96), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_0_2()
        {
            // Act
            _camera.LookAtCell(new Point(0, 2));

            // Assert
            Assert.AreEqual(new Point(0, 192), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_1_1()
        {
            // Act
            _camera.LookAtCell(new Point(1, 1));

            // Assert
            Assert.AreEqual(new Point(166, 96), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_cell_2_2()
        {
            // Act
            _camera.LookAtCell(new Point(2, 2));

            // Assert
            Assert.AreEqual(new Point(222, 192), _camera.CameraFocusPointInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_pixel_220_192()
        {
            // Act
            _camera.LookAtPixel(new Point(220, 192));

            // Assert
            Assert.AreEqual(new Point(220, 192), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new Point(2, 2), _camera.CameraFocusCellInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_pixel_3327_1936()
        {
            // Act
            _camera.LookAtPixel(new Point(3327, 1936));

            // Assert
            Assert.AreEqual(new Point(3327, 1936), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new Point(30, 20), _camera.CameraFocusCellInWorld);
            Assert.AreEqual(new Rectangle(2492, 1396, 1670, 1080), _camera.CameraRectangleInWorld);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_looking_at_pixel_835_540()
        {
            // Act
            _camera.Zoom = 1.0f;
            _camera.LookAtPixel(new Point(835, 540));

            // Assert
            Assert.AreEqual(new Point(835, 540), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new Point(7, 5), _camera.CameraFocusCellInWorld);
            Assert.AreEqual(new Rectangle(0, 0, 1670, 1080), _camera.CameraRectangleInWorld);

            Assert.AreEqual(9, _camera.NumberOfHexesToLeft);
            Assert.AreEqual(9, _camera.NumberOfHexesToRight);
            Assert.AreEqual(7, _camera.NumberOfHexesAbove);
            Assert.AreEqual(7, _camera.NumberOfHexesBelow);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_in_and_looking_at_pixel_835_540()
        {
            // Act
            _camera.Zoom = 2.0f;
            _camera.LookAtPixel(new Point(835, 540));

            // Assert
            Assert.AreEqual(new Point(835, 540), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new Point(7, 5), _camera.CameraFocusCellInWorld);
            Assert.AreEqual(new Rectangle(418, 270, 1670, 1080), _camera.CameraRectangleInWorld);

            Assert.AreEqual(5, _camera.NumberOfHexesToLeft);
            Assert.AreEqual(5, _camera.NumberOfHexesToRight);
            Assert.AreEqual(4, _camera.NumberOfHexesAbove);
            Assert.AreEqual(4, _camera.NumberOfHexesBelow);
        }

        [Test]
        public void Camera_will_set_focus_point_correctly_when_zoomed_out_and_looking_at_pixel_835_540()
        {
            // Act
            _camera.Zoom = 0.5f;
            _camera.LookAtPixel(new Point(835, 540));

            // Assert
            Assert.AreEqual(new Point(835, 540), _camera.CameraFocusPointInWorld);
            Assert.AreEqual(new Point(7, 5), _camera.CameraFocusCellInWorld);
            Assert.AreEqual(new Rectangle(-835, -540, 1670, 1080), _camera.CameraRectangleInWorld);

            Assert.AreEqual(17, _camera.NumberOfHexesToLeft);
            Assert.AreEqual(17, _camera.NumberOfHexesToRight);
            Assert.AreEqual(13, _camera.NumberOfHexesAbove);
            Assert.AreEqual(13, _camera.NumberOfHexesBelow);
        }
    }
}