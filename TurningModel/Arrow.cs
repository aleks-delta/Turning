
using System.Collections.Generic;

namespace TurningModel
{
    public struct ArrowCoors
    {
        public readonly float dxTip;
        public readonly float dyTip;
        public readonly float dxSide1;
        public readonly float dySide1;
        public readonly float dxSide2;
        public readonly float dySide2;

        public ArrowCoors(float tipX, float tipY, float side1X, float side1Y, float side2X, float side2Y)
        {
            dxTip = tipX;
            dyTip = tipY;
            dxSide1 = side1X;
            dySide1 = side1Y;
            dxSide2 = side2X;
            dySide2 = side2Y;
        }

        public ArrowCoors RotateClockwise()
        {
            float tipX, tipY;
            float side1X, side1Y;
            float side2X, side2Y;
            RotateClockwise(dxTip, dyTip, out tipX, out tipY);
            RotateClockwise(dxSide1, dySide1, out side1X, out side1Y);
            RotateClockwise(dxSide2, dySide2, out side2X, out side2Y);

            ArrowCoors nextArrowCoor = new ArrowCoors(tipX, tipY, side1X, side1Y, side2X, side2Y);

            return nextArrowCoor;
        }

        private static void RotateClockwise(float dx, float dy, out float nextDx, out float nextDy)
        {
            nextDx = -dy;
            nextDy = dx;
        }
    }
    public class Arrow
    {

        //ArrowCoors Left, Up, Right, Down;
        Dictionary<GameTileKind, ArrowCoors> coors;
        bool IsInitialized;

        public Arrow()
        {
            IsInitialized = false;   
        }
       
        public ArrowCoors ArrowFromCellContent(GameTileKind cell)
        {
            if (!IsInitialized)
            {
                coors = new Dictionary<GameTileKind, ArrowCoors>();

                //init left arrow
                var LeftArrow = new ArrowCoors(-1, 0, +1, -1, +1, +1);
          
                coors[GameTileKind.Left] = LeftArrow;
                coors[GameTileKind.Up] = coors[GameTileKind.Left].RotateClockwise();
                coors[GameTileKind.Right] = coors[GameTileKind.Up].RotateClockwise();
                coors[GameTileKind.Down] = coors[GameTileKind.Right].RotateClockwise();

                //init left-up arrow
                float diagonalFactor = 0.7f;
                var LeftUpArrow = new ArrowCoors(-1, -1, +diagonalFactor, -diagonalFactor, -diagonalFactor, +diagonalFactor);
                coors[GameTileKind.LeftUp] = LeftUpArrow;
                coors[GameTileKind.RightUp] = coors[GameTileKind.LeftUp].RotateClockwise();
                coors[GameTileKind.RightDown] = coors[GameTileKind.RightUp].RotateClockwise();
                coors[GameTileKind.LeftDown] = coors[GameTileKind.RightDown].RotateClockwise();
                IsInitialized = true;               
            }
            Arrow arrow = new Arrow();

            return coors[cell];
        }
    }
}
