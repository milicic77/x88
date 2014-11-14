using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.Common;
using UnityEngine;

namespace Game.GameLogic.Missile
{
    public class GLLifeControl
    {
        public float lifespan;
        public GLMissile itself;

        public void Activate()
        {
            if (0 == lifespan)
            {
                return;
            }

            lifespan -= GameDef.LOGIC_FRAME_INTERVEL;
            if (lifespan <= 0f)
            {
                OnTimeout();
            }
        }

        private void OnTimeout()
        {
            itself.Destroy();
        }
    }
}
