using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLRadish : MonoBehaviour
    {
        public enum RLRadishAni
        {
            RLRadishAni_None,
            RLRadishAni_Stand1,
            RLRadishAni_Stand2,
        }

        public RLRadishAni m_CurrentAni = RLRadishAni.RLRadishAni_None;

        public Sprite[] m_SpriteStandAni1 = null; // 站立动画1精灵
        public Sprite[] m_SpriteStandAni2 = null; // 站立动画2精灵

        private SpriteRenderer spriteRenderer; // 动画渲染

        Sprite m_sprite;

        int m_nCurFrame = 0;

        // 表现模板
        public RLRadishTemplate m_Template;

        public float framesPerSecond = 2;

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
            m_sprite = Sprite.Create(template.DefaultTexture,
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

            gameObject.AddComponent<SpriteRenderer>().sprite = m_sprite;

            spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;

            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;

            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        public void SetPosition(float fWorldX, float fWorldY)
        {
            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        public void DoStandAni()
        {
            m_CurrentAni = RLRadishAni.RLRadishAni_None;
        }

        public void DoStandAni_1()
        {
            m_CurrentAni = RLRadishAni.RLRadishAni_Stand1;
        }
        public void DoStandAni_2()
        {
            m_CurrentAni = RLRadishAni.RLRadishAni_Stand2;
        }

        virtual public void Update()
        {
            int nIndex = 0;

            switch (m_CurrentAni)
            {
                case RLRadishAni.RLRadishAni_None:
                    spriteRenderer.sprite = m_sprite;
                    m_nCurFrame = 0;
                    break;
                case RLRadishAni.RLRadishAni_Stand1:
                    nIndex = (int)(Time.timeSinceLevelLoad * framesPerSecond);
                    nIndex = nIndex % m_Template.TexStandAni1.Count;
                    spriteRenderer.sprite = m_SpriteStandAni1[nIndex];
                    m_nCurFrame++;
                    if (m_nCurFrame >= m_Template.TexStandAni1.Count)
                    {
                        m_nCurFrame = 0;
                        m_CurrentAni = RLRadishAni.RLRadishAni_None;
                    }

                    break;
                case RLRadishAni.RLRadishAni_Stand2:
                    nIndex = (int)(Time.timeSinceLevelLoad * framesPerSecond);
                    nIndex = nIndex % m_Template.TexStandAni2.Count;
                    spriteRenderer.sprite = m_SpriteStandAni2[nIndex];
                    m_nCurFrame++;
                    if (m_nCurFrame >= m_Template.TexStandAni2.Count)
                    {
                        m_nCurFrame = 0;
                        m_CurrentAni = RLRadishAni.RLRadishAni_None;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
