using System;

namespace TurningModel
{
    public class TurningCellGrid
    {
        public readonly int height = 5;
        public readonly int width = 5;
        public int Score { get; private set; } 

        private GameTile[,] grid;

        GameTileKind currentTile, nextTile;

        public TurningCellGrid()
        {
            currentTile = GameTileUtils.GenerateRandomTileKind();
            nextTile = GameTileUtils.GenerateRandomTileKind();
            Init();
        }

        void Init()
        {
            grid = new GameTile[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    grid[x, y] = new GameTile();
                }
            }
            Console.WriteLine("current = " + currentTile + "; next = " + nextTile);
        }

        public bool IsInBounds(int cellX, int cellY)
        {
            return cellX >= 0 && cellX < width
                && cellY >= 0 && cellY < height;
        }

        public void MakeMove(int cellX, int cellY)
        {
            RotateCellAt(cellX, cellY);
            currentTile = nextTile;
            nextTile = GameTileUtils.GenerateRandomTileKind();
        }

        public GameTileKind CellAt(int x, int y)
        {
            return grid[x, y].Kind;
        }

        public int HitPointsAt(int x, int y)
        {
            return grid[x, y].HitPoints;
        }

        public void PlaceSpecificTile(int cellX, int cellY, GameTileKind tile)
        {
            grid[cellX, cellY] = new GameTile(tile);
            var originalHitPoints = GameTileUtils.OriginalHitPoints(tile);

            bool needsRotation = true;
            do
            {
                var dXdY = GameTileUtils.DirectionFromGameTile(grid[cellX, cellY].Kind);
                var dx = dXdY.Item1;
                var dy = dXdY.Item2;
                cellX += dx;
                cellY += dy;

                needsRotation = IsInBounds(cellX, cellY) && grid[cellX, cellY].Kind != GameTileKind.None;
                if (needsRotation)
                {
                    RotateCellAt(cellX, cellY);
                    Score++;
                }
            } while (needsRotation);
        }

        public void PlaceCurrentTile(int cellX, int cellY)
        {
            Console.WriteLine("current = " + currentTile + "; next = " + nextTile);

            PlaceSpecificTile(cellX, cellY, currentTile);
            currentTile = nextTile;
            nextTile = GameTileUtils.GenerateRandomTileKind();
        }

        public void RotateCellAt(int x, int y)
        {
            grid[x, y].RotateMe();
        }

    }
}
