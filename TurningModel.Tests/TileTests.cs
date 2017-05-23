
using NUnit.Framework;

namespace TurningModel.Tests
{
    [TestFixture]
    public class TileTests
    {
        [Test]
        public void LeftRotatesToUp()
        {
            GameTile tile = GameTile.Left;
            var rotatedTile = GameTileUtils.RotateTile(tile);
            Assert.AreEqual(GameTile.Up, rotatedTile);
        }

        public void TopRightRotatesToBottomRight()
        {
            GameTile tile = GameTile.RightUp;
            var rotatedTile = GameTileUtils.RotateTile(tile);
            Assert.AreEqual(GameTile.RightDown, rotatedTile);
        }
    }
}
