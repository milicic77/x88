using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLEffect : MonoBehaviour
    {
        public Sprite[] m_Sprite = null; // 动画精灵

        private SpriteRenderer spriteRenderer; // 动画渲染

        private int m_nPlayCount = 0;
        private int m_nCurFrame = 0;
        private int m_nLastFrame = 0;

        // 表现模板
        public RLEffectTemplate m_Template;

        public float fFrameInterval = 100;
        public int nLastAniTime = 0;

        public void Init(int nRepresentId, float fWorldX, float fWorldY, int nOrder)
        {
            RLEffectTemplate template = RLResourceManager.Instance().GetRLEffectTemplate(nRepresentId);
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

            // 动画
            m_Sprite = new Sprite[template.TexAni.Count];
            for (int i = 0; i < template.TexAni.Count; ++i)
            {
                Rect rect = new Rect(0, 0,
                    template.TexAni[i].width, template.TexAni[i].height);

                m_Sprite[i] = Sprite.Create(
                    template.TexAni[i],
                    rect,
                    new Vector2(0.5f, 0.5f)
                );
            }


            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

            spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;

            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;

            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);

            spriteRenderer.enabled = false;
        }

        public void SetPosition(float fWorldX, float fWorldY)
        {
            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        public void Play(int nPlayerCount)
        {
            m_nPlayCount = nPlayerCount;
            m_nCurFrame = 0;

            if (nPlayerCount > 0)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }

        virtual public void Update()
        {
            if (m_nPlayCount > 0)
            {
                if (Environment.TickCount - m_nLastFrame > fFrameInterval)
                {
                    spriteRenderer.sprite = m_Sprite[m_nCurFrame];
                    m_nCurFrame++;

                    if (m_nCurFrame >= m_Template.TexAni.Count)
                    {
                        m_nPlayCount--;
                        m_nCurFrame = 0;

                        if (m_nPlayCount <= 0)
                        {
                            spriteRenderer.enabled = false;
                            //Represent.Instance().DestroyEffect(this);
                        }
                    }

                    m_nLastFrame = Environment.TickCount;
                }
            }
        }
    }
}
