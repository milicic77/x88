using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;
using Game.GameLogic;

namespace Game.RepresentLogic
{
    public class RLMissile : MonoBehaviour
    {
        private GLMissile m_Itself;
        private const int FRAMES_PER_SECOND = 10;
        private RLAniController m_FlyAni;
        private RLAniController m_ExploseAni;
        private SpriteRenderer m_SpriteRenderer;

        public void Init(GLMissile itself, int nRepresentId, int nOrder)
        {
            this.m_Itself = itself;

            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sortingOrder = nOrder;

            RLMissileTemplate template = RLResourceManager.Instance().GetRLMissileTemplate(nRepresentId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nRepresentId不合法！");
            }

            Sprite[] flySpriteSet = new Sprite[template.TexFlyAni.Count];
            for (int i = 0; i < template.TexFlyAni.Count; ++i)
            {
                Rect rect = new Rect(0, 0,
                    template.TexFlyAni[i].width, template.TexFlyAni[i].height);

                flySpriteSet[i] = Sprite.Create(
                    template.TexFlyAni[i],
                    rect,
                    new Vector2(0.5f, 0.5f)
                );
            }

            m_FlyAni = new RLAniController();
            m_FlyAni.SpriteAnimation = flySpriteSet;
            m_FlyAni.SpriteRenderer = m_SpriteRenderer;
            m_FlyAni.FramesPerSecond = FRAMES_PER_SECOND;
        }

        public virtual void Update()
        {
            if (m_Itself.bExplosed)
            {
                m_SpriteRenderer.enabled = false;
                return;
            }

            transform.up = GetComponent<Rigidbody2D>().velocity;

            m_FlyAni.Update();
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
