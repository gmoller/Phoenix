﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using GuiControls;
using Microsoft.Xna.Framework;
using Point = Utilities.Point;

namespace PhoenixTests
{
    [TestClass]
    public class GuiControlTests_Image
    {
        [TestMethod]
        public void Test_image_creation_top_left_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(100, 100), image.TopLeft);
            Assert.AreEqual(new Point(200, 100), image.TopRight);
            Assert.AreEqual(new Point(100, 200), image.BottomLeft);
            Assert.AreEqual(new Point(200, 200), image.BottomRight);
            Assert.AreEqual(new Point(150, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_top_center_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.TopCenter, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(50, 100), image.TopLeft);
            Assert.AreEqual(new Point(150, 100), image.TopRight);
            Assert.AreEqual(new Point(50, 200), image.BottomLeft);
            Assert.AreEqual(new Point(150, 200), image.BottomRight);
            Assert.AreEqual(new Point(100, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(50, image.Left);
            Assert.AreEqual(150, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_top_right_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.TopRight, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(0, 100), image.TopLeft);
            Assert.AreEqual(new Point(100, 100), image.TopRight);
            Assert.AreEqual(new Point(0, 200), image.BottomLeft);
            Assert.AreEqual(new Point(100, 200), image.BottomRight);
            Assert.AreEqual(new Point(50, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_middle_left_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.MiddleLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(100, 50), image.TopLeft);
            Assert.AreEqual(new Point(200, 50), image.TopRight);
            Assert.AreEqual(new Point(100, 150), image.BottomLeft);
            Assert.AreEqual(new Point(200, 150), image.BottomRight);
            Assert.AreEqual(new Point(150, 100), image.Center);
            Assert.AreEqual(50, image.Top);
            Assert.AreEqual(150, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_middle_center_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.MiddleCenter, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(50, 50), image.TopLeft);
            Assert.AreEqual(new Point(150, 50), image.TopRight);
            Assert.AreEqual(new Point(50, 150), image.BottomLeft);
            Assert.AreEqual(new Point(150, 150), image.BottomRight);
            Assert.AreEqual(new Point(100, 100), image.Center);
            Assert.AreEqual(50, image.Top);
            Assert.AreEqual(150, image.Bottom);
            Assert.AreEqual(50, image.Left);
            Assert.AreEqual(150, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_middle_right_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.MiddleRight, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(0, 50), image.TopLeft);
            Assert.AreEqual(new Point(100, 50), image.TopRight);
            Assert.AreEqual(new Point(0, 150), image.BottomLeft);
            Assert.AreEqual(new Point(100, 150), image.BottomRight);
            Assert.AreEqual(new Point(50, 100), image.Center);
            Assert.AreEqual(50, image.Top);
            Assert.AreEqual(150, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_bottom_left_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.BottomLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(100, 0), image.TopLeft);
            Assert.AreEqual(new Point(200, 0), image.TopRight);
            Assert.AreEqual(new Point(100, 100), image.BottomLeft);
            Assert.AreEqual(new Point(200, 100), image.BottomRight);
            Assert.AreEqual(new Point(150, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_bottom_center_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.BottomCenter, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(50, 0), image.TopLeft);
            Assert.AreEqual(new Point(150, 0), image.TopRight);
            Assert.AreEqual(new Point(50, 100), image.BottomLeft);
            Assert.AreEqual(new Point(150, 100), image.BottomRight);
            Assert.AreEqual(new Point(100, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(50, image.Left);
            Assert.AreEqual(150, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_bottom_right_no_parent()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.BottomRight, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(0, 0), image.TopLeft);
            Assert.AreEqual(new Point(100, 0), image.TopRight);
            Assert.AreEqual(new Point(0, 100), image.BottomLeft);
            Assert.AreEqual(new Point(100, 100), image.BottomRight);
            Assert.AreEqual(new Point(50, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }

        [TestMethod]
        public void Test_image_creation_top_left_parent_top_left()
        {
            var parent = new Image(new Vector2(100.0f, 100.0f), Alignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", "parent");

            var image = new Image("image", new Vector2(100.0f, 100.0f), "Icons_1", "");
            parent.AddControl(image, Alignment.TopLeft, Alignment.TopLeft);
            Assert.AreEqual(new Point(100, 100), image.TopLeft);
            Assert.AreEqual(new Point(200, 100), image.TopRight);
            Assert.AreEqual(new Point(100, 200), image.BottomLeft);
            Assert.AreEqual(new Point(200, 200), image.BottomRight);
            Assert.AreEqual(new Point(150, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);

            Assert.AreEqual(new Point(0, 0), image.RelativeTopLeft);
        }

        [TestMethod]
        public void Test_image_creation_top_left_parent_bottom_right()
        {
            var parent = new Image(new Vector2(100.0f, 100.0f), Alignment.BottomRight, new Vector2(100.0f, 100.0f), "Icons_1", "", "parent");

            IControl image = new Image("image", new Vector2(100.0f, 100.0f), "Icons_1", "");
            parent.AddControl(image, Alignment.TopLeft, Alignment.TopLeft);
            Assert.AreEqual(new Point(0, 0), image.TopLeft);
            Assert.AreEqual(new Point(100, 0), image.TopRight);
            Assert.AreEqual(new Point(0, 100), image.BottomLeft);
            Assert.AreEqual(new Point(100, 100), image.BottomRight);
            Assert.AreEqual(new Point(50, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(00, image.Left);
            Assert.AreEqual(100, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);

            Assert.AreEqual(new Point(0, 0), image.RelativeTopLeft);
        }

        [TestMethod]
        public void Test_image_creation_middle_center_no_parent_half_screen_resolution()
        {
            IControl image = new Image(new Vector2(960.0f, 540.0f), Alignment.MiddleCenter, new Vector2(1920.0f, 1080.0f), "Icons_1", "", "image");
            Assert.AreEqual(new Point(0, 0), image.TopLeft);
            Assert.AreEqual(new Point(1920, 0), image.TopRight);
            Assert.AreEqual(new Point(0, 1080), image.BottomLeft);
            Assert.AreEqual(new Point(1920, 1080), image.BottomRight);
            Assert.AreEqual(new Point(960, 540), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(1080, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(1920, image.Right);
            Assert.AreEqual(1920, image.Width);
            Assert.AreEqual(1080, image.Height);
            Assert.AreEqual(new Point(1920, 1080), image.Size);
        }

        [TestMethod]
        public void Test_changing_of_image_position()
        {
            IControl image = new Image(new Vector2(100.0f, 100.0f), Alignment.TopLeft, new Vector2(100.0f, 100.0f), "Icons_1", "", "image");
            image.SetTopLeftPosition(image.TopLeft - new Point(100, 100));
            Assert.AreEqual(new Point(0, 0), image.TopLeft);
            Assert.AreEqual(new Point(100, 0), image.TopRight);
            Assert.AreEqual(new Point(0, 100), image.BottomLeft);
            Assert.AreEqual(new Point(100, 100), image.BottomRight);
            Assert.AreEqual(new Point(50, 50), image.Center);
            Assert.AreEqual(0, image.Top);
            Assert.AreEqual(100, image.Bottom);
            Assert.AreEqual(0, image.Left);
            Assert.AreEqual(100, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);

            image.MoveTopLeftPosition(new Point(100, 100));
            Assert.AreEqual(new Point(100, 100), image.TopLeft);
            Assert.AreEqual(new Point(200, 100), image.TopRight);
            Assert.AreEqual(new Point(100, 200), image.BottomLeft);
            Assert.AreEqual(new Point(200, 200), image.BottomRight);
            Assert.AreEqual(new Point(150, 150), image.Center);
            Assert.AreEqual(100, image.Top);
            Assert.AreEqual(200, image.Bottom);
            Assert.AreEqual(100, image.Left);
            Assert.AreEqual(200, image.Right);
            Assert.AreEqual(100, image.Width);
            Assert.AreEqual(100, image.Height);
            Assert.AreEqual(new Point(100, 100), image.Size);
        }
    }
}