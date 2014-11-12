using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.RepresentLogic
{
    class RepresentCommon
    {
        //////////////////////////////////////////////////////////////////////////
        // 格子坐标 <=> 逻辑坐标 （游戏逻辑中使用，表现逻辑中不使用）
        public static int CellX2LogicX(int nCellX)
        {
            if (nCellX < 0 || nCellX >= RepresentDef.SCENE_CELL_COUNT_X)
            {
                ExceptionTool.ThrowException("CellX2LogicX nCellX不合法！");
            }

            int nLogicX = nCellX * RepresentDef.SCENE_CELL_SIZE_PIXEL_X + RepresentDef.SCENE_CELL_SIZE_PIXEL_X / 2;

            return nLogicX;
        }
        public static int CellY2LogicY(int nCellY)
        {
            if (nCellY < 0 || nCellY >= RepresentDef.SCENE_CELL_COUNT_Y)
            {
                ExceptionTool.ThrowException("CellY2LogicY nCellY不合法！");
            }

            int nLogicY = nCellY * RepresentDef.SCENE_CELL_SIZE_PIXEL_Y + RepresentDef.SCENE_CELL_SIZE_PIXEL_Y / 2;

            return nLogicY;
        }
        public static int LogicX2CellX(int nLogicX)
        {
            if (nLogicX < 0 || nLogicX >= RepresentDef.SCENE_SIZE_PIXEL_X)
            {
                ExceptionTool.ThrowException("LogicX2CellX nLogicX不合法！");
            }

            int nCellX = nLogicX / RepresentDef.SCENE_CELL_SIZE_PIXEL_X;

            return nCellX;
        }
        public static int LogicY2CellY(int nLogicY)
        {
            if (nLogicY < 0 || nLogicY >= RepresentDef.SCENE_SIZE_PIXEL_Y)
            {
                ExceptionTool.ThrowException("LogicY2CellY nLogicY不合法！");
            }

            int nCellY = nLogicY / RepresentDef.SCENE_CELL_SIZE_PIXEL_Y;

            return nCellY;
        }
        //////////////////////////////////////////////////////////////////////////
        // 逻辑坐标 <=> 世界坐标
        public static float LogicX2WorldX(int nLogicX)
        {
            if (nLogicX < 0 || nLogicX >= RepresentDef.SCENE_SIZE_PIXEL_X)
            {
                ExceptionTool.ThrowException("LogicX2WorldX nLogicX不合法！");
            }

            float fWorldX = nLogicX - RepresentDef.SCENE_SIZE_PIXEL_X / 2;
            fWorldX = fWorldX / (float)RepresentDef.PIXEL_UNITY_SCALE;

            return fWorldX;
        }
        public static float LogicY2WorldY(int nLogicY)
        {
            if (nLogicY < 0 || nLogicY >= RepresentDef.SCENE_SIZE_PIXEL_X)
            {
                ExceptionTool.ThrowException("LogicY2WorldY nLogicY不合法！");
            }

            float fWorldY = RepresentDef.SCENE_SIZE_PIXEL_Y / 2 - nLogicY;
            fWorldY = fWorldY / (float)RepresentDef.PIXEL_UNITY_SCALE;

            return fWorldY;
        }
        public static int WorldX2LogicX(float fWorldX)
        {
            if (fWorldX > (float)RepresentDef.SCENE_SIZE_UNITY_X / 2 || fWorldX < -(RepresentDef.SCENE_SIZE_UNITY_X / 2))
            {
                ExceptionTool.ThrowException("WorldX2LogicX fWorldX不合法！");
            }

            int nWorldX = (int)(fWorldX * RepresentDef.PIXEL_UNITY_SCALE);
            nWorldX = nWorldX + (RepresentDef.SCENE_SIZE_PIXEL_X / 2);

            return nWorldX;
        }
        public static int WorldY2LogicY(float fWorldY)
        {
            if (fWorldY > RepresentDef.SCENE_SIZE_UNITY_Y / 2 || fWorldY < -(RepresentDef.SCENE_SIZE_UNITY_Y / 2))
            {
                ExceptionTool.ThrowException("WorldY2LogicY fWorldY不合法！");
            }

            int nWorldY = (int)(fWorldY * RepresentDef.PIXEL_UNITY_SCALE);
            nWorldY = RepresentDef.SCENE_SIZE_PIXEL_Y / 2 - nWorldY;

            return nWorldY;
        }
        //////////////////////////////////////////////////////////////////////////
        // 逻辑距离=》世界距离
        public static float LogicDis2WorldDis(int nLogicDis)
        {
            float fWorldDis = 0.0f;
            fWorldDis = (float)nLogicDis / (float)RepresentDef.PIXEL_UNITY_SCALE;

            return fWorldDis;
        }

    }
}
