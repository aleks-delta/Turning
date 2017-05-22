

using System;

namespace TurningModel
{
    public enum CellContent
    {
        None, Left, Up, Right, Down,
        LeftUp, RightUp, RightDown, LeftDown
    };

    public class TurningCellGrid
    {
        public int height = 5;
        public int width = 5;

        private CellContent[,] grid;

        public static bool IsCellDiagonal(CellContent cell)
        {
            return cell == CellContent.LeftUp || cell == CellContent.RightUp
                || cell == CellContent.RightDown || cell == CellContent.LeftDown;
        }

        public static Tuple<int, int> DirectionFromCellContent(CellContent content)
        {
            int dx = 0;
            int dy = 0;
            switch (content)
            {
                case CellContent.Left:
                    dx = -1; dy = 0;
                    break;
                case CellContent.Up:
                    dx = 0; dy = -1;
                    break;
                case CellContent.Right:
                    dx = 1; dy = 0;
                    break;
                case CellContent.Down:
                    dx = 0; dy = 1;
                    break;
                case CellContent.LeftUp:
                    dx = -1; dy = -1;
                    break;
                case CellContent.RightUp:
                    dx = +1; dy = -1;
                    break;
                case CellContent.RightDown:
                    dx = +1; dy = +1;
                    break;
                case CellContent.LeftDown:
                    dx = -1; dy = +1;
                    break;
                default:
                    dx = dy = 0;
                    break;
            }
            return new Tuple<int, int>(dx, dy);
        }

        public TurningCellGrid()
        {
            Init();
        }

        public CellContent CellAt(int x, int y)
        {
            return grid[x, y];
        }

        void Init()
        {
            grid = new CellContent[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (y == 0)
                    {
                        if (x == 0)
                            grid[x, y] = CellContent.None;
                        else if (x == 1)
                            grid[x, y] = CellContent.Left;
                        else if (x == 2)
                            grid[x, y] = CellContent.Up;
                        else if (x == 3)
                            grid[x, y] = CellContent.Right;
                        else if (x == 4)
                            grid[x, y] = CellContent.Down;
                    }
                    else if (y == 1)
                    {
                        if (x == 0)
                            grid[x, y] = CellContent.None;
                        else if (x == 1)
                            grid[x, y] = CellContent.LeftUp;
                        else if (x == 2)
                            grid[x, y] = CellContent.RightUp;
                        else if (x == 3)
                            grid[x, y] = CellContent.RightDown;
                        else if (x == 4)
                            grid[x, y] = CellContent.LeftDown;
                    }
                    else grid[x, y] = CellContent.None;
                }
            }
        }

    }
}
