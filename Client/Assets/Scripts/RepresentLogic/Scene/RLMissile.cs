using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLMissile : MonoBehaviour
    {
        private const int FRAMES_PER_SECOND = 10;
        private RLAniController m_FlyAni;
        private RLAniController m_ExploseAni;

        public void Init(RLNpc target, int nRepresentId, int nOrder)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sortingOrder = nOrder;

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
            m_FlyAni.SpriteRenderer = sr;
            m_FlyAni.FramesPerSecond = FRAMES_PER_SECOND;
//             template = RLResourceManager.Instance().GetRLMissileTemplate(nRepresentId);
//             if (template == null)
//             {
//                 ExceptionTool.ThrowException("nRepresentId不合法！");
//             }
// 
//             m_ExploseAni = new RLAniControl();
//             m_ExploseAni.Init(sr, )
        }

        public virtual void Update()
        {
            transform.up = GetComponent<Rigidbody2D>().velocity;

            m_FlyAni.Update();
        }
    }
}
