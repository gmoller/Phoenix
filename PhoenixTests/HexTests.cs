using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexLibrary;

namespace PhoenixTests
{
    [TestClass]
    public class HexTests
    {
        [TestMethod]
        public void TestOffsetCoordinatesFromPixel1()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(0, 0);
            Assert.AreEqual(0, worldHex.Col);
            Assert.AreEqual(0, worldHex.Row);
        }

        [TestMethod]
        public void TestOffsetCoordinatesFromPixel2()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(86, 286);
            Assert.AreEqual(0, worldHex.Col);
            Assert.AreEqual(3, worldHex.Row);
        }

        [TestMethod]
        public void TestOffsetCoordinatesFromPixel3()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(103, 286);
            Assert.AreEqual(0, worldHex.Col);
            Assert.AreEqual(3, worldHex.Row);
        }

        [TestMethod]
        public void TestOffsetCoordinatesFromPixel4()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(381, 575);
            Assert.AreEqual(3, worldHex.Col);
            Assert.AreEqual(6, worldHex.Row);
        }

        [TestMethod]
        public void TestOffsetCoordinatesFromPixel5()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(401, 606);
            Assert.AreEqual(3, worldHex.Col);
            Assert.AreEqual(6, worldHex.Row);
        }

        [TestMethod]
        public void TestOffsetCoordinatesFromPixel6()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(405, 617);
            Assert.AreEqual(3, worldHex.Col);
            Assert.AreEqual(6, worldHex.Row);
        }

        [TestMethod]
        public void TestOffsetCoordinatesFromPixel7()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(1, 385);
            Assert.AreEqual(0, worldHex.Col);
            Assert.AreEqual(4, worldHex.Row);
        }

        [TestMethod]
        public void TestOffsetCoordinatesFromPixel8()
        {
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel(31, 385);
            Assert.AreEqual(0, worldHex.Col);
            Assert.AreEqual(4, worldHex.Row);
        }
    }
}