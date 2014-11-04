using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.RepresentLogic
{
    class RepresentCommon
    {
        // 逻辑坐标X转世界坐标X
        public static float LogicX2WorldX(int nLogicX)
        {
            if (nLogicX < 0 || nLogicX >= RepresentDef.SCENE_CELL_MAX_X)
            {
                ExceptionTool.ThrowException("nLogicX不合法！");
            }

            float nWorldX = nLogicX * RepresentDef.SCENE_OBJECT_PIXEL_X + RepresentDef.SCENE_OBJECT_PIXEL_X / 2 - RepresentDef.SCENE_PIXEL_X / 2;
            nWorldX = nWorldX / 100;

            return nWorldX;
        }

        // 逻辑坐标Y转世界坐标Y
        public static float LogicY2WorldY(int nLogicY)
        {
            if (nLogicY < 0 || nLogicY >= RepresentDef.SCENE_CELL_MAX_Y)
            {
                ExceptionTool.ThrowException("nLogicY不合法！");
            }

            float nWorldY = nLogicY * RepresentDef.SCENE_OBJECT_PIXEL_Y + RepresentDef.SCENE_OBJECT_PIXEL_Y / 2 - RepresentDef.SCENE_PIXEL_Y / 2;
            nWorldY = nWorldY / 100;

            return nWorldY;
        }
    }
}
