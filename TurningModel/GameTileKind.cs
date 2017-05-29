using System;

namespace TurningModel
{
    public enum GameTileKind
    {
        None, Left, Up, Right, Down,
        LeftUp, RightUp, RightDown, LeftDown
    };

    public static class GameTileUtils
    {
        public static bool IsCellDiagonal(GameTileKind cell)
        {
            return cell == GameTileKind.LeftUp || cell == GameTileKind.RightUp
                || cell == GameTileKind.RightDown || cell == GameTileKind.LeftDown;
        }

        public static int OriginalHitPoints(GameTileKind cell)
        {
            if (cell == GameTileKind.None)
                return 0;
            else
                return 4;
        }

        static int length = Enum.GetNames(typeof(GameTileKind)).Length;

        public static GameTileKind RotateTile(GameTileKind cell)
        {
            switch (cell)
            {
                case GameTileKind.Left:
                    return GameTileKind.Up;
                case GameTileKind.Up:
                    return GameTileKind.Right;
                case GameTileKind.Right:
                    return GameTileKind.Down;
                case GameTileKind.Down:
                    return GameTileKind.Left;
                case GameTileKind.LeftUp:
                    return GameTileKind.RightUp;
                case GameTileKind.RightUp:
                    return GameTileKind.RightDown;
                case GameTileKind.RightDown:
                    return GameTileKind.LeftDown;
                case GameTileKind.LeftDown:
                    return GameTileKind.LeftUp;
                default:
                    return GameTileKind.None;
            }
        }

        public static Tuple<int, int> DirectionFromGameTile(GameTileKind content)
        {
            int dx = 0;
            int dy = 0;
            switch (content)
            {
                case GameTileKind.Left:
                    dx = -1; dy = 0;
                    break;
                case GameTileKind.Up:
                    dx = 0; dy = -1;
                    break;
                case GameTileKind.Right:
                    dx = 1; dy = 0;
                    break;
                case GameTileKind.Down:
                    dx = 0; dy = 1;
                    break;
                case GameTileKind.LeftUp:
                    dx = -1; dy = -1;
                    break;
                case GameTileKind.RightUp:
                    dx = +1; dy = -1;
                    break;
                case GameTileKind.RightDown:
                    dx = +1; dy = +1;
                    break;
                case GameTileKind.LeftDown:
                    dx = -1; dy = +1;
                    break;
                default:
                    dx = dy = 0;
                    break;
            }
            return new Tuple<int, int>(dx, dy);
        }

        public static GameTileKind GenerateRandomTileKind()
        {
            Random r = new Random();
            const double diagProb = 0.0;
            int diagFirstIndex = (int)GameTileKind.LeftUp;
            if (r.NextDouble() < diagProb)
                return (GameTileKind)r.Next(diagFirstIndex, length - 1);
            return (GameTileKind)r.Next(1, diagFirstIndex);
        }
    }
}
