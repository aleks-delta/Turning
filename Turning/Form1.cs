using System;
using System.Drawing;
using System.Windows.Forms;
using TurningModel;

namespace Turning
{
    
    public partial class Form1 : Form
    {
        private Pen gridPen, cellPen;
        private Brush cellBrush;
        private int cellSizeInPixels = 50;
        private int gridMarginInPixels = 10;
        private int arrowMarginInPixels = 5;
        private TurningCellGrid grid;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawCells(e.Graphics);
            DrawGridLines(e.Graphics);
        }

        private void DrawOneCell(Graphics g, int x, int y)
        {
            var centerX = gridMarginInPixels + x * cellSizeInPixels + cellSizeInPixels / 2;
            var centerY = gridMarginInPixels + y * cellSizeInPixels + cellSizeInPixels / 2;
            if (grid.CellAt(x, y) == CellContent.None)
                g.FillRectangle(cellBrush, centerX, centerY, 2, 2);
            else //if (grid.CellAt(x, y) == CellContent.Left)
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
            CellContent cell = grid.CellAt(x, y);
            ArrowCoors coors = arrow.ArrowFromCellContent(cell);

            Point ptTip = new Point((int)(center.X + coors.dxTip * halfCell - coors.dxTip * arrowMarginInPixels), 
                                    (int)(center.Y + coors.dyTip * halfCell - coors.dyTip * arrowMarginInPixels));
            
            Point ptSide1 = new Point((int)(center.X + coors.dxSide1 * halfCell - coors.dxSide1 * arrowMarginInPixels),
                                      (int)(center.Y + coors.dySide1 * halfCell - coors.dySide1 * arrowMarginInPixels));
            Point ptSide2 = new Point((int)(center.X + coors.dxSide2 * halfCell - coors.dxSide2 * arrowMarginInPixels),
                                      (int)(center.Y + coors.dySide2 * halfCell - coors.dySide2 * arrowMarginInPixels));
            g.DrawLine(gridPen, ptTip, ptSide1);
            g.DrawLine(cellPen, ptTip, ptSide2);
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
            this.Size = new Size(3 * gridMarginInPixels + cellSizeInPixels * grid.width, 
                3 * gridMarginInPixels + cellSizeInPixels * grid.height);
        }

        public Form1()
        {
            InitializeComponent();
            gridPen = new Pen(Color.Gray);
            cellPen = new Pen(Color.Blue);
            cellBrush = Brushes.Blue;


            grid = new TurningCellGrid();
           
        }
    }
}
