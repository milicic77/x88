using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLScene : MonoBehaviour
    {
        // 场景对象-底层
        private RLLayer m_BackGroundLayer;
        // 场景对象-中层
        private RLLayer m_MiddleGroundLayer;
        // 场景对象-上层
        private RLLayer m_ForceGroundLayer;

        public void Init(int nTemplateId)
        {
            RLSceneTemplate template = RLResourceManager.Instance().GetRLSceneTemplate(nTemplateId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nTemplateId不合法！");
            }

            // 场景对象
            //m_SceneObject = new GameObject(cfg.szName);

            // 创建底层
            GameObject Object_BG = new GameObject();
            AddClild(Object_BG);
            m_BackGroundLayer = Object_BG.AddComponent<RLLayer>();
            m_BackGroundLayer.Init(
                template.szBackGroundImage, 
                (int)RLSceneObjectOrder.RLSceneOrder_BackLayer
            );

            // 创建底层
            GameObject Object_MG = new GameObject();
            AddClild(Object_MG);
            m_MiddleGroundLayer = Object_MG.AddComponent<RLLayer>();
            m_MiddleGroundLayer.Init(
                template.szMiddleGroundImage,
                (int)RLSceneObjectOrder.RLSceneOrder_MiddleLayer
            );

            // 创建上层
            GameObject Object_FG = new GameObject();
            AddClild(Object_FG);
            m_ForceGroundLayer = Object_FG.AddComponent<RLLayer>();
            m_ForceGroundLayer.Init(
                template.szForceGroundImage,
                (int)RLSceneObjectOrder.RLSceneOrder_ForceLayer
            );

            //for (int i = 0; i <= 19; ++i)
            //{
            //    for (int j = 0; j <= 14; ++j)
            //    {
            //        AddSceneObject(1, i, j);
            //    }
            //}
        }

        public void UnInit()
        {

        }

        public void AddClild(GameObject gameobject)
        {
            gameobject.transform.parent = gameObject.transform;
        }

        public RLDoodad AddSceneObject(int nRepresentId)
        {

            return null;
        }

        public void AddDoodad(RLDoodad doodad)
        {
            AddClild(doodad.gameObject);
        }

        public void AddNpc(RLNpc npc)
        {
            AddClild(npc.gameObject);
        }

        public void AddRadish(RLRadish radish)
        {
            AddClild(radish.gameObject);
        }

        public void AddEffect(RLEffect effect)
        {
            AddClild(effect.gameObject);
        }

        public void AddTower(RLTower tower)
        {
            AddClild(tower.gameObject);
        }

        //public RLCell GetRLCell(int nLogicX, int nLogicY)
        //{
        //    if (nLogicX >= RepresentDef.SCENE_CELL_COUNT_X || nLogicX < 0)
        //        return null;

        //    if (nLogicY >= RepresentDef.SCENE_CELL_COUNT_Y || nLogicX < 0)
        //        return null;

        //    return m_Cells[nLogicX, nLogicY];
        //}

        //public void SetRLCellState(int nLogicX, int nLogicY, int nState)
        //{
        //    RLCell cell = GetRLCell(nLogicX, nLogicY);
        //    if (null != cell)
        //    {
        //        cell.nState = nState;
        //    }
        //}

        //public void SetRLCellType(int nLogicX, int nLogicY, int nType)
        //{
        //    RLCell cell = GetRLCell(nLogicX, nLogicY);
        //    if (null != cell)
        //    {
        //        cell.nType = nType;
        //    }
        //}
    }
}
