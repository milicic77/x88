using System.Collections;
using System.Collections.Generic;

namespace Game.Common
{
    public class Blackboard
    {
        private Dictionary<string, object> m_DataStore = new Dictionary<string, object>();

        public void AddData(string name, object data)
        {
            m_DataStore[name] = data;
        }

        public void DelData(string name, object data)
        {
            if (!m_DataStore.ContainsKey(name))
                return;

            m_DataStore.Remove(name);
        }

        public object GetData(string name)
        {
            if (!m_DataStore.ContainsKey(name))
                return null;

            return m_DataStore[name];
        }
    }
    public class BehaviourTree
    {
        private IBTNode    m_RootNode;
        private Blackboard m_Blackboard = new Blackboard();

        public IBTNode RootNode
        {
            get { return m_RootNode;  }
            set { m_RootNode = value; }
        }

        public Blackboard Blackboard
        {
            get { return m_Blackboard;  }
            set { m_Blackboard = value; }
        }

        public void Activate()
        {
            m_RootNode.Activate();
        }
    }
}
