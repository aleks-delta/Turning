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
            VerifyCellAt(GameTileKind.None, 4, 4);
        }

        [Test]
        public void BoundsTest()
        {
            Assert.IsTrue(grid.IsInBounds(2, 2));
            Assert.IsFalse(grid.IsInBounds(5, 5));
            Assert.IsFalse(grid.IsInBounds(-1, 2));
        }

        [Test]
        public void PlaceTile_NoSideEffects_InIsolation()
        {
            grid.PlaceSpecificTile(2, 2, GameTileKind.Up);
            VerifyCellAt(GameTileKind.Up, 2, 2);
            Assert.AreEqual(4, grid.HitPointsAt(2,2));
            Assert.AreEqual(0, grid.Score);
        }

        [Test]
        public void PlaceTile_FeedsBackToOriginalTile()
        {
            //this tile lands as is
            grid.PlaceSpecificTile(2, 2, GameTileKind.Down);
            //this tile will make the Down tile turn Left, which will in turn cause the first tile to turn Down
            grid.PlaceSpecificTile(1, 2, GameTileKind.Right);

            VerifyCellAt(GameTileKind.Down, 1, 2);
            VerifyCellAt(GameTileKind.Left, 2, 2);
            Assert.AreEqual(2, grid.Score);
        }

        [Test]
        public void ChainEndsOffGrid()
        {
            grid.PlaceSpecificTile(0, 0, GameTileKind.Left);
            //this tile will make the Left tile turn to Up
            grid.PlaceSpecificTile(0, 1, GameTileKind.Up);
            VerifyCellAt(GameTileKind.Up, 0, 0);
            VerifyCellAt(GameTileKind.Up, 0, 1);
            Assert.AreEqual(1, grid.Score);
        }

        private void VerifyCellAt(GameTileKind expectedTile, int x, int y)
        {
            Assert.AreEqual(expectedTile, grid.CellAt(x, y));
        }
    }
}
