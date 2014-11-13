using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.GameEvent;
using UnityEngine;

namespace Game.GameLogic
{
    public class GLNpc
    {
        // 表现Npc
        public RLNpc m_RLNpc = null;

        // 逻辑坐标
        private int m_nLogicX = 0;
        private int m_nLogicY = 0;

        public int LogicX
        {
            get { return m_nLogicX; }
            set { m_nLogicX = value;}
        }

        public int LogicY
        {
            get { return m_nLogicY; }
            set { m_nLogicY = value; }
        }
    //    private int m_nDoing = (int)SceneObjectAni.SceneObjectAni_Stand;
    //    private int m_nDirection = (int)SceneObjectDirection.SceneObjectDirection_Right;

        // 宽
        public int m_nWidth = 0;
        // 高
        public int m_nHeight = 0;

        public int Width
        {
            get { return m_nWidth; }
            set { m_nWidth = value; }
        }

        public int Height
        {
            get { return m_nHeight; }
            set { m_nHeight = value; }
        }

        public int m_nLogicCenterX = 0;// 逻辑坐标中的中心点X
        public int m_nLogicCenterY = 0;// 逻辑坐标中的中心点Y

        // 行走路径
        private GLPath m_Path;
        // 当前目的点是路径的第几个点
        private int m_nCurPointIndex = 0;

        public GLScene m_GLScene;
    //    private int m_nPathIndex = 0;

    //    private int nTime1 = Environment.TickCount;

        public int m_nDelete = 0;

        // 当前血量
        public int m_nLife = 0;

        // 出生特效
        public GLEffect m_GLEffect = new GLEffect();
        public void Init(int nTemplateId, int nCellX, int nCellY, GLScene scene)
        {
            GLNpcTemplate template = GLSettingManager.Instance().GetGLNpcTemplate(nTemplateId);

            m_GLScene = scene;

            // 格子坐标 => 逻辑坐标
            int nLogicX = RepresentCommon.CellX2LogicX(nCellX);
            int nLogicY = RepresentCommon.CellY2LogicY(nCellY);

            // 逻辑坐标 => 世界坐标
            float fWorldX = RepresentCommon.LogicX2WorldX(nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(nLogicY);

            m_RLNpc = Represent.Instance().CreateNpc(
                template.nRepresentId, fWorldX, fWorldY, this);

            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;

            m_nHeight = template.nHeight;
            m_nWidth = template.nWidth;

            m_nLife = template.nLife;

            // 出生特效
            m_GLEffect.Init(1, nCellX, nCellY, m_GLScene);
            m_GLScene.AddEffect(m_GLEffect);

            // 只播放1次
            m_GLEffect.Play(1);
        }

        public void UnInit()
        {
            Represent.Instance().DestroyNpc(m_RLNpc);
        }

        public int GetLogicCenterX()
        {
            m_nLogicCenterX = m_nLogicX;

            return m_nLogicCenterX;
        }

        public int GetLogicCenterY()
        {
            m_nLogicCenterY = m_nLogicY - m_nHeight / 2;
            return m_nLogicCenterY;
        }

        public void SetPath(int nPathId)
        {
            GLPath path = GLSettingManager.Instance().GetGLPath(nPathId);
            m_Path = path;
        }

        public void Activate()
        {
            if (m_nDelete == 1)
                return;

            // 行走速度（逻辑坐标）
            int nSpeed = 4;

            if (m_nCurPointIndex >= m_Path.m_PointList.Count)
            {
                // 到达终点，此时删除自身
                m_nDelete = 1;

                // 发起事件
                EventDef.NpcAttackRadishArgs args = new EventDef.NpcAttackRadishArgs();
                args.npc = this;
                EventCenter.Event_NpcAttackRadish(null, args);

                return;
            }

            int nDestX = m_Path.m_PointList[m_nCurPointIndex].nCellX;
            nDestX = RepresentCommon.CellX2LogicX(nDestX);

            int nDestY = m_Path.m_PointList[m_nCurPointIndex].nCellY;
            nDestY = RepresentCommon.CellY2LogicY(nDestY);

            if (m_nLogicX == nDestX && m_nLogicY == nDestY)
            {
                m_nCurPointIndex++;

                if (m_nCurPointIndex >= m_Path.m_PointList.Count)
                    return;

                nDestX = m_Path.m_PointList[m_nCurPointIndex].nCellX;
                nDestX = RepresentCommon.CellX2LogicX(nDestX);

                nDestY = m_Path.m_PointList[m_nCurPointIndex].nCellY;
                nDestY = RepresentCommon.CellY2LogicY(nDestY);
            }

            if (nDestX > m_nLogicX)
            {
                m_nLogicX += nSpeed;  
            }
            else if (nDestX < m_nLogicX)
            {
                m_nLogicX -= nSpeed;
            }

            if (nDestY > m_nLogicY)
            {
                m_nLogicY += nSpeed;
            }
            else if (nDestY < m_nLogicY)
            {
                m_nLogicY -= nSpeed;
            }

            float fWorldX = RepresentCommon.LogicX2WorldX(m_nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(m_nLogicY);
            m_RLNpc.SetPosition(fWorldX, fWorldY);
        }
    }
}
