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

    public class BTCondition : BTBehaviorNode
    {
        public override void Activate()
        {
            m_State = BTTaskState.FAILURE;
        }
    }

    public class BTActionNode : BTBehaviorNode
    {
        public override void Activate()
        {
        }
    }
}
