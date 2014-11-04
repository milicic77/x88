using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RepresentLogic
{
    class RepresentSceneObject
    {
        // 场景对象
        private GameObject m_SceneObject;

        // 贴图
        Texture2D m_Texture;

        // 精灵
        Sprite m_Sprite;

        //// 逻辑X坐标
        //public int nLogicX;
        //// 逻辑Y坐标
        //public int nLogicY;

        public void Create(RepresentSceneLayer ParentSceneLayer, ref SceneObjectInfo sInfo)
        {
            float nWorldX = RepresentCommon.LogicX2WorldX(sInfo.nLogicX);
            float nWorldY = RepresentCommon.LogicY2WorldY(sInfo.nLogicY);

            m_SceneObject = new GameObject("test");

            // 贴图
            m_Texture = Resources.Load("walk_1") as Texture2D;

            // 精灵
            m_Sprite = Sprite.Create(m_Texture, new Rect(0, 0, RepresentDef.SCENE_OBJECT_PIXEL_X, RepresentDef.SCENE_OBJECT_PIXEL_Y), new Vector2(0.5f, 0.5f));


            m_SceneObject.AddComponent<SpriteRenderer>().sprite = m_Sprite;

            m_SceneObject.transform.parent = ParentSceneLayer.SceneObject.transform;
            m_SceneObject.transform.position = new Vector3(nWorldX, nWorldY, 0);
        }

    }
}
