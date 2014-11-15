using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;
using Game.RepresentLogic;
using Game.GameEvent;

namespace Game.GameLogic
{
    public class GameWorld : Common.Singleton<GameWorld>
    {
        public GLStage m_stage = null;
        public void Init(bool isOpenScreenUI)
        {
            GLSettingManager.Instance().Init();

            if (isOpenScreenUI)
                EventCenter.Event_LevelStart += OnLevelStart;
            else
                CreateStage(1);
        }

        public void UnInit()
        {
            EventCenter.Event_LevelStart -= OnLevelStart;
            GLSettingManager.Instance().Init();
        }

        public void Activate()
        {
            if (m_stage != null)
                m_stage.Activate();
        }

        public void Update()
        {

        }

        public void OnLevelStart(object sender, EventDef.BaseEventArgs args)
        {
            // 创建关卡
            CreateStage(1);
        }

        public GLStage Stage
        {
            get { return m_stage; }
        }

        public void CreateStage(int nStageId)
        {
            DestroyStage();

            m_stage = new GLStage();
            m_stage.Init(nStageId);
            m_stage.Start();
        }

        public void DestroyStage()
        {
            if (m_stage != null)
            {
                m_stage.UnInit();
                m_stage = null;
            }
        }
    }
}
