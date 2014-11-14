using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.RepresentLogic;
using Game.Common;
using Game.GameEvent;

namespace Game.GameLogic.Missile
{
    public class GLWarheadBase
    {
        protected GLMissile itself;

        public GLWarheadBase(GLMissile itself)
        {
            this.itself = itself;
            RLTriggerHandler h = itself.rlMissile.gameObject.AddComponent<RLTriggerHandler>();
            h.itself = itself;
        }

        public virtual void OnTriggerEnter(GLNpc target)
        {
        
        }

        public virtual void OnExplose(GLNpc target = null)
        {

        }
    }

    public class GLBulletWarhead : GLWarheadBase
    {
        public int impactDamage;

        public GLBulletWarhead(GLMissile itself) : base(itself)
        {
        }

        public override void OnTriggerEnter(GLNpc target)
        {
            OnImpact(target);
        }

        public void OnImpact(GLNpc target)
        {
            EventDef.NpcHurtArgs args = new EventDef.NpcHurtArgs();
            args.npc = target;
            args.missile = itself;
            args.damage = impactDamage;
            EventCenter.Event_NpcHurt(null, args);
            itself.Destroy();
        }
    }

    public class GLBombWarhead : GLWarheadBase
    {
        public int exploseDamage;
        public int exploseRadius;
        private CircleCollider2D circleCollider;

        public GLBombWarhead(GLMissile itself) : base(itself)
        {
            circleCollider = itself.rlMissile.gameObject.GetComponent<CircleCollider2D>();
        }

        public override void OnTriggerEnter(GLNpc target)
        {
            if (!itself.bExplosed)
            {
                OnExplose(target);
            }
            else
            {
                OnExploseDamage(target);
            }
        }

        public override void OnExplose(GLNpc target)
        {
            OnExploseDamage(target);
            circleCollider.radius = RepresentCommon.LogicDis2WorldDis(exploseRadius);
            itself.bExplosed = true;
            itself.lifeControl.lifespan = 1;
        }

        public void OnExploseDamage(GLNpc target)
        {
            EventDef.NpcHurtArgs args = new EventDef.NpcHurtArgs();
            args.npc = target;
            args.missile = itself;
            args.damage = exploseDamage;
            EventCenter.Event_NpcHurt(null, args);
        }
    }

    public class GLMultiWarhead : GLWarheadBase
    {
        public int impactDamage;
        public int childTemplateId;

        public GLMultiWarhead(GLMissile itself)
            : base(itself)
        {
        }

        public override void OnTriggerEnter(GLNpc target)
        {
            OnExploseDamage(target);
            OnExplose(target);
        }

        public override void OnExplose(GLNpc target)
        {
            itself.bExplosed = true;
            itself.lifeControl.lifespan = GameDef.LOGIC_FRAME_INTERVEL * 3;

            Vector2[] directions = new Vector2[]{
                Vector2.right,
                Vector2.up,
                -Vector2.right,
                -Vector2.up
            };

            for (int i = 0; i < 2; ++i)
            {
                int directionIdx = UnityEngine.Random.Range(0, 4);
                GLMissile missile = new GLMissile();
                GameWorld.Instance().m_stage.m_GLMissileList.Add(missile);

                missile.Init(childTemplateId, directions[directionIdx], itself.rlMissile.gameObject.transform.position, null);
            }
        }

        public void OnExploseDamage(GLNpc target)
        {
            EventDef.NpcHurtArgs args = new EventDef.NpcHurtArgs();
            args.npc = target;
            args.missile = itself;
            args.damage = impactDamage;
            EventCenter.Event_NpcHurt(null, args);
        }
    }

    public class GLRocketWarhead : GLWarheadBase
    {
        public int impactDamage;

        public GLRocketWarhead(GLMissile itself)
            : base(itself)
        {
        }

        public override void OnTriggerEnter(GLNpc target)
        {
            OnImpact(target);
        }

        public void OnImpact(GLNpc target)
        {
            EventDef.NpcHurtArgs args = new EventDef.NpcHurtArgs();
            args.npc = target;
            args.missile = itself;
            args.damage = impactDamage;
            EventCenter.Event_NpcHurt(null, args);
        }
    }
}
