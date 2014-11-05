using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    // 关卡
    public class GLScene
    {
        // 场景中NPC列表
        public List<GLNpc> asNpctList = new List<GLNpc>();

        // 表现逻辑场景
        private RLScene m_RLScene;

        public RLScene RepresentScene
        {
            set
            {
                m_RLScene = value;
            }
            get
            {
                return m_RLScene;
            }
        }

        public void Init(ref GLSceneConfig cfg)
        {
            // 创建游戏场景
            m_RLScene = Represent.Instance().CreateScene(cfg.nTemplateId);

            // 创建NPC
            GLNpc npc = AddNpc(1, 0, 0);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Stand);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Right);

            npc = AddNpc(2, 1, 1);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Run);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Up);

            npc = AddNpc(3, 2, 2);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Left);

            npc = AddNpc(4, 3, 3);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Hurt);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Down);

            npc = AddNpc(5, 4, 4);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Stand);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Right);

            npc = AddNpc(6, 5, 5);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Run);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Up);

            npc = AddNpc(7, 6, 6);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Left);

            npc = AddNpc(8, 7, 7);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Hurt);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Down);

            npc = AddNpc(9, 8, 8);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Up);

            npc = AddNpc(10, 9, 9);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Down);
        }

        public void UnInit()
        {

        }

        public GLNpc AddNpc(int nTemplateId, int nLogicX, int nLogicY)
        {
            GLNpc npc = new GLNpc();
            npc.Init(nTemplateId, this);
            npc.SetPosition(nLogicX, nLogicY);

            asNpctList.Add(npc);

            return npc;
        }

        public void DelNpc()
        {

        }

        public void Activate()
        {
            for (int i = 0; i < asNpctList.Count; i++)
            {
                if (asNpctList[i] != null)
                {
                    asNpctList[i].Activate();
                }
            }
        }
    }
}
