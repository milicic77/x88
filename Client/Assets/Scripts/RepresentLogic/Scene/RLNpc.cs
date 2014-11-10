using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLNpc : MonoBehaviour
    {
        public Sprite[] m_sprites = new Sprite[2]; // 动画贴图
        private SpriteRenderer spriteRenderer; // 动画渲染

        //private RLSceneObjectAnimationCtrl m_Animation = new RLSceneObjectAnimationCtrl();


        //public float moveSpeed;

        // 表现模板
        public RLNpcTemplate m_Template;

        public float framesPerSecond = 4;

        //// 逻辑X坐标
        //public int m_nLogicX;
        //// 逻辑Y坐标
        //public int m_nLogicY;
        //// 当前动作
        //public int m_nAni = (int)SceneObjectAni.SceneObjectAni_Stand;
        //// 当前方向
        //public int m_Direction = (int)SceneObjectDirection.SceneObjectDirection_Right;

        //public int DOING
        //{
        //    set
        //    {
        //        m_nAni = value;
        //    }
        //    get
        //    {
        //        return m_nAni;
        //    }
        //}

        //public int DIRECTION
        //{
        //    set
        //    {
        //        m_Direction = value;
        //    }
        //    get
        //    {
        //        return m_Direction;
        //    }
        //}

        //public SpriteRenderer SceneObjectSpriteRenderer
        //{
        //    set
        //    {
        //        spriteRenderer = value;
        //    }
        //    get
        //    {
        //        return spriteRenderer;
        //    }
        //}

        public void Init(int nRepresentId, float fWorldX, float fWorldY, int nOrder)
        {
            RLNpcTemplate template = RLResourceManager.Instance().GetRLNpcTemplate(nRepresentId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nRepresentId不合法！");
            }

            m_Template = template;

            Rect spriteRect = new Rect(0, 0, 
                template.TexAni[0].width, template.TexAni[0].height);

            // 精灵
            Sprite sprite = Sprite.Create(template.TexAni[0],
                spriteRect,
                new Vector2(0.5f, 0.5f)
            );

            for (int i = 0; i < 2; ++i )
            {
                Rect rect = new Rect(0, 0, 
                    template.TexAni[i].width, template.TexAni[i].height);

                m_sprites[i] = Sprite.Create(
                    template.TexAni[i],
                    rect,
                    new Vector2(0.5f, 0.5f)
                );
            }
                
            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

            spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;

            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;

            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        public void UnInit()
        {

        }

        public void SetPosition(float fWorldX, float fWorldY)
        {
            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        virtual public void Update()
        {
            //m_DeltaTime = Time.time - m_LastRepresentFrameTime;
            //m_LastRepresentFrameTime = Time.time / 1000.0f;
            // 

            // 播放动画
            //m_Animation.ShowAnimation(m_nAni, m_Direction);
            // 
            int nIndex = (int)(Time.timeSinceLevelLoad * framesPerSecond);

            nIndex = nIndex % 2;

            spriteRenderer.sprite = m_sprites[nIndex];
        }

    }
}
