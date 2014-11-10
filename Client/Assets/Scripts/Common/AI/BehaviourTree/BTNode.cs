using System.Collections;
using System.Collections.Generic;

namespace Game.Common
{
    public enum BTTaskState
    {
        READY = 0,                                                      // 初始状态
        RUNNING,                                                        // 正在运行
        SUCCESS,                                                        // 执行成功
        FAILURE,                                                        // 执行失败
    }

    public interface IBTNode
    {
        string        Name  { get; set; }                               // 节点名称
        IBTNode       Parent{ get; set; }                               // 父亲节点
        BTTaskState   State { get; set; }                               // 任务状态
        BehaviourTree Owner { get; set; }                               // 树拥有者

        void Activate();                                                // 更新函数
    }

    public abstract class BTNode : IBTNode
    {
        protected string        m_Name   = "";                          // 节点名
        protected IBTNode       m_Parent = null;                        // 父节点
        protected BTTaskState   m_State  = BTTaskState.READY;           // 任务状态
        protected BehaviourTree m_Owner = null;                         // 树拥有者

        public BTNode()
        {
            m_Name  = this.GetType().Name;
        }

        public string Name
        {
            get { return m_Name;  }
            set { m_Name = value; }
        }

        public IBTNode Parent
        {
            get { return m_Parent;  }
            set { m_Parent = value; }
        }

        public BTTaskState State
        {
            get { return m_State;  }
            set { m_State = value; }
        }

        public BehaviourTree Owner
        {
            get { return m_Owner;  }
            set { m_Owner = value; }
        }

        public virtual void Activate()
        {
        }
    }
}
