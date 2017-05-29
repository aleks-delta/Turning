namespace TurningModel
{
    public class GameTile
    {
        public GameTileKind Kind { get; private set; }
        public int HitPoints { get; private set; }

        public GameTile()
        {
            Kind = GameTileKind.None;
            HitPoints = 0;
        }

        public GameTile(GameTileKind tileKind)
        {
            Kind = tileKind;
            HitPoints = GameTileUtils.OriginalHitPoints(Kind);
        }

        public GameTile(GameTileKind tileKind, int hitPoints)
        {
            Kind = tileKind;
            HitPoints = hitPoints;
        }

        public void RotateMe()
        {
            HitPoints--;
            Kind = GameTileUtils.RotateTile(Kind);
        }
    }
}
