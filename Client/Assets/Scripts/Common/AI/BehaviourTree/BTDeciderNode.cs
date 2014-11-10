using System.Collections;
using System.Collections.Generic;

namespace Game.Common
{
    // BTDeciderNode(树根节点) = BTCompositeNode + BTDecoratorNode
    public abstract class BTDeciderNode : BTNode
    {
        protected List<IBTNode> m_Children = new List<IBTNode>();       // 叶子节点

        public void AddNode(IBTNode node)
        {
            if (null == node)
                return;

            if (m_Children.Contains(node))
                return;

            node.Parent = this;
            node.Owner  = m_Owner;

            m_Children.Add(node);
        }

        public void DelNode(IBTNode node)
        {
            if (null == node)
                return;

            if (!m_Children.Contains(node))
                return;

            node.Parent = null;
            node.Owner  = null;

            m_Children.Remove(node);
        }

        public bool HasNode(IBTNode node)
        {
            if (null == node)
                return false;

            return m_Children.Contains(node);
        }
    }

    // ---------------------------------- BTComposite Node ----------------------------------//
    // BTCompositeNode = BTSelectorNode + BTSequenceNode + BTParallelNode
    public abstract class BTCompositeNode : BTDeciderNode
    {
    }

    public class BTSelector : BTCompositeNode
    { // selector节点的子节点 = selector节点 + sequence节点 + action节点
    }

    // 每次从左向右依次选择，所以子节点的前提设定必须是从窄到宽的方式
    public class BTPrioritySelector : BTSelector
    {
    }

    // 每次从上一个执行过的子节点开始进行选择，这种选择方式不存在节点优先判断，所以就要保证前提条件之间的互斥性
    public class BTNonPrioritySelector : BTSelector
    {
        protected IBTNode m_LastNode = null;

        public override void Activate()
        {
            int nChildNum = m_Children.Count;
            if (nChildNum <= 0)
            { // 子节点个数 <= 0
                m_State = BTTaskState.FAILURE;
                return;
            }

            int nStartIdx = 0;
            if (null != m_LastNode)
            {
                nStartIdx = m_Children.IndexOf(m_LastNode);
                nStartIdx = nStartIdx >= 0 ? nStartIdx : 0;
            }

            IBTNode node = null;
            for (int i = 0; i < nChildNum; i++)
            {
                node = m_Children[nStartIdx];
                node.Activate();

                if (BTTaskState.FAILURE != node.State)
                {
                    m_LastNode = node;
                    break;
                }

                nStartIdx = (nStartIdx + 1) % nChildNum;
            }

            m_State = node.State;
        }
    }

    public class BTWeightedSelector : BTSelector
    {
    }

    public class BTParallelNode : BTCompositeNode
    {
    }

    public class BTSequenceNode : BTCompositeNode
    {
        protected List<IBTNode> m_Conditions = new List<IBTNode>();
        protected IBTNode       m_LastNode   = null;

        public void AddCond(IBTNode node)
        {
            if (null == node)
                return;

            if (m_Conditions.Contains(node))
                return;

            node.Parent = this;
            node.Owner  = m_Owner;

            m_Conditions.Add(node);
        }

        public void DelCond(IBTNode node)
        {
            if (null == node)
                return;

            if (!m_Conditions.Contains(node))
                return;

            node.Parent = null;
            node.Owner  = null;

            m_Conditions.Remove(node);
        }

        public bool HasCond(IBTNode node)
        {
            if (null == node)
                return false;

            return m_Conditions.Contains(node);
        }

        public override void Activate()
        {
            if (!CheckConds())
            {
                m_State = BTTaskState.FAILURE;
                return;
            }

            int nChildNum = m_Children.Count;
            if (nChildNum <= 0)
            { // 子节点个数 <= 0
                m_State = BTTaskState.FAILURE;
                return;
            }

            int nStartIdx = 0;
            if (null != m_LastNode)
            {
                nStartIdx = m_Children.IndexOf(m_LastNode);
                nStartIdx = nStartIdx >= 0 ? nStartIdx : 0;
            }

            if (BTTaskState.RUNNING != m_LastNode.State)
            {
                nStartIdx = 0;
            }

            IBTNode node = null;
            for (int i = nStartIdx; i < nChildNum; i++)
            {
                node = m_Children[i];
                node.Activate();

                m_State = node.State;

                if (BTTaskState.RUNNING == node.State)
                {
                    m_LastNode = node;
                    break;
                }

                if (BTTaskState.SUCCESS == node.State)
                {
                    continue;
                }

                if (BTTaskState.FAILURE == node.State)
                {
                    break;
                }
            }
        }

        protected virtual bool CheckConds()
        {
            for (int i = 0, nCondNum = m_Conditions.Count; i < nCondNum; i++)
            {
                IBTNode cond = m_Conditions[i];
                cond.Activate();

                if (BTTaskState.FAILURE == cond.State)
                {
                    return false;
                }
            }

            return true;
        }
    }

    // BTDecoratorNode
    public class BTDecoratorNode : BTDeciderNode
    {

    }
}
