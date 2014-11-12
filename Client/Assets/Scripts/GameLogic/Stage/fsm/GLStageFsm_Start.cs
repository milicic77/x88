using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameLogic
{
    public class FSMStateStart : KFsmCompositeState // 有子状态
    {
        // 关卡
        private GLStage m_Stage;

        public FSMStateStart(GLStage stage)
        {
            m_Stage = stage;
        }

        public override void InitFsm()
        {
            KFsmState[] states = new KFsmState[3];

            states[0] = new FSMStateStart_CreateGroupNpc(m_Stage);
            states[1] = new FSMStateStart_CreateGroupNpcInterval(m_Stage);
            states[2] = new FSMStateStart_CreateGroupNpcFinish(m_Stage);

            object[,] map = new object[,]
            {
                {states[0], (int)StageFsmLink_StartSub.STAGE_FSMLINK_START_SUB_CREATE_INTERVAL, states[1]},
                {states[1], (int)StageFsmLink_StartSub.STAGE_FSMLINK_START_SUB_INTERVAL_CREATE, states[0]},
                {states[0], (int)StageFsmLink_StartSub.STAGE_FSMLINK_START_SUB_CREATE_FIHISH, states[2]},
            };

            m_SubFsm = new KFsm();
            m_SubFsm.InitFsm(map, states, this, m_FsmRoot, 0);
        }

        public override void Enter(object[] args = null)
        {
            Common.Console.Write("Enter FSMStateStart");
            m_Stage.DoStart();

            base.Enter();
        }

        public override void OnEvent(KFsmEvent evt)
        {
            Common.Console.Write("FSMStateStart OnEvent");
            switch ((StageFsmEvent)evt.m_EventId)
            {
                case StageFsmEvent.STAGE_FSMLINK_GAME_END:
                    m_Fsm.DoTrans((int)StageFsmLink.STAGE_FSMLINK_START_END);
                    break;
                default:
                    m_SubFsm.m_CurState.OnEvent(evt);
                    break;
            }
        }

        public override void Update()
        {
            base.Update();
            m_Stage.ActivateNpc();
            m_Stage.ActivateTower();
            m_Stage.ActivateRadish();
            m_Stage.ActivateMissile();
        }
    }
    //////////////////////////////////////////////////////////////////////////
    public class FSMStateStart_CreateGroupNpc : KFsmState
    {
        // 关卡
        private GLStage m_Stage;

        public FSMStateStart_CreateGroupNpc(GLStage stage)
        {
            m_Stage = stage;
        }

        public override void Enter(object[] args = null)
        {
            Common.Console.Write("Enter FSMStateStart_CreateGroupNpc");
            Common.Console.Write("开始创建Npc");
        }

        public override void OnEvent(KFsmEvent evt)
        {
            Common.Console.Write("FSMStateStart_CreateGroupNpc OnEvent");

            switch ((StageFsmEvent)evt.m_EventId)
            {
                case StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_OK:
                    m_Fsm.DoTrans((int)StageFsmLink_StartSub.STAGE_FSMLINK_START_SUB_CREATE_INTERVAL);
                    break;
                case StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_FINISH:
                    m_Fsm.DoTrans((int)StageFsmLink_StartSub.STAGE_FSMLINK_START_SUB_CREATE_FIHISH);
                    break;
                default:
                    base.OnEvent(evt);
                    break;
            }
        }

        public override void Update()
        {
            int nRetCode = m_Stage.ActiveCreateGroupNpc();
            if (nRetCode == 1)
            {
                Common.Console.Write("所有组Npc创建完成");
                m_FsmRoot.PushEvent((int)StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_FINISH);
                return;
            }
            if (nRetCode == 2)
            {
                Common.Console.Write("本组Npc创建完成");
                m_FsmRoot.PushEvent((int)StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_OK);
                return;
            }
        }
    }
    //////////////////////////////////////////////////////////////////////////
    public class FSMStateStart_CreateGroupNpcInterval : KFsmState
    {
        // 关卡
        private GLStage m_Stage;

        public FSMStateStart_CreateGroupNpcInterval(GLStage stage)
        {
            m_Stage = stage;
        }

        public override void Enter(object[] args = null)
        {
            Common.Console.Write("Enter FSMStateStart_CreateGroupNpcInterval");
            Common.Console.Write("进入创建Npc组间隔时间");
            m_Stage.m_nBetweenTime = (uint)Environment.TickCount;
        }

        public override void OnEvent(KFsmEvent evt)
        {
            Common.Console.Write("FSMStateStart_CreateGroupNpcInterval OnEvent");

            switch ((StageFsmEvent)evt.m_EventId)
            {
                case StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_INTERVAL_OK:
                    m_Fsm.DoTrans((int)StageFsmLink_StartSub.STAGE_FSMLINK_START_SUB_INTERVAL_CREATE);
                    break;
                default:
                    base.OnEvent(evt);
                    break;
            }
        }
        public override void Update()
        {
            int nRetCode = m_Stage.ActiveCreateGroupNpcInterval();
            if (nRetCode == 1)
            {
                Common.Console.Write("离开创建Npc组间隔时间");
                m_FsmRoot.PushEvent((int)StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_INTERVAL_OK);
                return;
            }
        }
    }
    //////////////////////////////////////////////////////////////////////////
    public class FSMStateStart_CreateGroupNpcFinish : KFsmState
    {
        // 关卡
        private GLStage m_Stage;

        public FSMStateStart_CreateGroupNpcFinish(GLStage stage)
        {
            m_Stage = stage;
        }

        public override void Enter(object[] args = null)
        {
            Common.Console.Write("Enter FSMStateStart_CreateGroupNpcFihish");
            Common.Console.Write("进入所有组Npc创建完成状态");
        }

        public override void OnEvent(KFsmEvent evt)
        {
            Common.Console.Write("FSMStateStart_CreateGroupNpcFihish OnEvent");

            switch ((StageFsmEvent)evt.m_EventId)
            {
                case StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_FINISH:
                    m_FsmRoot.PushEvent((int)StageFsmEvent.STAGE_FSMLINK_GAME_END);
                    break;
                default:
                    base.OnEvent(evt);
                    break;
            }
        }
        public override void Update()
        {
            //Common.Console.Write("离开所有组Npc创建完成状态");

            // 外部发这个事件，游戏就结束了
            //m_FsmRoot.PushEvent((int)StageFsmEvent.STAGE_FSMEEVENT_START_GROUP_NPC_FINISH);
        }
    }
}
