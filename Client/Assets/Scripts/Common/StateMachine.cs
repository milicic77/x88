using System.Collections;
using System.Collections.Generic;

/* The first enum lable in LinkId & StateID must be 0
public enum LinkId
{
    NullTrans = 0,
}
public enum StateID
{
    NullStateID = 0,
}
*/

namespace Game.Common
{
    public sealed class KFsm
    {
        private Queue<KFsmEvent> m_eventQueue;
        private Dictionary<KFsmState, KFsmLinkSet> m_LinkMap;
        public KFsmState m_CurState;
        private KFsmState m_StartupState;
        private bool m_Running;

        public void InitFsm(object[,] map, KFsmState[] states, object owner, KFsm root, int startupStateIdx)
        {
            m_eventQueue = new Queue<KFsmEvent>();
            m_LinkMap = new Dictionary<KFsmState, KFsmLinkSet>();
            m_StartupState = states[startupStateIdx];
            m_Running = false;

            foreach (KFsmState state in states)
            {
                state.Init(owner, this, root);
            }
            for (int i = 0; i < map.GetLength(0); ++i)
            {
                KFsmState state = (KFsmState)map[i, 0];
                if (!m_LinkMap.ContainsKey(state))
                    m_LinkMap[state] = new KFsmLinkSet();
                m_LinkMap[state].AddLink((int)map[i, 1], new KFsmLink((KFsmState)map[i, 0], (KFsmState)map[i, 2]));
            }
        }

        public void Startup(object[] args = null)
        {
            m_eventQueue.Clear();
            m_CurState = m_StartupState;
            m_Running = true;
            m_CurState.Enter(args);
        }

        public void Update()
        {
            if (!m_Running)
                return;

            DispatchEvent();

            if (m_CurState != null)
            {
                m_CurState.Update();
            }
        }

        public bool DoTrans(int linkId, object[] args = null)
        {
            if (m_LinkMap[m_CurState].m_LinkSet.ContainsKey(linkId))
            {
                KFsmTrans trans = new KFsmTrans(m_LinkMap[m_CurState].m_LinkSet[linkId], args);
                return DoTrans(trans);
            }
            if (m_CurState is KFsmCompositeState)
            {
                return ((KFsmCompositeState)m_CurState).m_SubFsm.DoTrans(linkId, args);
            }
            throw new System.Exception("Invalid trans linkId: " + linkId + " in state: " + m_CurState.ToString());
        }

        private bool DoTrans(KFsmTrans trans)
        {
            if (trans != null)
            {
                Console.Write(string.Format("DoTrans: m_CurState({0}), m_NextState({1})", m_CurState.ToString(), trans.m_Link.m_To.ToString()));
                if (m_CurState != null)
                {
                    m_CurState.Exit(trans);
                }
                m_CurState = trans.m_Link.m_To;
                m_CurState.Enter(trans.m_Args);
                return true;
            }
            return false;
        }

        public void PushEvent(int eventId, object[] args = null)
        {
            PushEvent(new KFsmEvent(eventId, args));
        }

        public void PushEvent(KFsmEvent evt)
        {
            m_eventQueue.Enqueue(evt);
        }

        public void DispatchEvent()
        {
            while (m_eventQueue.Count > 0)
            {
                m_CurState.OnEvent(m_eventQueue.Dequeue());
            }
        }
    }

    public class KFsmState
    {
        protected Dictionary<int, int> m_EventTransMap;
        protected object m_Owner;
        public KFsm m_Fsm;
        protected KFsm m_FsmRoot;
        public KFsmState()
        {
            m_EventTransMap = new Dictionary<int, int>();
        }
        public virtual void Init(object owner, KFsm fsm, KFsm root)
        {
            m_Owner = owner;
            m_Fsm = fsm;
            m_FsmRoot = root;
            InitEventTransMap();
        }
        public virtual void InitEventTransMap() { }
        public virtual void Enter(object[] args = null) { }
        public virtual void Update() { }
        public virtual void Exit(KFsmTrans trans) { }
        public virtual void OnEvent(KFsmEvent evt)
        {
            if (m_EventTransMap.ContainsKey((int)evt.m_EventId))
            {
                m_FsmRoot.DoTrans(m_EventTransMap[(int)evt.m_EventId], evt.m_Args);
                return;
            }
            throw new System.Exception("Unhandled event! EventId: " + evt.m_EventId + " in state: " + this.ToString());
        }
    }

    public class KFsmCompositeState : KFsmState
    {
        public KFsm m_SubFsm;
        public override void Init(object owner, KFsm fsm, KFsm root)
        {
            base.Init(owner, fsm, root);
            InitFsm();
        }
        public virtual void InitFsm() { throw new System.NotImplementedException(); }
        public override void Enter(object[] args = null)
        {
            m_SubFsm.Startup(args);
        }
        public override void Update()
        {
            m_SubFsm.Update();
        }
        public override void OnEvent(KFsmEvent evt)
        {
            m_SubFsm.PushEvent(evt);
        }
    }

