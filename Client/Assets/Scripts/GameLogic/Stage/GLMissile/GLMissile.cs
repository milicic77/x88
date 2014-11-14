using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.Common;
using Game.GameLogic.Missile;
using UnityEngine;

namespace Game.GameLogic
{
    public enum BallisticMode
    {
        LINEAR = 1,
        GUIDED,
        CURVE,
    }

    public enum WarheadType
    {
        BULLET = 1,
        BOMB,
        MULTI,
    }

    public class GLMissile
    {
        public RLMissile rlMissile;
        private GLBallisticBase ballisticSystem;
        public GLWarheadBase warhead;
        public GLLifeControl lifeControl;
        public bool bExplosed = false;

        public void Init(int templateId, Vector2 direction, Vector2 position, GLNpc targetNpc)
        {
            GLMissileTemplate template = GLSettingManager.Instance().GetGLMissileTemplate(templateId);
            rlMissile = Represent.Instance().CreateMissile(this, template.representId);

            switch (template.ballisticMode)
            {
                case BallisticMode.LINEAR:
                    GLGuidedBallistic linearBallistic = new GLGuidedBallistic();
                    linearBallistic.Init(
                        null, 
                        this, 
                        template.speed, 
                        direction,
                        position
                        );
                    ballisticSystem = linearBallistic;
                    break;
                case BallisticMode.GUIDED:
                    GLGuidedBallistic guidedBallistic = new GLGuidedBallistic();
                    guidedBallistic.Init(
                        targetNpc,
                        this,
                        template.speed,
                        direction,
                        position
                        );
                    ballisticSystem = guidedBallistic;
                    break;
                case BallisticMode.CURVE:
                    break;
                default:
                    break;
            }

            switch (template.warheadType)
            {
                case WarheadType.BULLET:
                    GLBulletWarhead bulletWarhead = new GLBulletWarhead(this);
                    bulletWarhead.impactDamage = template.impactDamage;
                    warhead = bulletWarhead;
                    break;
                case WarheadType.BOMB:
                    GLBombWarhead bombWarhead = new GLBombWarhead(this);
                    bombWarhead.exploseDamage = template.exploseDamage;
                    bombWarhead.exploseRadius = template.exploseRadius;
                    warhead = bombWarhead;
                    break;
                case WarheadType.MULTI:
                    GLMultiWarhead multiWarhead = new GLMultiWarhead(this);
                    multiWarhead.impactDamage = template.impactDamage;
                    multiWarhead.childTemplateId = template.childTemplateId;
                    warhead = multiWarhead;
                    break;
                default:
                    break;
            }

            lifeControl = new GLLifeControl();
            lifeControl.lifespan = template.lifespan;
            lifeControl.itself = this;
        }

        public void Activate()
        {
            ballisticSystem.Activate();
            lifeControl.Activate();
        }

        public void Destroy()
        {
            GameWorld.Instance().m_stage.m_GLMissileList.Remove(this);
            rlMissile.Destroy();
        }
    }
}
