using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.GameEvent;

namespace Game.GameLogic
{
    public class GLStage
    {
        // 逻辑场景
        private GLScene m_GLScene = null;

        // 模板
        private GLStageTemplate m_Template = null;

        // 逻辑场景Doodad
        private List<GLDoodad> m_GLDoodadList = new List<GLDoodad>();

        // 逻辑场景Npc
        private List<GLNpc> m_GLNpcList = new List<GLNpc>();

        private List<GLTower> m_TowerList = new List<GLTower>();    // 关卡中Tower列表
        public List<GLTower> TowerList
        {
            get { return m_TowerList; }
        }

        // 怪物总波数
        private int m_nNpcGroupCount = 0;

        // 当前第几波
        private int m_nCurNpcGroupIndex = 0;

        // 当前第出现怪在组内的索引
        private int m_nCurNpcIndex = 0;

        // 上一个怪创建的时间
        private uint m_nLastNpcCreateTime = 0;

        // 处于两组NPC的间隔
        private int m_nBetween = 0;
        // 进入两组NPC间隔的时间
        private uint m_nBetweenTime = 0;

        // 创建NPC完成
        private int m_nCreateNpcFinish = 0;

        private int m_nStart = 0;

        public void Init(int nStageId)
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
            // 怪物总波数
            m_nNpcGroupCount = template.asNpc.Count;
            // 当前第几波
            m_nCurNpcGroupIndex = 0;
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

            //    //m_GLDoodadList.Add(doodad);
            //}
            //////////////////////////////////////////////////////////////////////////
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
            m_nStart = 1;
        }

        public void Activate()
        {
            //if (m_nStart == 0)
            //    return;
            //    

            //DestroyNpc();
            CreateNpc();

            //for (int i = 0; i < m_GLNpcList.Count; i++)
            //{
            //    if (m_GLNpcList[i].m_nDelete == 1)
            //    {
            //        m_GLNpcList[i].Activate();
            //    }
            //}

            for (int i = 0; i < m_GLNpcList.Count; i++)
            {
                if (m_GLNpcList[i].m_nDelete == 0)
                {
                    m_GLNpcList[i].Activate();
                }
            }

            ActivateTowerList();
        }

        public void CreateNpc()
        {
            if (m_nCreateNpcFinish == 1)
                return;

            // 所有组NPC都已经生成完
            if (m_nCurNpcGroupIndex >= m_nNpcGroupCount)
            {
                m_nCreateNpcFinish = 1;
                return;
            }

            if (m_nCurNpcIndex >= m_Template.asNpc[m_nCurNpcGroupIndex].asNpc.Count)
            {
                m_nCurNpcGroupIndex++;
                m_nCurNpcIndex = 0;

                m_nBetween = 1;
                m_nBetweenTime = (uint)Time.time;

                return;
            }

            if (m_nBetween == 1)
            {
                // 波次之间5秒
                if (Time.time - m_nBetweenTime < 5)
                {
                    return;
                }
                else
                {
                    m_nBetween = 0;
                    m_nBetweenTime = 0;
                }
            }

            // 1秒创建一个
            if (Time.time - m_nLastNpcCreateTime >= 2)
            {
                GLPath path = GLSettingManager.Instance().GetGLPath(m_Template.asNpc[m_nCurNpcGroupIndex].nPathId);
                int nStartCellX = path.m_PointList[0].nCellX;
                int nStartCellY = path.m_PointList[0].nCellY;

                if (m_nCurNpcIndex < m_Template.asNpc[m_nCurNpcGroupIndex].asNpc.Count)
                {
                    int nNpcTemplate = m_Template.asNpc[m_nCurNpcGroupIndex].asNpc[m_nCurNpcIndex];
                    GLNpc npc = new GLNpc();
                    npc.Init(nNpcTemplate, nStartCellX, nStartCellY, m_GLScene);
                    npc.SetPath(m_Template.asNpc[m_nCurNpcGroupIndex].nPathId);
                    m_GLScene.AddNpc(npc);

                    m_GLNpcList.Add(npc);

                    m_nLastNpcCreateTime = (uint)Time.time;
                    m_nCurNpcIndex++;
                }
            }
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
            }
            catch (Exception e)
            {
                Common.ExceptionTool.ProcessException(e);
            }
        }

        public void ActivateTowerList()
        {
            for (int i = 0; i < m_TowerList.Count; i++)
            {
                if (m_TowerList[i] != null)
                {
                    m_TowerList[i].Activate();
                }
            }
        }

    }
}
