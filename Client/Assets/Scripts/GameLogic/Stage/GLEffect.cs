using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GLEffect
    {
        // 表现Npc
        public RLEffect m_RLEffect = null;

        // 模板
        public GLEffectTemplate m_Template = null;
        // 逻辑坐标
        private int m_nLogicX = 0;
        private int m_nLogicY = 0;

        public uint m_LastAniTime = 0;
        public int m_nAniIndex = 0;


        public void Init(int nTemplateId, int nCellX, int nCellY, GLScene scene)
        {
            GLEffectTemplate template = GLSettingManager.Instance().GetGLEffectTemplate(nTemplateId);

            m_Template = template;

            // 格子坐标 => 逻辑坐标
            int nLogicX = RepresentCommon.CellX2LogicX(nCellX);
            int nLogicY = RepresentCommon.CellY2LogicY(nCellY);

            // 逻辑坐标 => 世界坐标
            float fWorldX = RepresentCommon.LogicX2WorldX(nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(nLogicY);

            m_RLEffect = Represent.Instance().CreateEffect(template.nRepresentId, fWorldX, fWorldY);

            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;
        }

        public void UnInit()
        {

        }

        public void Play(int nCount)
        {
            m_RLEffect.Play(nCount);
        }

        public void Activate()
        {
            //if (IsFullLife() == true) // 满血状态才做空闲动作
            //{
            //    if (Environment.TickCount - m_LastAniTime >= 5000)
            //    {
            //        if (m_nAniIndex == 0)
            //        {
            //            m_RLRadish.DoStandAni_1();
            //        }
            //        else if (m_nAniIndex == 1)
            //        {
            //            m_RLRadish.DoStandAni_2();
            //        }

            //        m_nAniIndex++;
            //        m_nAniIndex = m_nAniIndex % 2;

            //        m_LastAniTime = (uint)Environment.TickCount;
            //    }
            //}

        }

    }
}
