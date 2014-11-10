using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GLTower
    {
        // 表现炮塔
        public RLTower m_RLTower = null;

        // 逻辑坐标
        private int m_nLogicX = 0;
        private int m_nLogicY = 0;

        public void Init(int nTemplateId, int nCellX, int nCellY, GLScene scene)
        {
            GLTowerTemplate template = GLSettingManager.Instance().GetGLTowerTemplate(nTemplateId);

            // 格子坐标 => 逻辑坐标
            int nLogicX = RepresentCommon.CellX2LogicX(nCellX);
            int nLogicY = RepresentCommon.CellY2LogicY(nCellY);

            // 逻辑坐标 => 世界坐标
            float fWorldX = RepresentCommon.LogicX2WorldX(nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(nLogicY);

            m_RLTower = Represent.Instance().CreateTower(template.nRepresentId, fWorldX, fWorldY);

            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;
        }

        public void UnInit()
        {

        }

        public void Activate()
        {

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
