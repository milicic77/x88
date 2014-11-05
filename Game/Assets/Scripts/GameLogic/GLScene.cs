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
            AddNpc(1, 0, 0);
            AddNpc(2, 1, 1);
            AddNpc(3, 2, 2);
            AddNpc(4, 3, 3);
            AddNpc(5, 4, 4);
            AddNpc(6, 5, 5);
            AddNpc(7, 6, 6);
            AddNpc(8, 7, 7);
            AddNpc(9, 8, 8);
            AddNpc(10, 9, 9);
        }

        public void UnInit()
        {

        }

        public void AddNpc(int nTemplateId, int nLogicX, int nLogicY)
        {
            GLNpc npc = new GLNpc();
            npc.Init(nTemplateId, this);
            npc.SetPosition(nLogicX, nLogicY);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Stand);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Right);

            asNpctList.Add(npc);
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
