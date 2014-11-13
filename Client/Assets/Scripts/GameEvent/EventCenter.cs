using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameEvent
{
    public class EventCenter
    {
        ////////////////////////////////////////////////////////////////////////// 
        // 游戏客户端初始化完毕事件
        public static event EventDef.Event_Common m_EventGameClientInitComplete;
        public static EventDef.Event_Common Event_ClientInitComplete
        {
            get
            {
                return m_EventGameClientInitComplete;
            }
            set
            {
                m_EventGameClientInitComplete = value;
            }
        }
        //////////////////////////////////////////////////////////////////////////
        // 增加Npc事件
        public static event EventDef.Event_AddNpc m_EventAddNpc;
        public static EventDef.Event_AddNpc Event_AddNpc
        {
            get
            {
                return m_EventAddNpc;
            }
            set
            {
                m_EventAddNpc = value;
            }
        }
        //////////////////////////////////////////////////////////////////////////
        // Npc啃到萝卜（到达终点）
        public static event EventDef.Event_NpcAttackRadish m_EventNpcAttackRadish;
        public static EventDef.Event_NpcAttackRadish Event_NpcAttackRadish
        {
            get
            {
                return m_EventNpcAttackRadish;
            }
            set
            {
                m_EventNpcAttackRadish = value;
            }
        }
        //////////////////////////////////////////////////////////////////////////
        // 关卡开始
        public static event EventDef.Event_LevelStart m_EventLevelStart;
        public static EventDef.Event_LevelStart Event_LevelStart
        {
            get
            {
                return m_EventLevelStart;
            }
            set
            {
                m_EventLevelStart = value;
            }
        }
        //////////////////////////////////////////////////////////////////////////
        // 游戏结束
        public static event EventDef.Event_GameOver m_EventGameOver;
        public static EventDef.Event_GameOver Event_GameOver
        {
            get
            {
                return m_EventGameOver;
            }
            set
            {
                m_EventGameOver = value;
            }
        }
        //////////////////////////////////////////////////////////////////////////
        // 游戏结束
        public static event EventDef.Event_NpcHurt m_EventNpcHurt;
        public static EventDef.Event_NpcHurt Event_NpcHurt
        {
            get
            {
                return m_EventNpcHurt;
            }
            set
            {
                m_EventNpcHurt = value;
            }
        }
        //////////////////////////////////////////////////////////////////////////
    }
}
