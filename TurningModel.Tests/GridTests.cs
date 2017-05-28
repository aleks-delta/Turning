using NUnit.Framework;
using System.Drawing;

namespace TurningModel.Tests
{
    [TestFixture]
    partial class GridTests
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
            var move = new TurningCellGrid.MoveSequence(grid);
            move.PlaceTile(2, 2, GameTileKind.Up);
            VerifyCellAndHitPointsAt(GameTileKind.Up, 4, 2, 2);
            VerifyScore(0);
        }

        [Test]
        public void PlaceTile_AffectingOneOtherTile()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            //this tile lands as is
            move.PlaceTile(2, 2, GameTileKind.Right);
            //this tile will make the next tile turn Down
            move.PlaceTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 2, 2);
            VerifyScore(1);
        }

        [Test]
        public void PlaceTile_AffectingOneOtherHalfDone()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            //this tile lands as is with 2 hit points left
            move.PlaceTile(2, 2, GameTileKind.Right, 2);
            //this tile will make the next tile turn Down
            move.PlaceTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Down, 1, 2, 2);
            VerifyScore(3);
        }

        [Test]
        public void PlaceTile_AffectingOneAlmostFinishedTile()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            //this tile lands as is, with only 1 hit point left
            move.PlaceTile(2, 2, GameTileKind.Right, 1);
            //this tile will make the next tile turn Down
            move.PlaceTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAt(GameTileKind.None, 2, 2);

            VerifyScore(4); 
        }

        [Test]
        public void PlaceTile_FeedsBackToOriginalTile()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            //this tile lands as is
            move.PlaceTile(2, 2, GameTileKind.Down);
            //this tile will make the Down tile turn Left, which will in turn cause the first tile to turn Down
            move.PlaceTile(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Left, 3, 2, 2);

            VerifyScore(2);
        }

        [Test][Ignore("need to handle keeping tiles alive until the end of the move")]
        public void PlaceTile_FeedsBackToOriginalTileAlmostDone()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            //this tile lands as is
            move.PlaceTile(2, 2, GameTileKind.Down, 1);
            //this tile will make the Down tile turn Left, which will in turn cause the first tile to turn Down
            move.PlaceTile(1, 2, GameTileKind.Right);

            //currently the original cell turns to "None" too early, 
            //we need to keep it alive and keep rotating until the whole move is over
            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Left, 3, 2, 2);

            VerifyScore(2);
        }

        [Test]
        public void ChainEndsOffGrid()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            move.PlaceTile(0, 0, GameTileKind.Left);
            //this tile will make the Left tile turn to Up
            move.PlaceTile(0, 1, GameTileKind.Up);
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
            var move = new TurningCellGrid.MoveSequence(grid);
            move.PlaceTile(2, 2, GameTileKind.Left);
            move.PlaceTile(3, 2, GameTileKind.Left);
            move.PlaceTile(1, 2, GameTileKind.Right);
            move.PlaceTile(2, 1, GameTileKind.Down);
            move.PlaceTile(2, 3, GameTileKind.Up);
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
