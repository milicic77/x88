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
        protected float speed;
        protected GLMissile itself;
        protected Rigidbody2D rigidbody;

        public virtual void Activate()
        {

        }
    }

    public class GLGuidedBallistic : GLBallisticBase
    {
        protected GLNpc m_Target;

        public void Init(GLNpc target, GLMissile itself, float speed, Vector2 initDirection, Vector2 position)
        {
            this.itself = itself;
            m_Target = target;
            rigidbody = itself.rlMissile.GetComponent<Rigidbody2D>();
            this.speed = speed;

            rigidbody.velocity = initDirection * speed * GameDef.LOGIC_FRAME_INTERVEL;
            rigidbody.position = position;
        }

        public override void Activate()
        {
            if (itself == null || itself.bExplosed)
            {
                rigidbody.velocity = Vector2.zero;
            }

            if (m_Target == null || m_Target.m_nDelete == 1)
            {
                return;
            }

            float x = RepresentCommon.LogicX2WorldX(m_Target.GetLogicCenterX());
            float y = RepresentCommon.LogicY2WorldY(m_Target.GetLogicCenterY());

            Vector2 direction = (new Vector2(x, y) - rigidbody.position).normalized;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, direction * speed * GameDef.LOGIC_FRAME_INTERVEL, 0.5f);
        }
    }
}
