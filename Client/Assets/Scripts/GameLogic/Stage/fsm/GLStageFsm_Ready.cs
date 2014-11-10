using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameLogic
{
    public class FSMStateReady : KFsmState
    {
        // 关卡
        private GLStage m_Stage;

        public FSMStateReady(GLStage stage)
        {
            m_Stage = stage;
        }

        public override void Enter(object[] args = null)
        {
            Common.Console.Write("Enter FSMStateReady");
            m_Stage.DoReady();
        }

        public override void OnEvent(KFsmEvent evt)
        {
            Common.Console.Write("FSMStateReady OnEvent");

            switch ((StageFsmEvent)evt.m_EventId)
            {
                case StageFsmEvent.STAGE_FSMEEVENT_READY_OK:
                    m_FsmRoot.DoTrans((int)StageFsmLink.STAGE_FSMLINK_READY_START);
                    break;
                default:
                    base.OnEvent(evt);
                    break;
            }
        }

        public override void Update()
        {
            Common.Console.Write("游戏准备完成");
            m_FsmRoot.PushEvent((int)StageFsmEvent.STAGE_FSMEEVENT_READY_OK);
        }
    }
}
