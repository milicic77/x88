using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.Common;
using UnityEngine;

namespace Game.GameLogic.Missile
{
    public class GLDamageControl
    {
        public GLMissile itself;
        public void Init(GLMissile itself)
        {
            this.itself = itself;
            RLTriggerHandler h = itself.rlMissile.gameObject.AddComponent<RLTriggerHandler>();
            h.itself = itself;
        }
    }
}
