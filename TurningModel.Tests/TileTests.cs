
using NUnit.Framework;

namespace TurningModel.Tests
{
    [TestFixture]
    public class TileTests
    {
        [Test]
        public void LeftRotatesToUp()
        {
            GameTileKind tile = GameTileKind.Left;
            var rotatedTile = GameTileUtils.RotateTile(tile);
            Assert.AreEqual(GameTileKind.Up, rotatedTile);
        }

        public void TopRightRotatesToBottomRight()
        {
            GameTileKind tile = GameTileKind.RightUp;
            var rotatedTile = GameTileUtils.RotateTile(tile);
            Assert.AreEqual(GameTileKind.RightDown, rotatedTile);
        }
    }
}
