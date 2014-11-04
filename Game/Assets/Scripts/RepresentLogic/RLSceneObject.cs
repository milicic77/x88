using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLSceneObject : MonoBehaviour
    {
        // 场景对象
        //private GameObject m_SceneObject;

        // 精灵
        Sprite m_Sprite;

        //public Sprite[] sprites; // 动画贴图
        private SpriteRenderer spriteRenderer; // 动画渲染

        private RLSceneObjectAnimationCtrl m_Animation = new RLSceneObjectAnimationCtrl();

        
        public float moveSpeed;
        
        // 展现ID
        public int m_nRepresentId;
        // 逻辑X坐标
        public int m_nLogicX;
        // 逻辑Y坐标
        public int m_nLogicY;
        // 当前动作
        public int m_nAni = (int)SceneObjectAni.SceneObjectAni_Stand;
        // 当前方向
        public int m_Direction = (int)SceneObjectDirection.SceneObjectDirection_Right;

        public SpriteRenderer SceneObjectSpriteRenderer
        {
            set
            {
                spriteRenderer = value;
            }
            get
            {
                return spriteRenderer;
            }
        }

        public void Init(int nRepresentId)
        {
            RepresentSceneObjectConfig cfg = RLSceneObjectTemplateManager.Instance().GetSceneObjectTemplateConfig(nRepresentId);
            if (cfg == null)
            {
                ExceptionTool.ThrowException("nTemplateId不合法！");
            }

            m_nRepresentId = nRepresentId;

            // 精灵
            m_Sprite = Sprite.Create(cfg.DefaultTexture, 
                new Rect(0, 0, RepresentDef.SCENE_OBJECT_PIXEL_X, RepresentDef.SCENE_OBJECT_PIXEL_Y), 
                new Vector2(0.5f, 0.5f)
            );

            //sprites = new Sprite[2];
            //sprites[0] = Sprite.Create(cfg.arrTexRunLeft[0], new Rect(0, 0, RepresentDef.SCENE_OBJECT_PIXEL_X, RepresentDef.SCENE_OBJECT_PIXEL_Y), new Vector2(0.5f, 0.5f));
            //sprites[1] = Sprite.Create(cfg.arrTexRunLeft[1], new Rect(0, 0, RepresentDef.SCENE_OBJECT_PIXEL_X, RepresentDef.SCENE_OBJECT_PIXEL_Y), new Vector2(0.5f, 0.5f));

            gameObject.AddComponent<SpriteRenderer>().sprite = m_Sprite;


            spriteRenderer = renderer as SpriteRenderer;

            m_Animation.Init(this, ref cfg);
        }

        public void SetPosition(int nLogicX, int nLogicY)
        {
            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;

            float fWorldX = RepresentCommon.LogicX2WorldX(m_nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(m_nLogicY);

            gameObject.transform.position = new Vector3(fWorldX, fWorldY, (float)SceneLayerZ.SceneGroundZ_Object);
        }

        virtual public void Update()
        {
            //m_DeltaTime = Time.time - m_LastRepresentFrameTime;
            //m_LastRepresentFrameTime = Time.time / 1000.0f;
            // 

            // 播放动画
            m_Animation.ShowAnimation(m_nAni, m_Direction);
        }

    }
}
