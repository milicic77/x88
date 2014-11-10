using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameLogic
{
    // 关卡中的格子坐标
    public class GLPoint
    {
        public int nCellX = 0;
        public int nCellY = 0;

        public GLPoint(int nCellX, int nCellY)
        {
            this.nCellX = nCellX;
            this.nCellY = nCellY;
        }
    }
}
