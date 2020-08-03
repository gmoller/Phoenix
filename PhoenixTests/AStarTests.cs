using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary;
using Utilities;

namespace PhoenixTests
{
    [TestClass]
    public class AStarTests
    {
        [TestMethod]
        public void TestOne()
        {
            var mapSolver = new MapSolver();
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var cellGrid = new CellGrid(60, 40);
            mapSolver.Graph(null, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), new Point(0, 0), new Point(1, 2), openList, closedList);

            if (mapSolver.Solution.HasValue)
            {
                var pos = mapSolver.Solution.Value.Position;
                var cost = mapSolver.Solution.Value.Cost;

                var result = new List<Point> { pos };
                do
                {
                    pos = mapSolver.ToPosition(cost.ParentIndex);
                    cost = closedList[pos];
                    result.Add(pos);
                } while (cost.ParentIndex >= 0);

                result.Reverse();

                Assert.AreEqual(3, result.Count);
                Assert.AreEqual(new Point(0, 0), result[0]);
                Assert.AreEqual(new Point(0, 1), result[1]);
                Assert.AreEqual(new Point(1, 2), result[2]);
            }
        }

        [TestMethod]
        public void TestTwo()
        {
            var mapSolver = new MapSolver();
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var cellGrid = new CellGrid(60, 40);
            mapSolver.Graph(null, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), new Point(0, 0), new Point(6, 0), openList, closedList);

            if (mapSolver.Solution.HasValue)
            {
                var pos = mapSolver.Solution.Value.Position;
                var cost = mapSolver.Solution.Value.Cost;

                var result = new List<Point> { pos };
                do
                {
                    pos = mapSolver.ToPosition(cost.ParentIndex);
                    cost = closedList[pos];
                    result.Add(pos);
                } while (cost.ParentIndex >= 0);

                result.Reverse();

                Assert.AreEqual(7, result.Count);
                Assert.AreEqual(new Point(0, 0), result[0]);
                Assert.AreEqual(new Point(1, 0), result[1]);
                Assert.AreEqual(new Point(2, 0), result[2]);
                Assert.AreEqual(new Point(3, 0), result[3]);
                Assert.AreEqual(new Point(4, 0), result[4]);
                Assert.AreEqual(new Point(5, 0), result[5]);
                Assert.AreEqual(new Point(6, 0), result[6]);
            }
        }

        [TestMethod]
        public void TestThree()
        {
            var mapSolver = new MapSolver();
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var cellGrid = new CellGrid(60, 40);
            mapSolver.Graph(null, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), new Point(12, 9), new Point(13, 7), openList, closedList);

            if (mapSolver.Solution.HasValue)
            {
                var pos = mapSolver.Solution.Value.Position;
                var cost = mapSolver.Solution.Value.Cost;

                var result = new List<Point> { pos };
                do
                {
                    pos = mapSolver.ToPosition(cost.ParentIndex);
                    cost = closedList[pos];
                    result.Add(pos);
                } while (cost.ParentIndex >= 0);

                result.Reverse();

                Assert.AreEqual(6, result.Count);
                Assert.AreEqual(new Point(12, 9), result[0]);
                Assert.AreEqual(new Point(13, 9), result[1]);
                Assert.AreEqual(new Point(14, 9), result[2]);
                Assert.AreEqual(new Point(15, 8), result[3]);
                Assert.AreEqual(new Point(14, 7), result[4]);
                Assert.AreEqual(new Point(13, 7), result[5]);
            }
        }
    }
}