using System;

namespace TurningModel
{
    public class TurningCellGrid
    {
        public int height = 5;
        public int width = 5;

        private GameTile[,] grid;

        GameTile currentPiece, upcomingPiece;

        bool IsInBounds(int cellX, int cellY)
        {
            return cellX >= 0 && cellX < width
                && cellY >= 0 && cellY < height;
        }

        public void MakeMove(int cellX, int cellY)
        {
            RotateCellAt(cellX, cellY);
            currentPiece = upcomingPiece;
            upcomingPiece = GameTileUtils.GenerateRandomPiece();
        }

        public TurningCellGrid()
        {
            currentPiece = GameTileUtils.GenerateRandomPiece();
            upcomingPiece = GameTileUtils.GenerateRandomPiece();
            Init();
        }

        public GameTile CellAt(int x, int y)
        {
            return grid[x, y];
        }

        public void PlaceCurrentTile(int cellX, int cellY)
        {
            Console.WriteLine("currentTile=" + currentPiece +
                " upcoming = " + upcomingPiece);
            grid[cellX, cellY] = currentPiece;
            currentPiece = upcomingPiece;
            upcomingPiece = GameTileUtils.GenerateRandomPiece();
            var dXdY = GameTileUtils.DirectionFromGameTile(grid[cellX, cellY]);
            var dx = dXdY.Item1;
            var dy = dXdY.Item2;
            var nextX = cellX + dx;
            var nextY = cellY + dy;
            if (IsInBounds(nextX, nextY) && grid[nextX, nextY] != GameTile.None)
                RotateCellAt(nextX, nextY);
        }

        public void RotateCellAt(int x, int y)
        {
            grid[x, y] = GameTileUtils.RotateCell(grid[x, y]);
        }

        void Init()
        {
            GameTileUtils.GenerateRandomPiece();
            grid = new GameTile[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (y == 0)
                    {
                        if (x == 0)
                            grid[x, y] = GameTile.None;
                        else if (x == 1)
                            grid[x, y] = GameTile.Left;
                        else if (x == 2)
                            grid[x, y] = GameTile.Up;
                        else if (x == 3)
                            grid[x, y] = GameTile.Right;
                        else if (x == 4)
                            grid[x, y] = GameTile.Down;
                    }
                    else if (y == 1)
                    {
                        if (x == 0)
                            grid[x, y] = GameTile.None;
                        else if (x == 1)
                            grid[x, y] = GameTile.LeftUp;
                        else if (x == 2)
                            grid[x, y] = GameTile.RightUp;
                        else if (x == 3)
                            grid[x, y] = GameTile.RightDown;
                        else if (x == 4)
                            grid[x, y] = GameTile.LeftDown;
                    }
                    else grid[x, y] = GameTile.None;
                }
            }
        }

    }
}
