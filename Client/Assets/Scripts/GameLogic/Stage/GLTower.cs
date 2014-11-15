using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

using Game.RepresentLogic;
using Game.Common;

namespace Game.GameLogic
{
    public class GLTower
    {
        public  RLTower       m_RLTower         = null;                 // 表现炮塔
        private BehaviourTree m_TowerAI         = null;                 // 炮塔AI
        private int           m_nBulletTempId   = 0;                    // 子弹模板Id
        private int           m_nLogicX         = 0;                    // 逻辑坐标X
        private int           m_nLogicY         = 0;                    // 逻辑坐标Y
        private int           m_nAngle          = 0;                    // 炮塔当前角度
        private int           m_nFireRange      = 0;                    // 炮塔射程(像素)
        private int           m_nAngularSpeed   = 0;                    // 角速度
        private int           m_nAimDeviation   = 0;                    // 瞄准误差值
        private object        m_Target          = null;                 // 锁定目标
        private int           m_nAttackFreq     = 0;                    // 攻击频率(ms)
        private int           m_nLastAttackTime = 0;                    // 上一次攻击时间

        public void Init(int nTemplateId, int nCellX, int nCellY, GLScene scene)
        {
            GLTowerTemplate t = GLSettingManager.Instance().GetGLTowerTemplate(nTemplateId);

            // 格子坐标 => 逻辑坐标
            int nLogicX = RepresentCommon.CellX2LogicX(nCellX);
            int nLogicY = RepresentCommon.CellY2LogicY(nCellY);

            // 逻辑坐标 => 世界坐标
            float fWorldX = RepresentCommon.LogicX2WorldX(nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(nLogicY);

            m_RLTower = Represent.Instance().CreateTower(t.nRepresentId, fWorldX, fWorldY);

            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;

            // 初始化值
            LogicX       = nLogicX;
            LogicY       = nLogicY;
            BulletTempId = t.nBulletTempId;
            FireRange    = 200;
            AngularSpeed = 10;
            AimDeviation = 5;
            AttackFreq   = 20;

            // 初始化AI
            m_TowerAI = GLTowerAI.Create(1, this);
        }

        public void UnInit()
        {
        }

        public void Activate()
        {
            m_TowerAI.Activate();
        }

        public bool IsInFireRange(GLNpc npc)
        {
            if (null == npc)
            {
                return false;
            }

            int nNpcLogicCenterX = npc.GetLogicCenterX();
            int nNpcLogicCenterY = npc.GetLogicCenterY();

            GLCircle circle = new GLCircle();
            circle.x = m_nLogicX;
            circle.y = m_nLogicY;
            circle.r = m_nFireRange;

            GLRectangle rect = new GLRectangle();
            rect.x = nNpcLogicCenterX;
            rect.y = nNpcLogicCenterY;
            rect.w = npc.Width;
            rect.h = npc.Height;

            return IsIntersected(circle, rect);
        }

        private bool IsIntersected(GLCircle circle, GLRectangle rect)
        {
            float fDistanceX = Mathf.Abs(circle.x - rect.x);
            float fDistanceY = Mathf.Abs(circle.y - rect.y);

            if (fDistanceX > (rect.w / 2 + circle.r))
            {
                return false;
            }

            if (fDistanceY > (rect.h / 2 + circle.r))
            {
                return false;
            }

            if (fDistanceX <= (rect.w / 2))
            {
                return true;
            }

            if (fDistanceY <= (rect.h / 2))
            {
                return true;
            }

            float fCornerDistanceSq = Mathf.Pow(fDistanceX - rect.w / 2, 2) + Mathf.Pow(fDistanceY - rect.h / 2, 2);
            return fCornerDistanceSq <= Mathf.Pow(circle.r, 2);
        }

        public void Attack(Vector2 bulletDirection, Vector2 bulletPosition)
        {
            int nCurTime = (int)Time.time * 1000;
            if (nCurTime - m_nLastAttackTime <= m_nAttackFreq)
            { // 攻击间隔未到，不能攻击
                return;
            }
            m_nLastAttackTime = nCurTime;
            m_RLTower.PlayAttackAnimation();

            GLMissile missile      = new GLMissile();
            GameWorld.Instance().m_stage.m_GLMissileList.Add(missile);

            missile.Init(BulletTempId, bulletDirection, bulletPosition, m_Target as GLNpc);
        }

        public RLTower RLTower
        {
            get { return m_RLTower; }
        }
        public int BulletTempId
        {
            get { return m_nBulletTempId; }
            set { m_nBulletTempId = value; }
        }
        public int LogicX
        {
            get { return m_nLogicX; }
            set { m_nLogicX = value; }
        }
        public int LogicY
        {
            get { return m_nLogicY; }
            set { m_nLogicY = value; }
        }
        public int Angle
        {
            get { return m_nAngle; }
            set { m_nAngle = value; }
        }
        public int FireRange
        {
            get { return m_nFireRange; }
            set { m_nFireRange = value; m_RLTower.FireRange = m_nFireRange; }
        }
        public int AngularSpeed
        {
            get { return m_nAngularSpeed; }
            set { m_nAngularSpeed = value; }
        }
        public int AimDeviation
        {
            get { return m_nAimDeviation; }
            set { m_nAimDeviation = value; }
        }
        public object Target
        {
            get { return m_Target; }
            set { m_Target = value; m_RLTower.Target = m_Target; }
        }
        public int AttackFreq
        {
            get { return m_nAttackFreq; }
            set { m_nAttackFreq = value; }
        }
    }
}
