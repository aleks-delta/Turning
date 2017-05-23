using System;

namespace TurningModel
{
    public class TurningCellGrid
    {
        public int height = 5;
        public int width = 5;

        private GameTile[,] grid;

        GameTile currentTile, nextTile;

        public bool IsInBounds(int cellX, int cellY)
        {
            return cellX >= 0 && cellX < width
                && cellY >= 0 && cellY < height;
        }

        public void MakeMove(int cellX, int cellY)
        {
            RotateCellAt(cellX, cellY);
            currentTile = nextTile;
            nextTile = GameTileUtils.GenerateRandomPiece();
        }

        public TurningCellGrid()
        {
            currentTile = GameTileUtils.GenerateRandomPiece();
            nextTile = GameTileUtils.GenerateRandomPiece();
            Init();
        }

        public GameTile CellAt(int x, int y)
        {
            return grid[x, y];
        }

        public void PlaceSpecificTile(int cellX, int cellY, GameTile tile)
        {
            grid[cellX, cellY] = tile;
            bool needsRotation = true;
            do
            {
                var dXdY = GameTileUtils.DirectionFromGameTile(grid[cellX, cellY]);
                var dx = dXdY.Item1;
                var dy = dXdY.Item2;
                cellX += dx;
                cellY += dy;

                needsRotation = IsInBounds(cellX, cellY) && grid[cellX, cellY] != GameTile.None;
                if (needsRotation)
                    RotateCellAt(cellX, cellY);
            } while (needsRotation);
        }

        public void PlaceCurrentTile(int cellX, int cellY)
        {
            Console.WriteLine("current = " + currentTile + "; next = " + nextTile);

            PlaceSpecificTile(cellX, cellY, currentTile);
            currentTile = nextTile;
            nextTile = GameTileUtils.GenerateRandomPiece();
        }

        public void RotateCellAt(int x, int y)
        {
            grid[x, y] = GameTileUtils.RotateTile(grid[x, y]);
        }

        void Init()
        {
            GameTileUtils.GenerateRandomPiece();
            grid = new GameTile[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    grid[x, y] = GameTile.None;
                }
            }
        }

    }
}
