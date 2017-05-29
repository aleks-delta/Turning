using System;
using System.Drawing;
using System.Windows.Forms;
using TurningModel;
using System.Threading;

namespace Turning
{
    public partial class GridForm : Form
    {
        private Pen gridPen, lightTilePen, darkTilePen, hotTilePen;
        private Brush cellBrush;
        private int cellSizeInPixels = 50;
        private int gridMarginInPixels = 10;
        private int arrowMarginInPixels = 5;
        private TurningCellGrid grid;
        SoundManager soundManager;       

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int cellX = (e.Location.X - gridMarginInPixels) / cellSizeInPixels;
            int cellY = (e.Location.Y - gridMarginInPixels) / cellSizeInPixels;

            var move = new TurningCellGrid.MoveSequence(grid);
            move.PlaceTileFirstStep(cellX, cellY, grid.currentTile);
            soundManager.Play(TurningSound.Place);
            Refresh();
            Thread.Sleep(500);
            while (!move.IsMoveFinished())
            {
                move.RotateAndShoot();
                Refresh();
                soundManager.Play(TurningSound.RotateAndShoot);
                Thread.Sleep(500);
            }
            grid.currentTile = grid.nextTile;
            grid.nextTile = GameTileUtils.GenerateRandomTileKind();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawCells(e.Graphics);
            DrawGridLines(e.Graphics);
            Console.WriteLine("current = " + grid.currentTile + "; next = " + grid.nextTile);

        }

        private void DrawOneCell(Graphics g, int x, int y)
        {
            var centerX = gridMarginInPixels + x * cellSizeInPixels + cellSizeInPixels / 2;
            var centerY = gridMarginInPixels + y * cellSizeInPixels + cellSizeInPixels / 2;
            if (grid.CellAt(x, y) == GameTileKind.None)
                g.FillRectangle(cellBrush, centerX, centerY, 2, 2);
            else
            {
                DrawArrow(g, x, y);
            }
        }

        private void DrawArrow(Graphics g, int x, int y)
        {
            var halfCell = cellSizeInPixels / 2;
            var center = new Point(gridMarginInPixels + x * cellSizeInPixels + halfCell,
                                   gridMarginInPixels + y * cellSizeInPixels + halfCell);

            Arrow arrow = new Arrow();
            GameTileKind cell = grid.CellAt(x, y);
            var hitPoints = grid.HitPointsAt(x, y);
            ArrowCoors coors = arrow.ArrowFromCellContent(cell);

            Point ptTip = new Point((int)(center.X + coors.dxTip * halfCell - coors.dxTip * arrowMarginInPixels), 
                                    (int)(center.Y + coors.dyTip * halfCell - coors.dyTip * arrowMarginInPixels));
            
            Point ptSide1 = new Point((int)(center.X + coors.dxSide1 * halfCell - coors.dxSide1 * arrowMarginInPixels),
                                      (int)(center.Y + coors.dySide1 * halfCell - coors.dySide1 * arrowMarginInPixels));
            Point ptSide2 = new Point((int)(center.X + coors.dxSide2 * halfCell - coors.dxSide2 * arrowMarginInPixels),
                                      (int)(center.Y + coors.dySide2 * halfCell - coors.dySide2 * arrowMarginInPixels));

            Point ptSide1Middle = new Point( (ptSide1.X + ptTip.X) / 2, (ptSide1.Y + ptTip.Y) / 2);
            Point ptSide2Middle = new Point((ptSide2.X + ptTip.X) / 2, (ptSide2.Y + ptTip.Y) / 2);

            g.DrawLine(lightTilePen, ptTip, ptSide1);
            g.DrawLine(lightTilePen, ptTip, ptSide2);

            switch (hitPoints)
            {
                case 4:
                    g.DrawLine(darkTilePen, ptTip, ptSide1);
                    g.DrawLine(darkTilePen, ptTip, ptSide2);
                    break;
                case 3:
                    //the left side and half of the right side will be dark
                    g.DrawLine(darkTilePen, ptTip, ptSide1);
                    g.DrawLine(darkTilePen, ptTip, ptSide2Middle);
                    break;
                case 2:
                    //only the "left" side will be dark
                    g.DrawLine(darkTilePen, ptTip, ptSide1); 
                    break;
                case 1:
                    //only half of the "left" side will be dark
                    g.DrawLine(darkTilePen, ptSide1, ptSide1Middle);
                    break;
            }
            if (hitPoints < 0)
            {
                g.DrawLine(hotTilePen, ptTip, ptSide1);
                g.DrawLine(hotTilePen, ptTip, ptSide2);
            }
        }

        private void DrawCells(Graphics g)
        {
            for (var y = 0; y < grid.height; y++)
                for (var x=0; x < grid.width; x++)
                {
                    DrawOneCell(g, x, y);
                }
        }

        private void DrawGridLines(Graphics g)
        {
            //draw horizontal lines
            for (var y = 0; y <= grid.height; y++)
                g.DrawLine(gridPen, gridMarginInPixels, gridMarginInPixels + y * cellSizeInPixels,
                    gridMarginInPixels + grid.width * cellSizeInPixels, gridMarginInPixels + y * cellSizeInPixels);
            for (var x = 0; x <= grid.width; x++)
                g.DrawLine(gridPen, gridMarginInPixels + x * cellSizeInPixels, gridMarginInPixels,
                    gridMarginInPixels + x * cellSizeInPixels, gridMarginInPixels + grid.height * cellSizeInPixels);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(300+
                3 * gridMarginInPixels + cellSizeInPixels * grid.width, 300+
                3 * gridMarginInPixels + cellSizeInPixels * grid.height);
        }

        public GridForm()
        {
            InitializeComponent();
            gridPen = new Pen(Color.Gray);
            lightTilePen = new Pen(Color.LightBlue, 3);
            darkTilePen = new Pen(Color.Blue, 3);
            hotTilePen = new Pen(Color.Red, 3);
            cellBrush = Brushes.Blue;

            grid = new TurningCellGrid();
            soundManager = new SoundManager();
           
        }
    }
}
