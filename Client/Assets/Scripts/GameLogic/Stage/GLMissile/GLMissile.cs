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
        LINEAR,
        GUIDED,
        CURVE,
    }
    public class GLMissile
    {
        public RLMissile rlMissile;
        private GLBallisticBase ballisticSystem;
        private GLDamageControl damageControl;
        private GLLifeControl lifeControl;

        public void Init(int templateId, Vector2 direction, Vector2 position, GLNpc targetNpc)
        {
            GLMissileTemplate template = GLSettingManager.Instance().GetGLMissileTemplate(templateId);
            rlMissile = Represent.Instance().CreateMissile(targetNpc.m_RLNpc, template.nRepresentId);

            switch (template.eBallisticMode)
            {
                case BallisticMode.LINEAR:
                    GLGuidedBallistic linearBallistic = new GLGuidedBallistic();
                    linearBallistic.Init(
                        null, 
                        rlMissile.gameObject.GetComponent<Rigidbody2D>(), 
                        template.fSpeed, 
                        direction,
                        position
                        );
                    ballisticSystem = linearBallistic;
                    break;
                case BallisticMode.GUIDED:
                    GLGuidedBallistic guidedBallistic = new GLGuidedBallistic();
                    guidedBallistic.Init(
                        targetNpc,
                        rlMissile.gameObject.GetComponent<Rigidbody2D>(),
                        template.fSpeed,
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

            damageControl = new GLDamageControl();
            damageControl.Init(this);

            lifeControl = new GLLifeControl();
            lifeControl.lifespan = template.fLifespan;
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
            UnityEngine.Object.Destroy(rlMissile.gameObject);
        }
    }
}
