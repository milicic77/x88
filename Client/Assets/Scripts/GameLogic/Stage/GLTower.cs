﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.Common;
using UnityEngine;

namespace Game.GameLogic
{
    class TowerAction_FindTarget : BTActionNode
    {
        public override void Activate()
        {
            GLTower tower = m_Owner.Blackboard.GetData("self") as GLTower;
            if (null == tower)
            {
                m_State = BTTaskState.FAILURE;
                return;
            }

            List<GLNpc> enemies = GameWorld.Instance().Stage.NpcList;
            if (enemies.Count <= 0)
            {
                m_State = BTTaskState.FAILURE;
                return;
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                GLNpc npc = enemies[i];
                double distance = Math.Sqrt(Math.Pow(tower.LogicX - npc.LogicX, 2) + Math.Pow(tower.LogicY - npc.LogicY, 2));
                if (distance <= tower.FireRange)
                {
                    tower.Target = npc;
                    m_State = BTTaskState.SUCCESS;
                    return;
                }
            }

            m_State = BTTaskState.FAILURE;
        }
    }

    class TowerAction_AimTarget : BTActionNode
    {
        public override void Activate()
        {
            GLTower tower = m_Owner.Blackboard.GetData("self") as GLTower;
            if (null == tower)
            {
                m_State = BTTaskState.FAILURE;
                return;
            }

            if (null == tower.Target)
            {
                m_State = BTTaskState.FAILURE;
                return;
            }

            // 对准目标
            GLNpc  target           = tower.Target as GLNpc;
            Vector2 towerPos        = new Vector2(RepresentCommon.LogicX2WorldX(tower.LogicX),  RepresentCommon.LogicY2WorldY(tower.LogicY) );
            Vector2 targetPos       = new Vector2(RepresentCommon.LogicX2WorldX(target.LogicX), RepresentCommon.LogicY2WorldY(target.LogicY));
            Vector2 vecTower2Target = targetPos - towerPos;                         // 炮塔到目标的方向向量
            float   angTower2Target = Vector2.Angle(vecTower2Target, Vector2.up);   // 炮塔到目标的方向向量和Y轴的夹角

            if (vecTower2Target.x < 0)
            { // 转换成顺时针夹角
                angTower2Target = 360 - angTower2Target;
            }

            // 旋转方向
            bool bRotateDirection = false;                                          // fase - 逆时针，true - 顺时针
            if (tower.Angle >= 0 && tower.Angle <= 180)
            {
                if (angTower2Target >= tower.Angle && angTower2Target <= 180 + tower.Angle)
                { // 顺时针
                    bRotateDirection = true;
                }
                else
                { // 逆时针
                    bRotateDirection = false;
                }
            }
            else if (tower.Angle > 180 && tower.Angle <= 360)
            {
                if (angTower2Target >= tower.Angle - 180 && angTower2Target <= tower.Angle)
                { // 逆时针
                    bRotateDirection = false;
                }
                else
                { // 顺时针
                    bRotateDirection = true;
                }
            }

            // 取旋转值
            int nTowerAngle    = 0;                                                 // 炮塔最终角度
            int nRotationAngle = tower.AngularSpeed;                                // 炮塔旋转角度

            // 根据炮管的方向向量A和炮塔到目标的向量B，求出A和B之间的夹角
            Vector2 vecTowerTube = new Vector2(Mathf.Sin(tower.Angle * Mathf.Deg2Rad), Mathf.Cos(tower.Angle * Mathf.Deg2Rad));
            float   fAngleDiff   = Vector2.Angle(vecTower2Target, vecTowerTube);
            int     nAngleDiff   = (int)Mathf.Abs(fAngleDiff);

            if (nRotationAngle > nAngleDiff)
            { // 旋转角度大于夹角，说明只需要旋转夹角大小，否则旋转tower.AngularSpeed大小
                nRotationAngle = nAngleDiff;
            }

            if (!bRotateDirection)
            { // 如果是逆时针，那么值为负
                nRotationAngle = -nRotationAngle;
            }

            // 设置RL层的旋转角度
            tower.RLTower.RotationAngle = nRotationAngle;

            // 设置GL层的目标角度
            nTowerAngle = tower.Angle + nRotationAngle;
            if (nTowerAngle < 0)
            { // 逆时针角度换算成顺时针角度
                nTowerAngle = 360 + nTowerAngle;
            }

            if (nTowerAngle >= 360)
            { // 超过360度处理
                nTowerAngle= nTowerAngle - 360;
            }

            tower.Angle = nTowerAngle;
            m_State = BTTaskState.RUNNING;
        }
    }
    public class GLTower
    {
        public  RLTower       m_RLTower       = null;                      // 表现炮塔
        private BehaviourTree m_TowerAI       = null;                      // 炮塔AI
        private int           m_nLogicX       = 0;                         // 逻辑坐标X
        private int           m_nLogicY       = 0;                         // 逻辑坐标Y
        private int           m_nAngle        = 0;                         // 炮塔当前角度
        private int           m_nFireRange    = 0;                         // 炮塔射程(像素)
        private int           m_nAngularSpeed = 0;
        private object        m_Target        = null;                      // 锁定目标

        public RLTower RLTower
        {
            get { return m_RLTower; }
        }
        public int LogicX
        {
            get { return m_nLogicX;  }
            set { m_nLogicX = value; }
        }
        public int LogicY
        {
            get { return m_nLogicY;  }
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
            get { return m_nAngularSpeed;  }
            set { m_nAngularSpeed = value; }
        }
        public object Target
        {
            get { return m_Target;  }
            set { m_Target = value; }
        }

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
            m_nLogicY    = nLogicY;
            FireRange    = 200;
            AngularSpeed = 10;

            // 初始化AI
            InitAI();
        }

        public void InitAI()
        {
            m_TowerAI  = new BehaviourTree();
            BTNonPrioritySelector ai = new BTNonPrioritySelector();

            ai.Owner = m_TowerAI;
            ai.Name  = "炮塔AI";
            m_TowerAI.RootNode = ai;
            m_TowerAI.Blackboard.AddData("self", this);

            // 锁定目标
            BTCondition lockCond = new BTCondition();
            lockCond.Name = "选择目标条件";
            lockCond.CondHandler = LockTargetCondition;

            TowerAction_FindTarget lockAction = new TowerAction_FindTarget();
            lockAction.Name = "选择目标动作";

            BTSequenceNode lockTarget = new BTSequenceNode();
            ai.AddNode(lockTarget);

            lockTarget.Name = "选择目标";
            lockTarget.AddCond(lockCond);
            lockTarget.AddNode(lockAction);

            // 攻击目标
            BTCondition attackCond = new BTCondition();
            attackCond.Name = "攻击目标条件";
            attackCond.CondHandler = AttackTargetCondition;

            TowerAction_AimTarget aimAction = new TowerAction_AimTarget();
            lockAction.Name = "锁定目标动作";

            BTSequenceNode attackTarget = new BTSequenceNode();
            ai.AddNode(attackTarget);

            attackTarget.Name = "锁定目标";
            attackTarget.AddCond(attackCond);
            attackTarget.AddNode(aimAction);
        }

        public bool LockTargetCondition(object arg)
        {
            if (null != m_Target)
            {
                return false;
            }

            return true;
        }

        public bool AttackTargetCondition(object arg)
        {
            if (null != m_Target)
            {
                return true;
            }

            return false;
        }

        public void UnInit()
        {
        }

        public void Activate()
        {
            m_TowerAI.Activate();
        }
    }
}
