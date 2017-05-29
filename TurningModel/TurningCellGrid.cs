using System.Collections.Generic;
using System.Drawing;

namespace TurningModel
{
    public partial class TurningCellGrid
    {
        public readonly int height = 5;
        public readonly int width = 5;
        public int Score { get; private set; } 

        private GameTile[,] grid;

        public GameTileKind currentTile, nextTile;

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
        }

        public bool IsInBounds(int cellX, int cellY)
        {
            return cellX >= 0 && cellX < width
                && cellY >= 0 && cellY < height;
        }

        private bool UpForDestruction(int x, int y)
        {
            return (HitPointsAt(x, y) <= 0 && CellAt(x, y) != GameTileKind.None);
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

        public void RotateCellAt(int x, int y)
        {
            grid[x, y].RotateMe();
        }

        public void DestroyCellAt(int x, int y)
        {
            grid[x, y] = new GameTile();
        }
    }
}
