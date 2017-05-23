
using System;

namespace TurningModel
{
    public enum GameTile
    {
        None, Left, Up, Right, Down,
        LeftUp, RightUp, RightDown, LeftDown
    };

    public static class GameTileUtils
    {
        public static bool IsCellDiagonal(GameTile cell)
        {
            return cell == GameTile.LeftUp || cell == GameTile.RightUp
                || cell == GameTile.RightDown || cell == GameTile.LeftDown;
        }

        static int length = Enum.GetNames(typeof(GameTile)).Length;

        public static GameTile RotateTile(GameTile cell)
        {
            switch (cell)
            {
                case GameTile.Left:
                    return GameTile.Up;
                case GameTile.Up:
                    return GameTile.Right;
                case GameTile.Right:
                    return GameTile.Down;
                case GameTile.Down:
                    return GameTile.Left;
                case GameTile.LeftUp:
                    return GameTile.RightUp;
                case GameTile.RightUp:
                    return GameTile.RightDown;
                case GameTile.RightDown:
                    return GameTile.LeftDown;
                case GameTile.LeftDown:
                    return GameTile.LeftUp;
                default:
                    return GameTile.None;
            }
        }

        public static Tuple<int, int> DirectionFromGameTile(GameTile content)
        {
            int dx = 0;
            int dy = 0;
            switch (content)
            {
                case GameTile.Left:
                    dx = -1; dy = 0;
                    break;
                case GameTile.Up:
                    dx = 0; dy = -1;
                    break;
                case GameTile.Right:
                    dx = 1; dy = 0;
                    break;
                case GameTile.Down:
                    dx = 0; dy = 1;
                    break;
                case GameTile.LeftUp:
                    dx = -1; dy = -1;
                    break;
                case GameTile.RightUp:
                    dx = +1; dy = -1;
                    break;
                case GameTile.RightDown:
                    dx = +1; dy = +1;
                    break;
                case GameTile.LeftDown:
                    dx = -1; dy = +1;
                    break;
                default:
                    dx = dy = 0;
                    break;
            }
            return new Tuple<int, int>(dx, dy);
        }

        internal static GameTile GenerateRandomPiece()
        {
            Random r = new Random();
            const double diagProb = 0.0;
            int diagFirstIndex = (int)GameTile.LeftUp;
            if (r.NextDouble() < diagProb)
                return (GameTile)r.Next(diagFirstIndex, length - 1);
            return (GameTile)r.Next(1, diagFirstIndex);
        }
    }
}
