using NUnit.Framework;

namespace TurningModel.Tests
{
    [TestFixture]
    class GridTests
    {
        TurningCellGrid grid;

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
        public void PlaceTile_InIsolation()
        {
            grid.PlaceSpecificTile(2, 2, GameTileKind.Up);
            VerifyCellAndHitPointsAt(GameTileKind.Up, 4, 2, 2);
            VerifyScore(0);
        }

        [Test]
        public void PlaceTile_AffectingOneOtherTile()
        {
            //this tile lands as is
            grid.PlaceSpecificTile(2, 2, GameTileKind.Right);
            //this tile will make the next tile turn Down
            grid.PlaceSpecificTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 2, 2);
            VerifyScore(1);
        }

        [Test]
        public void PlaceTile_AffectingOneOtherHalfDone()
        {
            //this tile lands as is with 2 hit points left
            grid.PlaceSpecificTile(2, 2, GameTileKind.Right, 2);
            //this tile will make the next tile turn Down
            grid.PlaceSpecificTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Down, 1, 2, 2);
            VerifyScore(3);
        }

        [Test]
        public void PlaceTile_AffectingOneAlmostFinishedTile()
        {
            //this tile lands as is, with only 1 hit point left
            grid.PlaceSpecificTile(2, 2, GameTileKind.Right, 1);
            //this tile will make the next tile turn Down
            grid.PlaceSpecificTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAt(GameTileKind.None, 2, 2);

            VerifyScore(4); 
        }

        [Test]
        public void PlaceTile_FeedsBackToOriginalTile()
        {
            //this tile lands as is
            grid.PlaceSpecificTile(2, 2, GameTileKind.Down);
            //this tile will make the Down tile turn Left, which will in turn cause the first tile to turn Down
            grid.PlaceSpecificTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Left, 3, 2, 2);

            VerifyScore(2);
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

        [Test]
        public void TileDisappearsAfter4HitPoints()
        {
            SurroundCentralTileAndPointToIt();
            VerifyCellAt(GameTileKind.None, 2, 2);
        }

        [Test]
        public void HitPointsReach0After4Hits()
        {
            SurroundCentralTileAndPointToIt();
            Assert.AreEqual(0, grid.HitPointsAt(2, 2));
        }
 
        void SurroundCentralTileAndPointToIt()
        {
            grid.PlaceSpecificTile(2, 2, GameTileKind.Left);
            grid.PlaceSpecificTile(3, 2, GameTileKind.Left);
            grid.PlaceSpecificTile(1, 2, GameTileKind.Right);
            grid.PlaceSpecificTile(2, 1, GameTileKind.Down);
            grid.PlaceSpecificTile(2, 3, GameTileKind.Up);
        }

        private void VerifyCellAt(GameTileKind expectedTile, int x, int y)
        {
            Assert.AreEqual(expectedTile, grid.CellAt(x, y));
        }

        private void VerifyCellAndHitPointsAt(GameTileKind expectedTile, int expectedHitPoints, int x, int y)
        {
            Assert.AreEqual(expectedTile, grid.CellAt(x, y), "tile kind mismatch at ({0},{1})", x, y);
            Assert.AreEqual(expectedHitPoints, grid.HitPointsAt(x, y), string.Format("hit points mismatch at ({0},{1})", x, y));
        }

        private void VerifyScore(int expectedScore)
        {
            Assert.AreEqual(expectedScore, grid.Score, "score mismatch");
        }
    }
}
