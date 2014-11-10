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
        // 删除Npc事件
        public static event EventDef.Event_DelNpc m_EventDelNpc;
        public static EventDef.Event_DelNpc Event_DelNpc
        {
            get
            {
                return m_EventDelNpc;
            }
            set
            {
                m_EventDelNpc = value;
            }
        }
        //////////////////////////////////////////////////////////////////////////
    }
}
