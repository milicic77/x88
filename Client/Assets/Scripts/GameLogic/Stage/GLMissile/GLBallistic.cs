using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.Common;
using UnityEngine;

namespace Game.GameLogic.Missile
{
    public class GLBallisticBase
    {
        protected float m_Speed;
        protected Rigidbody2D m_Itself;

        public virtual void Activate()
        {

        }
    }

    public class GLGuidedBallistic : GLBallisticBase
    {
        protected GLNpc m_Target;

        public void Init(GLNpc target, Rigidbody2D itself, float speed, Vector2 initDirection, Vector2 position)
        {
            m_Target = target;
            m_Itself = itself;
            m_Speed = speed;

            m_Itself.velocity = initDirection * m_Speed * GameDef.LOGIC_FRAME_INTERVEL;
            m_Itself.position = position;
        }

        public override void Activate()
        {
            if (m_Itself == null || m_Target == null || m_Target.m_nDelete == 1)
            {
                return;
            }

            float x = RepresentCommon.LogicX2WorldX(m_Target.GetLogicCenterX());
            float y = RepresentCommon.LogicY2WorldY(m_Target.GetLogicCenterY());

            Vector2 direction = (new Vector2(x, y) - m_Itself.position).normalized;
            m_Itself.velocity = Vector2.Lerp(m_Itself.velocity, direction * m_Speed * GameDef.LOGIC_FRAME_INTERVEL, 0.5f);
        }
    }
}
