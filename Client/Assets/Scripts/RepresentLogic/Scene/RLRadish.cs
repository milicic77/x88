using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLRadish : MonoBehaviour
    {
        public Sprite[] m_SpriteStandAni1 = null; // 站立动画1精灵
        public Sprite[] m_SpriteStandAni2 = null; // 站立动画2精灵

        private SpriteRenderer spriteRenderer; // 动画渲染

        //private RLSceneObjectAnimationCtrl m_Animation = new RLSceneObjectAnimationCtrl();


        //public float moveSpeed;

        // 表现模板
        public RLRadishTemplate m_Template;

        public float framesPerSecond = 10;

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
            RLRadishTemplate template = RLResourceManager.Instance().GetRLRadishTemplate(nRepresentId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nRepresentId不合法！");
            }

            m_Template = template;

            Rect spriteRect = new Rect(0, 0,
                template.DefaultTexture.width, template.DefaultTexture.height);

            // 精灵
            Sprite sprite = Sprite.Create(template.DefaultTexture,
                spriteRect,
                new Vector2(0.4f, 0.3f)
            );

            // 站立动画1
            m_SpriteStandAni1 = new Sprite[template.TexStandAni1.Count];
            for (int i = 0; i < template.TexStandAni1.Count; ++i)
            {
                Rect rect = new Rect(0, 0,
                    template.TexStandAni1[i].width, template.TexStandAni1[i].height);

                m_SpriteStandAni1[i] = Sprite.Create(
                    template.TexStandAni1[i],
                    rect,
                    new Vector2(0.4f, 0.3f)
                );
            }

            // 站立动画2
            m_SpriteStandAni2 = new Sprite[template.TexStandAni2.Count];
            for (int i = 0; i < template.TexStandAni2.Count; ++i)
            {
                Rect rect = new Rect(0, 0,
                    template.TexStandAni2[i].width, template.TexStandAni2[i].height);

                m_SpriteStandAni2[i] = Sprite.Create(
                    template.TexStandAni2[i],
                    rect,
                    new Vector2(0.4f, 0.3f)
                );
            }

            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

            spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;

            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;

            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
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

            nIndex = nIndex % m_Template.TexStandAni2.Count;

            spriteRenderer.sprite = m_SpriteStandAni2[nIndex];
        }
    }
}
