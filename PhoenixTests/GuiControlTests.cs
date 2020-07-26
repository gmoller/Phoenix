using Microsoft.VisualStudio.TestTools.UnitTesting;
using GuiControls;
using Microsoft.Xna.Framework;
using Utilities;
using Point = Microsoft.Xna.Framework.Point;

namespace PhoenixTests
{
    [TestClass]
    public class GuiControlTests
    {
        [TestMethod]
        public void Test_image_creation_top_left_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(100.0f, 100.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(100, 100), image.TopLeft);
            Assert.AreEqual(new Point(200, 100), image.TopRight);
            Assert.AreEqual(new Point(100, 200), image.BottomLeft);
            Assert.AreEqual(new Point(200, 200), image.BottomRight);
            Assert.AreEqual(new Point(150, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);

            image.TopLeftPosition -= new Vector2(100.0f, 100.0f);
            Assert.AreEqual(new Vector2(0.0f, 0.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(0, 0), image.TopLeft);
            Assert.AreEqual(new Point(100, 0), image.TopRight);
            Assert.AreEqual(new Point(0, 100), image.BottomLeft);
            Assert.AreEqual(new Point(100, 100), image.BottomRight);
            Assert.AreEqual(new Point(50, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_top_center_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.TopCenter, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(50.0f, 100.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(50, 100), image.TopLeft);
            Assert.AreEqual(new Point(150, 100), image.TopRight);
            Assert.AreEqual(new Point(50, 200), image.BottomLeft);
            Assert.AreEqual(new Point(150, 200), image.BottomRight);
            Assert.AreEqual(new Point(100, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(50, image.Left);
            Assert.AreEqual(150, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_top_right_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.TopRight, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(0.0f, 100.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(0, 100), image.TopLeft);
            Assert.AreEqual(new Point(100, 100), image.TopRight);
            Assert.AreEqual(new Point(0, 200), image.BottomLeft);
            Assert.AreEqual(new Point(100, 200), image.BottomRight);
            Assert.AreEqual(new Point(50, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
        }



        [TestMethod]
        public void Test_image_creation_middle_left_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.MiddleLeft, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(100.0f, 50.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(100, 50), image.TopLeft);
            Assert.AreEqual(new Point(200, 50), image.TopRight);
            Assert.AreEqual(new Point(100, 150), image.BottomLeft);
            Assert.AreEqual(new Point(200, 150), image.BottomRight);
            Assert.AreEqual(new Point(150, 100), image.Center);
            Assert.AreEqual(50, image.Top);
            Assert.AreEqual(150, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_middle_center_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.MiddleCenter, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(50.0f, 50.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(50, 50), image.TopLeft);
            Assert.AreEqual(new Point(150, 50), image.TopRight);
            Assert.AreEqual(new Point(50, 150), image.BottomLeft);
            Assert.AreEqual(new Point(150, 150), image.BottomRight);
            Assert.AreEqual(new Point(100, 100), image.Center);
            Assert.AreEqual(50, image.Top);
            Assert.AreEqual(150, image.Bottom);
            Assert.AreEqual(50, image.Left);
            Assert.AreEqual(150, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_middle_right_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.MiddleRight, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(0.0f, 50.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(0, 50), image.TopLeft);
            Assert.AreEqual(new Point(100, 50), image.TopRight);
            Assert.AreEqual(new Point(0, 150), image.BottomLeft);
            Assert.AreEqual(new Point(100, 150), image.BottomRight);
            Assert.AreEqual(new Point(50, 100), image.Center);
            Assert.AreEqual(50, image.Top);
            Assert.AreEqual(150, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
        }



        [TestMethod]
        public void Test_image_creation_bottom_left_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.BottomLeft, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(100.0f, 0.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(100, 0), image.TopLeft);
            Assert.AreEqual(new Point(200, 0), image.TopRight);
            Assert.AreEqual(new Point(100, 100), image.BottomLeft);
            Assert.AreEqual(new Point(200, 100), image.BottomRight);
            Assert.AreEqual(new Point(150, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_bottom_center_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.BottomCenter, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(50.0f, 0.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(50, 0), image.TopLeft);
            Assert.AreEqual(new Point(150, 0), image.TopRight);
            Assert.AreEqual(new Point(50, 100), image.BottomLeft);
            Assert.AreEqual(new Point(150, 100), image.BottomRight);
            Assert.AreEqual(new Point(100, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(50, image.Left);
            Assert.AreEqual(150, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_bottom_right_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var image = new Image("ImageName", new Vector2(100.0f, 100.0f), ContentAlignment.BottomRight, new Vector2(100.0f, 100.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(0.0f, 0.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(0, 0), image.TopLeft);
            Assert.AreEqual(new Point(100, 0), image.TopRight);
            Assert.AreEqual(new Point(0, 100), image.BottomLeft);
            Assert.AreEqual(new Point(100, 100), image.BottomRight);
            Assert.AreEqual(new Point(50, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_top_left_parent_top_left()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var parent = new Image("ParentImage", new Vector2(100.0f, 100.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", 0);

            var image = new Image("ImageName", new Vector2(50.0f, 50.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", 0, 0.0f, parent);
            Assert.AreEqual(new Vector2(150.0f, 150.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(150, 150), image.TopLeft);
            Assert.AreEqual(new Vector2(250.0f, 150.0f), image.TopRightPosition);
            Assert.AreEqual(new Point(250, 150), image.TopRight);
            Assert.AreEqual(new Vector2(150.0f, 250.0f), image.BottomLeftPosition);
            Assert.AreEqual(new Point(150, 250), image.BottomLeft);
            Assert.AreEqual(new Vector2(250.0f, 250.0f), image.BottomRightPosition);
            Assert.AreEqual(new Point(250, 250), image.BottomRight);
            Assert.AreEqual(new Point(200, 200), image.Center);
            Assert.AreEqual(150, image.Top);
            Assert.AreEqual(250, image.Bottom);
            Assert.AreEqual(150, image.Left);
            Assert.AreEqual(250, image.Right);

            Assert.AreEqual(new Vector2(50.0f, 50.0f), image.RelativePosition);
        }

        [TestMethod]
        public void Test_image_creation_top_left_parent_bottom_right()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var parent = new Image("ParentImage", new Vector2(100.0f, 100.0f), ContentAlignment.BottomRight, new Vector2(100.0f, 100.0f), "Icons_1", 0);

            var image = new Image("ImageName", new Vector2(50.0f, 50.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", 0, 0.0f, parent);
            Assert.AreEqual(new Vector2(50.0f, 50.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(50, 50), image.TopLeft);
            Assert.AreEqual(new Point(150, 50), image.TopRight);
            Assert.AreEqual(new Point(50, 150), image.BottomLeft);
            Assert.AreEqual(new Point(150, 150), image.BottomRight);
            Assert.AreEqual(new Point(100, 100), image.Center);
            Assert.AreEqual(50, image.Top);
            Assert.AreEqual(150, image.Bottom);
            Assert.AreEqual(50, image.Left);
            Assert.AreEqual(150, image.Right);
        }

        [TestMethod]
        public void Test_image_creation_middle_center_no_parent_half_screen_resolution()
        {
            DeviceManager.Instance.SetScreenResolution(960, 540);

            var image = new Image("ImageName", new Vector2(960.0f, 540.0f), ContentAlignment.MiddleCenter, new Vector2(1920.0f, 1080.0f), "Icons_1", 0);
            Assert.AreEqual(new Vector2(0.0f, 0.0f), image.TopLeftPosition);
            Assert.AreEqual(new Point(0, 0), image.TopLeft);
            Assert.AreEqual(new Point(1920, 0), image.TopRight);
            Assert.AreEqual(new Point(0, 1080), image.BottomLeft);
            Assert.AreEqual(new Point(1920, 1080), image.BottomRight);
            Assert.AreEqual(new Point(960, 540), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(1080, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(1920, image.Right);
        }
    }
}