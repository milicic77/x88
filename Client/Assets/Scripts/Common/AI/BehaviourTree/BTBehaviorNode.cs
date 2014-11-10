using System.Collections;
using System.Collections.Generic;

namespace Game.Common
{
    // BTBehaviorNode(叶子节点) = BTConditionNode + BTActionNode
    public abstract class BTBehaviorNode : BTNode
    {
        public override void Activate()
        {
        }
    }

    public delegate bool BTCondHandler(object arg);
    public class BTCondition : BTBehaviorNode
    {
        protected BTCondHandler m_Handler = null;                       // 条件判断函数
        protected object        m_Arg     = null;

        public BTCondHandler CondHandler
        {
            get { return m_Handler;  }
            set { m_Handler = value; }
        }

        public object Arg
        {
            get { return m_Arg;  }
            set { m_Arg = value; }
        }

        public override void Activate()
        {
            if (null == m_Handler)
            {
                m_State = BTTaskState.FAILURE;
                return;
            }

            m_State = m_Handler(m_Arg) ? BTTaskState.SUCCESS : BTTaskState.FAILURE;
        }
    }

    public class BTActionNode : BTBehaviorNode
    {
        public override void Activate()
        {
        }
    }
}
