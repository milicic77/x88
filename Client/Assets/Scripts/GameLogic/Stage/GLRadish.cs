using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GLRadish
    {
        // 表现Npc
        public RLRadish m_RLRadish = null;

        // 模板
        public GLRadishTemplate m_Template = null;
        // 逻辑坐标
        private int m_nLogicX = 0;
        private int m_nLogicY = 0;

        public uint m_LastAniTime = 0;
        public int m_nAniIndex = 0;

        // 当前血量
        public int m_nLife = 0;

        // 受伤特效
        public GLEffect m_GLEffect = new GLEffect();

        // 所在场景
        public GLScene m_GLScene;

        public void Init(int nTemplateId, int nCellX, int nCellY, GLScene scene)
        {
            GLRadishTemplate template = GLSettingManager.Instance().GetGLRadishTemplate(nTemplateId);

            m_Template = template;

            m_GLScene = scene;

            m_nLife = template.nLife;
            // 格子坐标 => 逻辑坐标
            int nLogicX = RepresentCommon.CellX2LogicX(nCellX);
            int nLogicY = RepresentCommon.CellY2LogicY(nCellY);

            // 逻辑坐标 => 世界坐标
            float fWorldX = RepresentCommon.LogicX2WorldX(nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(nLogicY);

            m_RLRadish = Represent.Instance().CreateRadish(template.nRepresentId, fWorldX, fWorldY);

            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;

            // 受伤特效
            m_GLEffect.Init(1, nCellX, nCellY, m_GLScene);
            m_GLScene.AddEffect(m_GLEffect);
        }

        public void UnInit()
        {

        }

        public void ChangeRepresent(int nRepresentId)
        {
            Represent.Instance().DestroyRadish(m_RLRadish);
            float fWorldX = RepresentCommon.LogicX2WorldX(m_nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(m_nLogicY);
            m_RLRadish = Represent.Instance().CreateRadish(
                nRepresentId, fWorldX, fWorldY);
        }

        public bool IsFullLife()
        {
            if (m_nLife >= m_Template.nLife)
                return true;
            return false;
        }

        public void OnHurt()
        {
            m_nLife--;
            
            // 只播放1次
            m_GLEffect.Play(1);
        }

        public void Activate()
        {
            if (IsFullLife() == true) // 满血状态才做空闲动作
            {
                if (Environment.TickCount - m_LastAniTime >= 5000)
                {
                    if (m_nAniIndex == 0)
                    {
                        m_RLRadish.DoStandAni_1();
                    }
                    else if (m_nAniIndex == 1)
                    {
                        m_RLRadish.DoStandAni_2();
                    }

                    m_nAniIndex++;
                    m_nAniIndex = m_nAniIndex % 2;

                    m_LastAniTime = (uint)Environment.TickCount;
                }
            }

        }

        //    public void SetPosition(int nLogicX, int nLogicY)
        //    {
        //        m_nLogicX = nLogicX;
        //        m_nLogicY = nLogicY;

        //        m_RLSceneObject.SetPosition(nLogicX, nLogicY);
        //    }

        //    public void SetDoing(int nDoing)
        //    {
        //        m_nDoing = nDoing;

        //        m_RLSceneObject.DOING = nDoing;
        //    }

        //    public void SetDirection(int nDirection)
        //    {
        //        m_nDirection = nDirection;

        //        m_RLSceneObject.DIRECTION = nDirection;
        //    }

        //    public void SetPath(GLPath path)
        //    {
        //        m_Path = path;
        //        m_nPathIndex = 0;
        //    }

    }
}
