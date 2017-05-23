using NUnit.Framework;

namespace TurningModel.Tests
{
    [TestFixture]
    class GridTests
    {
        TurningCellGrid grid;

        
        public GridTests()
        {
            
        }

        [SetUp]
        public void Init()
        {
            grid = new TurningCellGrid();
        }

        [Test]
        public void GridCornerEmpty()
        {
            Assert.AreEqual(GameTile.None, grid.CellAt(4, 4));
        }

        [Test]
        public void BoundsTest()
        {
            Assert.IsTrue(grid.IsInBounds(2, 2));
            Assert.IsFalse(grid.IsInBounds(5, 5));
            Assert.IsFalse(grid.IsInBounds(-1, 2));
        }

        [Test]
        public void PlaceTileTest()
        {
            grid.PlaceSpecificTile(2, 2, GameTile.Up);
            Assert.AreEqual(GameTile.Up, grid.CellAt(2, 2));
            Assert.AreEqual(GameTile.None, grid.CellAt(2, 1));
        }

        [Test]
        public void PlaceTwoTilesNonInteracting()
        {
            grid.PlaceSpecificTile(1, 2, GameTile.Right);
            Assert.AreEqual(GameTile.Right, grid.CellAt(1, 2));
            Assert.AreEqual(GameTile.None, grid.CellAt(2, 2));
        }

        public void PlaceTwoTilesInteracting()
        {
            //this tile lands as is
            grid.PlaceSpecificTile(2, 2, GameTile.Down);
            //this tile will make the Down tile turn Left
            grid.PlaceSpecificTile(1, 2, GameTile.Right);
            Assert.AreEqual(GameTile.Right, grid.CellAt(1, 2));
            Assert.AreEqual(GameTile.Left, grid.CellAt(2, 2));
        }
    }
}
