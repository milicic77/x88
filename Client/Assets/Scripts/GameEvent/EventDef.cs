using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameLogic;

namespace Game.GameEvent
{
    public class EventDef
    {
        //////////////////////////////////////////////////////////////////////////
        //// 事件枚举
        //public enum EVENT_ID
        //{
        //    EVENT_CLIENT_INIT_COMPLETE = 1, // 客户端初始化完成
        //    EVENT_ADDNPC = 2, // 创建NPC
        //    EVENT_DELNPC = 3, // 删除NPC

        //    EVENT_COUNT,     // 事件数量
        //}

        //////////////////////////////////////////////////////////////////////////
        // 通用事件
        public delegate void Event_Common(object sender, BaseEventArgs args);
        public class BaseEventArgs : EventArgs
        {
            public int[] eventParam = new int[5];
        }
        //////////////////////////////////////////////////////////////////////////
        // 增加Npc事件
        public delegate void Event_AddNpc(object sender, AddNpcArgs args);
        public class AddNpcArgs : BaseEventArgs
        {
            public UInt32 id;
            public UInt32 posX;
            public UInt32 posY;
            public UInt32 posZ;
            public UInt32 genre;
            public UInt32 detail;
            public UInt32 particular;
            public string name;
        }
        //////////////////////////////////////////////////////////////////////////
        // Npc啃到萝卜（到达终点）
        public delegate void Event_NpcAttackRadish(object sender, NpcAttackRadishArgs args);
        public class NpcAttackRadishArgs : BaseEventArgs
        {
            public GLNpc npc;
        }
        //////////////////////////////////////////////////////////////////////////
        // 关卡开始
        public delegate void Event_LevelStart(object sender, BaseEventArgs args);
        //////////////////////////////////////////////////////////////////////////
        // 游戏结束事件
        public delegate void Event_GameOver(object sender, BaseEventArgs args);

        //////////////////////////////////////////////////////////////////////////
        // Npc被打到
        public delegate void Event_NpcHurt(object sender, NpcHurtArgs args);
        public class NpcHurtArgs : BaseEventArgs
        {
            public GLNpc npc;
            public GLMissile missile;
        }
        //////////////////////////////////////////////////////////////////////////
    }
}
