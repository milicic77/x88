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

            float nWorldY = RepresentDef.SCENE_PIXEL_Y / 2 - nLogicY * RepresentDef.SCENE_OBJECT_PIXEL_Y - RepresentDef.SCENE_OBJECT_PIXEL_Y / 2;
            nWorldY = nWorldY / 100;

            return nWorldY;
        }

        // 世界坐标X转逻辑坐标X
        public static int WorldX2LogicX(float fWorldX)
        {
            if (fWorldX > RepresentDef.SCENE_PIXEL_X / 2 || fWorldX < -(RepresentDef.SCENE_PIXEL_X / 2))
            {
                ExceptionTool.ThrowException("fWorldX不合法！");
            }

            int nWorldX = (int)fWorldX;
            nWorldX = nWorldX + (RepresentDef.SCENE_PIXEL_X / 2);

            if (nWorldX == 0)
                return 0;

            if (nWorldX == RepresentDef.SCENE_PIXEL_X)
                return RepresentDef.SCENE_CELL_MAX_X - 1;

            int nIndex = nWorldX / RepresentDef.SCENE_OBJECT_PIXEL_X - 1;
            int nMod = nWorldX % RepresentDef.SCENE_OBJECT_PIXEL_X;

            if (nMod > 0)
            {
                nIndex++;
            }

            return nIndex;
        }

        // 世界坐标Y转逻辑坐标Y
        public static int WorldY2LogicY(float fWorldY)
        {
            if (fWorldY > RepresentDef.SCENE_PIXEL_Y / 2 || fWorldY < -(RepresentDef.SCENE_PIXEL_Y / 2))
            {
                ExceptionTool.ThrowException("fWorldY不合法！");
            }

            int nWorldY = (int)fWorldY;
            nWorldY = nWorldY + (RepresentDef.SCENE_PIXEL_Y / 2);

            if (nWorldY == 0)
                return RepresentDef.SCENE_CELL_MAX_Y - 1;

            if (nWorldY == RepresentDef.SCENE_PIXEL_Y)
                return 0;

            int nIndex = nWorldY / RepresentDef.SCENE_OBJECT_PIXEL_Y - 1;
            int nMod = nWorldY % RepresentDef.SCENE_OBJECT_PIXEL_Y;

            if (nMod > 0)
            {
                nIndex++;
            }

            return RepresentDef.SCENE_CELL_MAX_Y - 1 - nIndex;
        }
    }
}
