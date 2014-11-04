using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RepresentLogic
{
    class RepresentScene
    {
        // 场景对象
        private GameObject m_SceneObject;

        // 场景对象-底层
        private RepresentSceneLayer m_BackGroundLayer;
        // 场景对象-中层
        private RepresentSceneLayer m_MiddleGroundLayer;
        // 场景对象-上层
        private RepresentSceneLayer m_ForceGroundLayer;

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

        public void Create()
        {
            // 场景对象
            m_SceneObject = new GameObject("Scene001");

            // 创建底层
            m_BackGroundLayer = new RepresentSceneLayer();
            SceneLayerInfo BackGroundLayerInfo = new SceneLayerInfo();
            BackGroundLayerInfo.szLayerName = "Scene001_BackGroundLayer";
            BackGroundLayerInfo.szTextureName = "scene_001";
            BackGroundLayerInfo.sTextureRect = new Rect(0, 0, 800, 600);
            BackGroundLayerInfo.nZ = SceneLayerZ.SceneGroundZ_Back;
            m_BackGroundLayer.Create(this, ref BackGroundLayerInfo);

            // 创建中层
            m_MiddleGroundLayer = new RepresentSceneLayer();
            SceneLayerInfo MiddleGroundLayerInfo = new SceneLayerInfo();
            MiddleGroundLayerInfo.szLayerName = "Scene001_MiddleGroundLayer";
            MiddleGroundLayerInfo.szTextureName = "";
            MiddleGroundLayerInfo.sTextureRect = new Rect(0, 0, 800, 600);
            MiddleGroundLayerInfo.nZ = SceneLayerZ.SceneGroundZ_Middle;
            m_MiddleGroundLayer.Create(this, ref MiddleGroundLayerInfo);

            SceneObjectInfo sSceneObjectInfo = new SceneObjectInfo();
            for (int i = 0; i <= 19; ++i )
            {
                for (int j = 0; j <= 14; ++j)
                {
                    sSceneObjectInfo.nLogicX = i;
                    sSceneObjectInfo.nLogicY = j;
                    m_MiddleGroundLayer.AddSceneObject(ref sSceneObjectInfo);
                }
            }

            m_SceneObject.transform.parent = RepresentEnv.SceneRoot.transform;
        }

        public void Destroy()
        {

        }

    }
}
