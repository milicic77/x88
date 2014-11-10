using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameLogic
{
    // 开始状态的子状态机连接
    public enum StageFsmLink_StartSub
    {
        STAGE_FSMLINK_START_SUB_CREATE_INTERVAL, // 创建-》间隔
        STAGE_FSMLINK_START_SUB_INTERVAL_CREATE, // 间隔-》创建
        STAGE_FSMLINK_START_SUB_CREATE_FIHISH, // 创建-》结束
    }

    // 状态机连接
    public enum StageFsmLink
    {
        STAGE_FSMLINK_INIT_READY,
        STAGE_FSMLINK_READY_START,
        STAGE_FSMLINK_START_END,
    }

    // 状态机事件
    public enum StageFsmEvent
    {
        STAGE_FSMEEVENT_INIT_OK,
        STAGE_FSMEEVENT_READY_OK,
        STAGE_FSMEEVENT_START_GROUP_NPC_OK, // 创建一组Npc完成
        STAGE_FSMEEVENT_START_GROUP_NPC_INTERVAL_OK,// 产生两组Npc的中间间隔时间完成
        STAGE_FSMEEVENT_START_GROUP_NPC_FINISH,// 产生所有组Npc结束
        STAGE_FSMLINK_GAME_END,
    }

    public class GLStageFsm
    {
        //////////////////////////////////////////////////////////////////////////
        // 关卡
        private GLStage m_Stage;
        private int m_nStageId;
        // 状态机
        private KFsm m_StageFsm;
        //////////////////////////////////////////////////////////////////////////

        public void Init(GLStage stage, int nStageId)
        {
            m_Stage = stage;
            m_nStageId = nStageId;

            KFsmState[] states = new KFsmState[4];

            states[0] = new FSMStateInit(stage);
            states[1] = new FSMStateReady(stage);
            states[2] = new FSMStateStart(stage);
            states[3] = new FSMStateEnd(stage);

            object[,] map = new object[,]
            {
                {states[0], (int)StageFsmLink.STAGE_FSMLINK_INIT_READY,   states[1]},
                {states[1], (int)StageFsmLink.STAGE_FSMLINK_READY_START,  states[2]},
                {states[2], (int)StageFsmLink.STAGE_FSMLINK_START_END,    states[3]},
            };

            m_StageFsm = new KFsm();
            m_StageFsm.InitFsm(map, states, this, m_StageFsm, 0);
        }

        public void Start()
        {
            object[] fsmArgs = new object[1];
            fsmArgs[0] = m_nStageId;
            m_StageFsm.Startup(fsmArgs);
        }

        public void UnInit()
        {

        }

        public void Activate()
        {
            m_StageFsm.Update();
        }

        public void PushEvent(int eventId, object[] args = null)
        {
            m_StageFsm.PushEvent(new KFsmEvent(eventId, args));
        }
    }
}
