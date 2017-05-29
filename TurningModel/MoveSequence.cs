using System;
using System.Collections.Generic;
using System.Drawing;

namespace TurningModel
{
    public partial class TurningCellGrid {
        public class MoveSequence
        {
            public MoveSequence(TurningCellGrid g)
            {
                grid = g;
            }

            public void PlaceTile(int cellX, int cellY, GameTileKind tile, int externalHitPoints = -1)
            {
                PlaceTileFirstStep(cellX, cellY, tile, externalHitPoints);
                
                while (!IsMoveFinished())
                {
                    RotateAndShoot();
                }
                List<Point> finishedCells = GetFinishedCells();
                DestroyFinishedCells(finishedCells);
            }


            public List<Point> GetFinishedCells()
            {
                var finishedCells = new List<Point>();
                for (int y = 0; y < grid.height; y++)
                    for (int x = 0; x < grid.width; x++)
                        if (grid.UpForDestruction(x, y))
                            finishedCells.Add(new Point(x, y));
                return finishedCells;
            }

            private void DestroyFinishedCells(IEnumerable<Point> finishedCells)
            {
                foreach (var cell in finishedCells)
                    grid.DestroyCellAt(cell.X, cell.Y);
            }

            public void PlaceTileFirstStep(int cellX, int cellY, GameTileKind tile, int externalHitPoints = -1)
            {
                grid.grid[cellX, cellY] = (externalHitPoints < 0)
                    ? new GameTile(tile)
                    : new GameTile(tile, externalHitPoints);
                var dXdY = GameTileUtils.DirectionFromGameTile(grid.CellAt(cellX, cellY));
                var dx = dXdY.Item1;
                var dy = dXdY.Item2;
                CurPoint = new Point (cellX + dx, cellY + dy);
            }

            public void RotateAndShoot()
            {
                if (!IsMoveFinished())
                {
                    grid.RotateCellAt(CurPoint.X, CurPoint.Y);
                    int score = 4 - grid.HitPointsAt(CurPoint.X, CurPoint.Y);
                    grid.Score += score;
                    var dXdY = GameTileUtils.DirectionFromGameTile(grid.CellAt(CurPoint.X, CurPoint.Y));
                    var dx = dXdY.Item1;
                    var dy = dXdY.Item2;
                    CurPoint = new Point(CurPoint.X + dx, CurPoint.Y + dy);
                }
                else 
                    CurPoint = new Point(-1, -1);
            }

            public bool IsMoveFinished()
            {
                return !grid.IsInBounds(CurPoint.X, CurPoint.Y) || grid.CellAt(CurPoint.X, CurPoint.Y) == GameTileKind.None;
            }

            public Point CurPoint { get; private set; }
            private TurningCellGrid grid;
        }
    }
}
