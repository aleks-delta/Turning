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
            //this tile lands as is
            grid.PlaceTile(2, 2, GameTileKind.Right);
            //this tile will make the next tile turn Down
            var nextPoint = grid.PlaceTileFirstStep(1, 2, GameTileKind.Right);
            VerifyPoint(2, 2, nextPoint);
            
            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 2, 2);
            VerifyScore(0);
            nextPoint = grid.RotateAndShoot(nextPoint.X, nextPoint.Y);
            VerifyCellAndHitPointsAt(GameTileKind.Down, 3, 2, 2);

            VerifyPoint(2, 3, nextPoint);
            VerifyCellAt(GameTileKind.None, nextPoint.X, nextPoint.Y);
            VerifyScore(1);
        }

        [Test]
        public void PlaceTile_FeedsBackToOriginalTileStepwise()
        {
            //this tile lands as is
            grid.PlaceTile(2, 2, GameTileKind.Down);
            //this tile will make the Down tile turn Left, which will in turn cause the first tile to turn Down
            grid.PlaceTileFirstStep(1, 2, GameTileKind.Right);

            VerifyCellAndHitPointsAt(GameTileKind.Right, 4, 1, 2);
            VerifyCellAndHitPointsAt(GameTileKind.Down, 4, 2, 2);

            VerifyScore(0);
        }

        private void VerifyPoint(int expectedX, int expectedY, Point pt)
        {
            Assert.AreEqual(expectedX, pt.X);
            Assert.AreEqual(expectedY, pt.Y);
        }
    }
}
