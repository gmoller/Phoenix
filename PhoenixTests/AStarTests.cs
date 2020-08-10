using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
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

            var unit = new Unit(null, new UnitType(), new Point());
            GetCostToMoveIntoResult GetCostToMoveIntoFunc(Point point) => unit.CostToMoveInto(point);
            var cellGrid = new CellGrid(60, 40);
            mapSolver.Solve(GetCostToMoveIntoFunc, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), new Point(0, 0), new Point(1, 2), openList, closedList);

            var path = mapSolver.Solution;

            Assert.AreEqual(3, path.Count);
            Assert.AreEqual(new Point(0, 0), path[0]);
            Assert.AreEqual(new Point(0, 1), path[1]);
            Assert.AreEqual(new Point(1, 2), path[2]);
        }

        [TestMethod]
        public void TestTwo()
        {
            var mapSolver = new MapSolver();
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var unit = new Unit(null, new UnitType(), new Point());
            GetCostToMoveIntoResult GetCostToMoveIntoFunc(Point point) => unit.CostToMoveInto(point);
            var cellGrid = new CellGrid(60, 40);
            mapSolver.Solve(GetCostToMoveIntoFunc, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), new Point(0, 0), new Point(6, 0), openList, closedList);

            var path = mapSolver.Solution;

            Assert.AreEqual(7, path.Count);
            Assert.AreEqual(new Point(0, 0), path[0]);
            Assert.AreEqual(new Point(1, 0), path[1]);
            Assert.AreEqual(new Point(2, 0), path[2]);
            Assert.AreEqual(new Point(3, 0), path[3]);
            Assert.AreEqual(new Point(4, 0), path[4]);
            Assert.AreEqual(new Point(5, 0), path[5]);
            Assert.AreEqual(new Point(6, 0), path[6]);
        }

        [TestMethod]
        public void TestThree()
        {
            var mapSolver = new MapSolver();
            var openList = new PriorityQueue<AStarSearch<Point, Cost>.Node>();
            var closedList = new Dictionary<Point, Cost>();

            var unit = new Unit(null, new UnitType(), new Point());
            GetCostToMoveIntoResult GetCostToMoveIntoFunc(Point point) => unit.CostToMoveInto(point);
            var cellGrid = new CellGrid(60, 40);
            mapSolver.Solve(GetCostToMoveIntoFunc, new Point(cellGrid.NumberOfColumns, cellGrid.NumberOfRows), new Point(12, 9), new Point(13, 7), openList, closedList);

            var path = mapSolver.Solution;

            Assert.AreEqual(6, path.Count);
            Assert.AreEqual(new Point(12, 9), path[0]);
            Assert.AreEqual(new Point(13, 9), path[1]);
            Assert.AreEqual(new Point(14, 9), path[2]);
            Assert.AreEqual(new Point(15, 8), path[3]);
            Assert.AreEqual(new Point(14, 7), path[4]);
            Assert.AreEqual(new Point(13, 7), path[5]);
        }
    }
}