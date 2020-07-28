using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using GuiControls;
using Utilities;
using Point = Microsoft.Xna.Framework.Point;

namespace PhoenixTests
{
    [TestClass]
    public class GuiControlTests_Frame
    {
        [TestMethod]
        public void Test_frame_creation_top_left_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            IControl frame = new Frame("FrameName", new Vector2(100.0f, 100.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", 10, 10, 10, 10);
            Assert.AreEqual(new Point(100, 100), frame.TopLeft);
            Assert.AreEqual(new Point(200, 100), frame.TopRight);
            Assert.AreEqual(new Point(100, 200), frame.BottomLeft);
            Assert.AreEqual(new Point(200, 200), frame.BottomRight);
            Assert.AreEqual(new Point(150, 150), frame.Center);
            Assert.AreEqual(100, frame.Top);
            Assert.AreEqual(200, frame.Bottom);
            Assert.AreEqual(100, frame.Left);
            Assert.AreEqual(200, frame.Right);
            Assert.AreEqual(100, frame.Width);
            Assert.AreEqual(100, frame.Height);
            Assert.AreEqual(new Point(100, 100), frame.Size);
        }

        [TestMethod]
        public void Test_frame_creation_bottom_right_no_parent()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            IControl frame = new Frame("FrameName", new Vector2(100.0f, 100.0f), ContentAlignment.BottomRight, new Vector2(100.0f, 100.0f), "Icons_1", "", 10, 10, 10, 10);
            Assert.AreEqual(new Point(0, 0), frame.TopLeft);
            Assert.AreEqual(new Point(100, 0), frame.TopRight);
            Assert.AreEqual(new Point(0, 100), frame.BottomLeft);
            Assert.AreEqual(new Point(100, 100), frame.BottomRight);
            Assert.AreEqual(new Point(50, 50), frame.Center);
            Assert.AreEqual(0, frame.Top);
            Assert.AreEqual(100, frame.Bottom);
            Assert.AreEqual(0, frame.Left);
            Assert.AreEqual(100, frame.Right);
            Assert.AreEqual(100, frame.Width);
            Assert.AreEqual(100, frame.Height);
            Assert.AreEqual(new Point(100, 100), frame.Size);
        }

        [TestMethod]
        public void Test_frame_creation_top_left_parent_top_left()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var parent = new Frame("ParentFrame", new Vector2(100.0f, 100.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", 10, 10, 10, 10);

            IControl frame = new Frame("FrameName", new Vector2(50.0f, 50.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", 10, 10, 10, 10, null, 0.0f, parent);
            Assert.AreEqual(new Point(150, 150), frame.TopLeft);
            Assert.AreEqual(new Point(250, 150), frame.TopRight);
            Assert.AreEqual(new Point(150, 250), frame.BottomLeft);
            Assert.AreEqual(new Point(250, 250), frame.BottomRight);
            Assert.AreEqual(new Point(200, 200), frame.Center);
            Assert.AreEqual(150, frame.Top);
            Assert.AreEqual(250, frame.Bottom);
            Assert.AreEqual(150, frame.Left);
            Assert.AreEqual(250, frame.Right);
            Assert.AreEqual(100, frame.Width);
            Assert.AreEqual(100, frame.Height);
            Assert.AreEqual(new Point(100, 100), frame.Size);

            Assert.AreEqual(new Point(50, 50), frame.RelativeTopLeft);
        }

        [TestMethod]
        public void Test_frame_creation_top_left_parent_bottom_right()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            var parent = new Frame("ParentFrame", new Vector2(100.0f, 100.0f), ContentAlignment.BottomRight, new Vector2(100.0f, 100.0f), "Icons_1", "", 10, 10, 10, 10);

            IControl frame = new Frame("FrameName", new Vector2(50.0f, 50.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", 10, 10, 10, 10, null, 0.0f, parent);
            Assert.AreEqual(new Point(50, 50), frame.TopLeft);
            Assert.AreEqual(new Point(150, 50), frame.TopRight);
            Assert.AreEqual(new Point(50, 150), frame.BottomLeft);
            Assert.AreEqual(new Point(150, 150), frame.BottomRight);
            Assert.AreEqual(new Point(100, 100), frame.Center);
            Assert.AreEqual(50, frame.Top);
            Assert.AreEqual(150, frame.Bottom);
            Assert.AreEqual(50, frame.Left);
            Assert.AreEqual(150, frame.Right);
            Assert.AreEqual(100, frame.Width);
            Assert.AreEqual(100, frame.Height);
            Assert.AreEqual(new Point(100, 100), frame.Size);
        }

        [TestMethod]
        public void Test_frame_creation_middle_center_no_parent_half_screen_resolution()
        {
            DeviceManager.Instance.SetScreenResolution(960, 540);

            IControl frame = new Frame("FrameName", new Vector2(960.0f, 540.0f), ContentAlignment.MiddleCenter, new Vector2(1920.0f, 1080.0f), "Icons_1", "", 10, 10, 10, 10);
            Assert.AreEqual(new Point(0, 0), frame.TopLeft);
            Assert.AreEqual(new Point(1920, 0), frame.TopRight);
            Assert.AreEqual(new Point(0, 1080), frame.BottomLeft);
            Assert.AreEqual(new Point(1920, 1080), frame.BottomRight);
            Assert.AreEqual(new Point(960, 540), frame.Center);
            Assert.AreEqual(0, frame.Top);
            Assert.AreEqual(1080, frame.Bottom);
            Assert.AreEqual(0, frame.Left);
            Assert.AreEqual(1920, frame.Right);
            Assert.AreEqual(1920, frame.Width);
            Assert.AreEqual(1080, frame.Height);
            Assert.AreEqual(new Point(1920, 1080), frame.Size);
        }

        [TestMethod]
        public void Test_changing_of_frame_position()
        {
            DeviceManager.Instance.SetScreenResolution(1920, 1080);

            IControl frame = new Frame("FrameName", new Vector2(100.0f, 100.0f), ContentAlignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", 10, 10, 10, 10);
            frame.SetTopLeftPosition(frame.TopLeft.X - 100, frame.TopLeft.Y - 100);
            Assert.AreEqual(new Point(0, 0), frame.TopLeft);
            Assert.AreEqual(new Point(100, 0), frame.TopRight);
            Assert.AreEqual(new Point(0, 100), frame.BottomLeft);
            Assert.AreEqual(new Point(100, 100), frame.BottomRight);
            Assert.AreEqual(new Point(50, 50), frame.Center);
            Assert.AreEqual(0, frame.Top);
            Assert.AreEqual(100, frame.Bottom);
            Assert.AreEqual(0, frame.Left);
            Assert.AreEqual(100, frame.Right);
            Assert.AreEqual(100, frame.Width);
            Assert.AreEqual(100, frame.Height);
            Assert.AreEqual(new Point(100, 100), frame.Size);
        }
    }
}