using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using UnityEngine;

namespace Game.GameLogic
{
    // 关卡
    public class GLScene
    {
        public List<GLNpc>    m_asNpctList   = new List<GLNpc>();       // 关卡中NPC列表
        public List<GLTurret> m_asTurretList = new List<GLTurret>();    // 关卡中Turret列表

        // 关卡中路径集合 Key从1开始
        private Dictionary<int, GLScenePath> m_PathList = new Dictionary<int, GLScenePath>();

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

            // 创建路径集合
            for (int i = 0; i < cfg.ScenePathList.Count(); ++i)
            {
                m_PathList[i + 1] = cfg.ScenePathList[i];
            }

            // 创建NPC
            GLNpc npc = AddNpc(1, 0, 0);
            npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Stand);
            npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Right);
            npc.SetPath(m_PathList[1]);

            //npc = AddNpc(2, 1, 1);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Run);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Up);

            //npc = AddNpc(3, 2, 2);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Left);

            //npc = AddNpc(4, 3, 3);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Hurt);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Down);

            //npc = AddNpc(5, 4, 4);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Stand);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Right);

            //npc = AddNpc(6, 5, 5);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Run);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Up);

            //npc = AddNpc(7, 6, 6);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Left);

            //npc = AddNpc(8, 7, 7);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Hurt);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Down);

            //npc = AddNpc(9, 8, 8);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Up);

            //npc = AddNpc(10, 9, 9);
            //npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Attack);
            //npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Down);
        }

        public void UnInit()
        {

        }

        public GLNpc AddNpc(int nTemplateId, int nLogicX, int nLogicY)
        {
            GLNpc npc = new GLNpc();
            npc.Init(nTemplateId, this);
            npc.SetPosition(nLogicX, nLogicY);

            m_asNpctList.Add(npc);

            return npc;
        }

        public bool CanAddTurret(int nLogicX, int nLogicY)
        {
            RLCell cell = m_RLScene.GetRLCell(nLogicX, nLogicY);
            if (null == cell)
                return false;

            if (cell.nType != (int)SceneCellType.SceneCellType_Idel)
                return false;

            return true;
        }

        public GLTurret AddTurret(int nTemplateId, int nLogicX, int nLogicY)
        {
            if (!CanAddTurret(nLogicX, nLogicY))
            {
                Debug.LogFormat("[Error] Can't Plant Turrent here! nLogicX = {0}, nLogicY = {1}", nLogicX, nLogicY);
                return null;
            }

            // 场景中添加Turret
            GLTurret turret = new GLTurret();
            turret.Init(nTemplateId, this);
            turret.SetPosition(nLogicX, nLogicY);
            m_asTurretList.Add(turret);

            // 修改Cell信息
            RLCell cell = m_RLScene.GetRLCell(nLogicX, nLogicY);
            cell.nType = (int)SceneCellType.SceneCellType_Obstacle;

            return turret;
        }

        public void DelNpc()
        {

        }

        public void Activate()
        {
            ActivateNpcList();
            ActivateTurretList();
        }

        public void ActivateNpcList()
        {
            for (int i = 0; i < m_asNpctList.Count; i++)
            {
                if (m_asNpctList[i] != null)
                {
                    m_asNpctList[i].Activate();
                }
            }
        }

        public void ActivateTurretList()
        {
            for (int i = 0; i < m_asTurretList.Count; i++)
            {
                if (m_asTurretList[i] != null)
                {
                    m_asTurretList[i].Activate();
                }
            }
        }
    }
}
