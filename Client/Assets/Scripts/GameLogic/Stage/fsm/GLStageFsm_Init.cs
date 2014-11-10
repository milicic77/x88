using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameLogic
{
    public class FSMStateInit : KFsmState
    {
        // 关卡
        private GLStage m_Stage;

        public FSMStateInit(GLStage stage)
        {
            m_Stage = stage;
        }

        public override void Enter(object[] args = null)
        {
            Common.Console.Write("Enter FSMStateInit");

            int nStageId = (int)args[0];

            m_Stage.DoInit(nStageId);

        }

        public override void OnEvent(KFsmEvent evt)
        {
            Common.Console.Write("FSMStateInit OnEvent");

            switch ((StageFsmEvent)evt.m_EventId)
            {
                case StageFsmEvent.STAGE_FSMEEVENT_INIT_OK:
                    m_FsmRoot.DoTrans((int)StageFsmLink.STAGE_FSMLINK_INIT_READY);
                    break;
                default:
                    base.OnEvent(evt);
                    break;
            }
        }

        public override void Update()
        {
            Common.Console.Write("游戏初始化完成");
            m_FsmRoot.PushEvent((int)StageFsmEvent.STAGE_FSMEEVENT_INIT_OK);
        }
    }
}
