using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GameWorld : Common.Singleton<GameWorld>
    {
        private GLStage m_stage = null;
        public void Init()
        {
            GLSettingManager.Instance().Init();

            // 创建关卡
            CreateStage(1);

        }

        public void UnInit()
        {
            GLSettingManager.Instance().Init();
        }

        public void Activate()
        {
            m_stage.Activate();
        }

        public void Update()
        {

        }

        public GLStage Stage
        {
            get { return m_stage; }
        }

        public void CreateStage(int nStageId)
        {
            m_stage = new GLStage();

            m_stage.Init(nStageId);
        }
    }
}
