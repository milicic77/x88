using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    class RLScene
    {
        // 场景对象
        private GameObject m_SceneObject;

        // 场景对象-底层
        private RLSceneLayer m_BackGroundLayer;
        // 场景对象-中层
        private RLSceneLayer m_MiddleGroundLayer;
        // 场景对象-上层
        private RLSceneLayer m_ForceGroundLayer;

        // 格子信息
        private RLCell[,] m_Cells = new RLCell[RepresentDef.SCENE_CELL_MAX_X, RepresentDef.SCENE_CELL_MAX_Y];

        public GameObject SceneObject
        {
            set
            {
                m_SceneObject = value;
            }
            get
            {
                return m_SceneObject;
            }
        }

        public void Create(int nTemplateId)
        {
            RepresentSceneConfig cfg = RLSceneTemplateManager.Instance().GetSceneTemplateConfig(nTemplateId);
            if (cfg == null)
            {
                ExceptionTool.ThrowException("nTemplateId不合法！");
            }

            // 场景对象
            m_SceneObject = new GameObject(cfg.szName);

            // 创建底层
            m_BackGroundLayer = new RLSceneLayer();
            SceneLayerInfo BackGroundLayerInfo = new SceneLayerInfo();
            BackGroundLayerInfo.szLayerName = cfg.szName + "_BackGroundLayer";
            BackGroundLayerInfo.szTextureName = cfg.szBackGroundImage;
            BackGroundLayerInfo.sTextureRect = new Rect(0, 0, RepresentDef.SCENE_PIXEL_X, RepresentDef.SCENE_PIXEL_Y);
            BackGroundLayerInfo.nZ = SceneLayerZ.SceneGroundZ_Back;
            m_BackGroundLayer.Create(this, ref BackGroundLayerInfo);

            // 创建中层
            m_MiddleGroundLayer = new RLSceneLayer();
            SceneLayerInfo MiddleGroundLayerInfo = new SceneLayerInfo();
            MiddleGroundLayerInfo.szLayerName = cfg.szName + "_MiddleGroundLayer";
            MiddleGroundLayerInfo.szTextureName = cfg.szMiddleGroundImage;
            MiddleGroundLayerInfo.sTextureRect = new Rect(0, 0, RepresentDef.SCENE_PIXEL_X, RepresentDef.SCENE_PIXEL_Y);
            MiddleGroundLayerInfo.nZ = SceneLayerZ.SceneGroundZ_Middle;
            m_MiddleGroundLayer.Create(this, ref MiddleGroundLayerInfo);

            // 格子类型
            for (int i = 0; i < RepresentDef.SCENE_CELL_MAX_X; ++i)
            {
                for (int j = 0; j < RepresentDef.SCENE_CELL_MAX_Y; ++j)
                {
                    m_Cells[i, j] = new RLCell();
                    m_Cells[i, j].nType = cfg.nCellType[i, j];
                }
            }

            for (int i = 0; i <= 19; ++i)
            {
                for (int j = 0; j <= 14; ++j)
                {
                    AddSceneObject(1, i, j);
                }
            }

            m_SceneObject.transform.parent = RepresentEnv.SceneRoot.transform;
        }

        public void Destroy()
        {

        }

        public void AddSceneObject(int nRepresentId, int nLogicX, int nLogicY)
        {
            GameObject playerGo = new GameObject();
            RLSceneObject sSceneObject = playerGo.AddComponent<RLSceneObject>();		
            sSceneObject.Init(nRepresentId); // representid
            sSceneObject.SetPosition(nLogicX, nLogicY);

            sSceneObject.transform.parent = m_SceneObject.transform;
        }

    }
}