    public class KFsmLinkSet
    {
        public Dictionary<int, KFsmLink> m_LinkSet;
        public KFsmLinkSet()
        {
            m_LinkSet = new Dictionary<int, KFsmLink>();
        }
        public bool AddLink(int linkId, KFsmLink link)
        {
            if (m_LinkSet.ContainsKey(linkId))
                return false;
            m_LinkSet.Add(linkId, link);
            return true;
        }
    }

    public class KFsmLink
    {
        public KFsmState m_From;
        public KFsmState m_To;
        public KFsmLink(KFsmState from, KFsmState to)
        {
            m_From = from;
            m_To = to;
        }
    }

    public class KFsmTrans
    {
        public KFsmLink m_Link;
        public object[] m_Args;
        public KFsmTrans(KFsmLink link, object[] args = null)
        {
            m_Link = link;
            m_Args = args;
        }
    }

    public class KFsmEvent
    {
        public int      m_EventId;
        public object[] m_Args;
        public KFsmEvent(int eventId, object[] args = null)
        {
            m_EventId   = eventId;
            m_Args      = args;
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// Example:
// 
//namespace Game.GameLogic
//{
//    public enum FsmLink
//    {
//        LINK_0_1,
//        LINK_1_2,
//        LINK_2_0,
//    }

//    public enum FsmEvent
//    {
//        EVENT_0_1,
//        EVENT_1_2,
//        EVENT_2_0,
//    }
//    public class FSMTest
//    {
//        private KFsm fsm = null;
//        public void Init()
//        {
//            fsm = new KFsm();
//            KFsmState[] state = new KFsmState[3];

//            state[0] = new FSMStateTest0();
//            state[1] = new FSMStateTest1();
//            state[2] = new FSMStateTest2();

//            object[,] map = new object[,]
//            {
//                {state[0], FsmLink.LINK_0_1, state[1]},
//                {state[1], FsmLink.LINK_1_2, state[2]},
//                {state[2], FsmLink.LINK_2_0, state[0]},
//            };



//            fsm.InitFsm(map, state, this, fsm, 0);

//            fsm.Startup();
//        }

//        public void Update()
//        {
//            fsm.Update();
//        }
//    }

//    public class FSMStateTest0 : KFsmState
//    {
//        public override void Enter(object[] args = null)
//        {
//            Common.Console.Write("Enter0");
//            m_FsmRoot.PushEvent(new KFsmEvent((int)FsmEvent.EVENT_0_1));
//        }

//        public override void OnEvent(KFsmEvent evt)
//        {
//            switch ((FsmEvent)evt.m_EventId)
//            {
//                case FsmEvent.EVENT_0_1:
//                    m_FsmRoot.DoTrans((int)FsmLink.LINK_0_1);
//                    break;
//                case FsmEvent.EVENT_1_2:
//                    break;
//                case FsmEvent.EVENT_2_0:
//                    break;
//                default:
//                    base.OnEvent(evt);
//                    break;
//            }
//        }
//    }

//    public class FSMStateTest1 : KFsmState
//    {
//        public override void Enter(object[] args = null)
//        {
//            Common.Console.Write("Enter1");

//            m_FsmRoot.PushEvent(new KFsmEvent((int)FsmEvent.EVENT_1_2));
//        }

//        public override void OnEvent(KFsmEvent evt)
//        {
//            switch ((FsmEvent)evt.m_EventId)
//            {
//                case FsmEvent.EVENT_0_1:
//                    break;
//                case FsmEvent.EVENT_1_2:
//                    m_FsmRoot.DoTrans((int)FsmLink.LINK_1_2);
//                    break;
//                case FsmEvent.EVENT_2_0:
//                    break;
//                default:
//                    base.OnEvent(evt);
//                    break;
//            }
//        }

//    }

//    public class FSMStateTest2 : KFsmState
//    {
//        public override void Enter(object[] args = null)
//        {
//            Common.Console.Write("Enter2");

//            m_FsmRoot.PushEvent(new KFsmEvent((int)FsmEvent.EVENT_2_0));
//        }

//        public override void OnEvent(KFsmEvent evt)
//        {
//            switch ((FsmEvent)evt.m_EventId)
//            {
//                case FsmEvent.EVENT_0_1:
//                    break;
//                case FsmEvent.EVENT_1_2:
//                    break;
//                case FsmEvent.EVENT_2_0:
//                    m_FsmRoot.DoTrans((int)FsmLink.LINK_2_0);
//                    break;
//                default:
//                    base.OnEvent(evt);
//                    break;
//            }
//        }
//    }
//}
