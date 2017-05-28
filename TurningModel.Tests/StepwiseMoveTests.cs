using NUnit.Framework;
using System.Drawing;

namespace TurningModel.Tests
{
    [TestFixture]
    partial class GridTests
    {
        [Test]
        public void PlaceTile_AffectingOneOtherTileStepwise()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            //this tile lands as is
            move.PlaceTile(2, 2, GameTileKind.Right);
            //this tile will make the next tile turn Down
            move.PlaceTileFirstStep(1, 2, GameTileKind.Right);
            VerifyPoint(2, 2, move.CurPoint);
            
            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 2, 2);
            VerifyScore(0);
            move.RotateAndShoot();
            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 2, 2);

            VerifyPoint(2, 3, move.CurPoint);
            VerifyCellAt(GameTileKind.None, 2, 3);
            VerifyScore(1);
        }

        [Test]
        public void PlaceTile_FeedsBackToOriginalTileStepwise()
        {
            var move = new TurningCellGrid.MoveSequence(grid);
            //this tile lands as is
            move.PlaceTile(2, 2, GameTileKind.Down);
            //this tile will make the Down tile turn Left, which will in turn cause the first tile to turn Down
            move.PlaceTileFirstStep(1, 2, GameTileKind.Right);

            VerifyPoint(2, 2, move.CurPoint);
            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Down, 4, 2, 2);

            move.RotateAndShoot();
            VerifyPoint(1, 2, move.CurPoint);
            VerifyScore(1);

            move.RotateAndShoot();
            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 1, 2);
            VerifyPoint(1, 3, move.CurPoint);
            VerifyCellAt(GameTileKind.None, 1, 3); 
        }

        private void VerifyPoint(int expectedX, int expectedY, Point pt)
        {
            Assert.AreEqual(expectedX, pt.X, "x mismatch");
            Assert.AreEqual(expectedY, pt.Y, "y mismatch");
        }
    }
}
