using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.GameEvent;
using Game.Common;

namespace Game.GameLogic
{
    public class GLStage
    {
        // 状态机
        GLStageFsm m_fsm;

        // 逻辑场景
        public GLScene m_GLScene = null;

        // 模板
        public GLStageTemplate m_Template = null;

        // 逻辑场景Doodad
        public List<GLDoodad> m_GLDoodadList = new List<GLDoodad>();

        // 逻辑场景Npc
        public List<GLNpc> m_GLNpcList = new List<GLNpc>();

        // 逻辑场景炮塔
        private List<GLTower> m_TowerList = new List<GLTower>();    // 关卡中Tower列表
        public List<GLTower> TowerList
        {
            get { return m_TowerList; }
        }

        // 怪物总组数
        public int m_nNpcGroupCount = 0;
        // 当前第几组
        public int m_nCurNpcGroup = 0;
        // 当前第几组第几个
        public int m_nCurNpcGroupIndex = 0;

        // 上一个怪创建的时间
        public uint m_nLastNpcCreateTime = 0;

        // 进入两组NPC间隔的时间
        public uint m_nBetweenTime = 0;

        public void Init(int nStageId)
        {
            m_fsm = new GLStageFsm();
            m_fsm.Init(this, nStageId);
        }
        public void Start()
        {
            m_fsm.Start();
        }

        public void DoInit(int nStageId)
        {
            RegisterEvents();

            GLStageTemplate template = GLSettingManager.Instance().GetStageTemplate(nStageId);

            m_Template = template;
            //////////////////////////////////////////////////////////////////////////
            // 创建场景
            m_GLScene = new GLScene();
            m_GLScene.Init(template.nSceneId);
            //////////////////////////////////////////////////////////////////////////
            // 创建Doodad
            for (int i = 0; i < template.asDoodad.Count; i++)
            {
                GLDoodad doodad = new GLDoodad();
                doodad.Init(template.asDoodad[i].nTemplateId,template.asDoodad[i].nCellX,
                    template.asDoodad[i].nCellY, m_GLScene);
                m_GLScene.AddDoodad(doodad);

                m_GLDoodadList.Add(doodad);
            }
            //////////////////////////////////////////////////////////////////////////
            // 怪物总组数
            m_nNpcGroupCount = template.asNpc.Count;
            // 当前正在创建第几组
            m_nCurNpcGroup = 0;
            //////////////////////////////////////////////////////////////////////////
            // 创建萝卜
            GLRadish radish = new GLRadish();
            radish.Init(template.nRadishTemplateId, template.nRadishCellX, template.nRadishCellY, m_GLScene);
            m_GLScene.AddRadish(radish);
            //////////////////////////////////////////////////////////////////////////
            // 创建炮塔
            //for (int i = 0; i < template.asTower.Count; i++)
            //{
            //    GLTower tower = new GLTower();
            //    tower.Init(template.asTower[i].nTemplateId, template.asTower[i].nCellX,
            //        template.asTower[i].nCellY, m_GLScene);
            //    m_GLScene.AddTower(tower);

                //m_GLTowerList.Add(tower);
            //}

            //////////////////////////////////////////////////////////////////////////
            Common.Console.Write("游戏初始化中");
        }

        public void DoStart()
        {
            Common.Console.Write("游戏开始中");
        }

        public void DoReady()
        {
            Common.Console.Write("游戏准备中");
        }

        public void DoEnd()
        {
            Common.Console.Write("游戏结束中");
            UnRegisterEvents();
        }

        public void UnInit()
        {
            UnRegisterEvents();
        }
        public GLScene Scene
        {
            get { return m_GLScene; }
        }

        public List<GLNpc> NpcList
        {
            get { return m_GLNpcList; }
        }

        public void Run()
        {

        }

        public void Activate()
        {
            m_fsm.Activate();
        }

        public void ActivateNpc()
        {
            for (int i = 0; i < m_GLNpcList.Count; i++)
            {
                if (m_GLNpcList[i].m_nDelete == 0)
                {
                    m_GLNpcList[i].Activate();
                }
            }
        }

        public void ActivateTower()
        {
            for (int i = 0; i < m_TowerList.Count; i++)
            {
                if (m_TowerList[i] != null)
                {
                    m_TowerList[i].Activate();
                }
            }
        }

        // ret==1 所有组Npc创建完成
        // ret==2 本组Npc创建完成
        public int ActiveCreateGroupNpc()
        {
            if (m_nCurNpcGroup >= m_nNpcGroupCount)
                return 1;

            if (m_nCurNpcGroupIndex >= m_Template.asNpc[m_nCurNpcGroup].asNpc.Count)
            {
                m_nCurNpcGroup++;
                m_nCurNpcGroupIndex = 0;

                return 2;
            }

            // 创建Npc的间隔
            if (Environment.TickCount - m_nLastNpcCreateTime >= m_Template.asNpc[m_nCurNpcGroup].nInterval)
            {
                GLPath path = GLSettingManager.Instance().GetGLPath(m_Template.asNpc[m_nCurNpcGroup].nPathId);
                int nStartCellX = path.m_PointList[0].nCellX;
                int nStartCellY = path.m_PointList[0].nCellY;

                if (m_nCurNpcGroupIndex < m_Template.asNpc[m_nCurNpcGroup].asNpc.Count)
                {
                    int nNpcTemplate = m_Template.asNpc[m_nCurNpcGroup].asNpc[m_nCurNpcGroupIndex];
                    GLNpc npc = new GLNpc();
                    npc.Init(nNpcTemplate, nStartCellX, nStartCellY, m_GLScene);
                    npc.SetPath(m_Template.asNpc[m_nCurNpcGroup].nPathId);
                    m_GLScene.AddNpc(npc);

                    m_GLNpcList.Add(npc);

                    m_nLastNpcCreateTime = (uint)Environment.TickCount;
                    m_nCurNpcGroupIndex++;

                    Common.Console.Write("创建Npc第" + m_nCurNpcGroup.ToString() + "组 第" + m_nCurNpcGroupIndex.ToString() + "个");
                }
            }
            return 0;
        }

        // ret==1 可以创建下一组
        public int ActiveCreateGroupNpcInterval()
        {
            if (m_GLNpcList.Count != 0)
                return 0;

            if (Environment.TickCount - m_nBetweenTime >= m_Template.nNpcGroupInterval)
                return 1;

            return 0;
        }

        //public void DestroyNpc()
        //{
        //    for (int i = 0; i < m_GLNpcList.Count; i++)
        //    {
        //        if (m_GLNpcList[i].m_nDelete == 1)
        //        {
        //            m_GLNpcList[i].UnInit();
        //            m_GLNpcList.RemoveAt(i);

        //            i--;
        //        }
        //    }
        //}

        //////////////////////////////////////////////////////////////////////////
        // 事件处理
        private void RegisterEvents()
        {
            EventCenter.Event_DelNpc += OnDelNpcEvent;
        }

        private void UnRegisterEvents()
        {
            EventCenter.Event_DelNpc -= OnDelNpcEvent;
        }

        private void OnDelNpcEvent(object sender, EventDef.DelNpcArgs args)
        {
            try
            {
                args.npc.UnInit();
                m_GLNpcList.Remove(args.npc);

                if (m_GLNpcList.Count == 0)
                {
                    m_nBetweenTime = (uint)Environment.TickCount;
                }
            }
            catch (Exception e)
            {
                Common.ExceptionTool.ProcessException(e);
            }
        }
        //////////////////////////////////////////////////////////////////////////
        

    }
}
