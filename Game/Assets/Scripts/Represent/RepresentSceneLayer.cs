using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RepresentLogic
{
    // 场景层
    class RepresentSceneLayer
    {
        // 场景层对象
        private GameObject m_SceneLayer;

        // 场景层背景贴图
        Texture2D m_Texture;

        // 场景层精灵
        Sprite m_Sprite;

        // 场景层对象
        public List<RepresentSceneObject> asSceneObjectList;

        public GameObject SceneObject
        {
            set
            {
                m_SceneLayer = value;
            }
            get
            {
                return m_SceneLayer;
            }
        }

        public void Create(RepresentScene ParentScene, ref SceneLayerInfo sInfo)
        {
            // 对象
            m_SceneLayer = new GameObject(sInfo.szLayerName);

            // 贴图
            m_Texture = Resources.Load(sInfo.szTextureName) as Texture2D;

            // 精灵
            m_Sprite = Sprite.Create(m_Texture, new Rect(0, 0, RepresentDef.SCENE_PIXEL_X, RepresentDef.SCENE_PIXEL_Y), new Vector2(0.5f, 0.5f));


            m_SceneLayer.AddComponent<SpriteRenderer>().sprite = m_Sprite;
            //m_SceneLayer.GetComponent<SpriteRenderer>().

            m_SceneLayer.transform.parent = ParentScene.SceneObject.transform;
            m_SceneLayer.transform.position = new Vector3(0, 0, (float)sInfo.nZ);

        }

        public void AddSceneObject(ref SceneObjectInfo sInfo)
        {
            RepresentSceneObject sSceneObject = new RepresentSceneObject();
            sSceneObject.Create(this, ref sInfo);
        }

        public void Destroy()
        {

        }
    }
}
