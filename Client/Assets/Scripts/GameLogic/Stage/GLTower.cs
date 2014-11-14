using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.Common;
using UnityEngine;

namespace Game.GameLogic
{
    class TowerAction_LockTarget : BTActionNode
    {
        public override void Activate()
        {
            GLTower tower = m_Owner.Blackboard.GetData("self") as GLTower;
            if (null == tower)
            { // 炮塔对象为空，查找失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            List<GLNpc> enemies = GameWorld.Instance().Stage.NpcList;
            if (enemies.Count <= 0)
            { // 敌人序列为空，查找失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            for (int i = 0; i < enemies.Count; i++)
            { // 查找射程范围中一个敌人
                GLNpc npc            = enemies[i];
                bool  bIsInFireRange = tower.IsInFireRange(npc);

                if (bIsInFireRange)
                {
                    tower.Target = npc;
                    m_State = BTTaskState.SUCCESS;
                    return;
                }
            }

            // 查找失败
            m_State = BTTaskState.FAILURE;
        }
    }

    class TowerAction_AimTarget : BTActionNode
    {
        public override void Activate()
        {
            GLTower tower = m_Owner.Blackboard.GetData("self") as GLTower;
            if (null == tower)
            { // 炮塔对象为空，瞄准失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            if (null == tower.Target)
            { // 攻击目标为空，瞄准失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            GLNpc target = tower.Target as GLNpc;
            List<GLNpc> enemies = GameWorld.Instance().Stage.NpcList;
            if (!enemies.Contains(target))
            { // 敌人已经死亡，则序列执行失败，重新寻找敌人
                tower.Target = null;
                m_State = BTTaskState.FAILURE;
                return;
            }

            // 对准目标
            Vector2 towerPos        = new Vector2(RepresentCommon.LogicX2WorldX(tower.LogicX),             RepresentCommon.LogicY2WorldY(tower.LogicY) );
            Vector2 targetPos       = new Vector2(RepresentCommon.LogicX2WorldX(target.GetLogicCenterX()), RepresentCommon.LogicY2WorldY(target.GetLogicCenterY()));
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

            // 瞄准成功
            if (Mathf.Abs(nRotationAngle) <= tower.AimDeviation)
            {
                m_State = BTTaskState.SUCCESS;
                return;
            }

            // 继续瞄准
            m_State = BTTaskState.RUNNING;
        }
    }

    class TowerAction_AttackTarget : BTActionNode
    {
        public override void Activate()
        {
            GLTower tower = m_Owner.Blackboard.GetData("self") as GLTower;
            if (null == tower)
            { // 炮塔对象为空，攻击失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            if (null == tower.Target)
            { // 敌人对象为空，攻击失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            GLNpc target = tower.Target as GLNpc;
            List<GLNpc> enemies = GameWorld.Instance().Stage.NpcList;
            if (!enemies.Contains(target))
            { // 敌人已经死亡，攻击失败，序列执行失败
                tower.Target = null;
                m_State = BTTaskState.FAILURE;
                return;
            }

            // 判断射程
            bool bIsInFireRange = tower.IsInFireRange(target);
            if (!bIsInFireRange)
            { // 超出射程，攻击失败，序列执行失败
                tower.Target = null;
                m_State = BTTaskState.FAILURE;
                return;
            }

            // 攻击成功
            tower.Attack();
            m_State = BTTaskState.SUCCESS;
        }
    }

    class TowerCondition_LockTarget : BTConditionNode
    {
        public override void Activate()
        {
            GLTower tower = m_Owner.Blackboard.GetData("self") as GLTower;
            if (null == tower)
            { // 炮塔对象为空，条件失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            if (null != tower.Target)
            { // 已经锁定目标，条件失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            // 未锁定目标，条件成功
            m_State = BTTaskState.SUCCESS;
        }
    }

    class TowerCondition_AttackTarget : BTConditionNode
    {
        public override void Activate()
        {
            GLTower tower = m_Owner.Blackboard.GetData("self") as GLTower;
            if (null == tower)
            { // 炮塔对象为空，条件失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            if (null == tower.Target)
            { // 还未锁定目标，条件失败
                m_State = BTTaskState.FAILURE;
                return;
            }

            // 已经锁定目标，条件成功
            m_State = BTTaskState.SUCCESS;
        }
    }

    public class GLTower
    {
        public  RLTower       m_RLTower         = null;                 // 表现炮塔
        private BehaviourTree m_TowerAI         = null;                 // 炮塔AI
        private int           m_nLogicX         = 0;                    // 逻辑坐标X
        private int           m_nLogicY         = 0;                    // 逻辑坐标Y
        private int           m_nAngle          = 0;                    // 炮塔当前角度
        private int           m_nFireRange      = 0;                    // 炮塔射程(像素)
        private int           m_nAngularSpeed   = 0;                    // 角速度
        private int           m_nAimDeviation   = 0;                    // 瞄准误差值
        private object        m_Target          = null;                 // 锁定目标
        private int           m_nAttackFreq     = 0;                    // 攻击频率(ms)
        private int           m_nLastAttackTime = 0;                    // 上一次攻击时间

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

        public int AimDeviation
        {
            get { return m_nAimDeviation; }
            set { m_nAimDeviation = value; }
        }

        public object Target
        {
            get { return m_Target;  }
            set { m_Target = value; m_RLTower.Target = m_Target; }
        }
        public int AttackFreq
        {
            get { return m_nAttackFreq; }
            set { m_nAttackFreq = value;}
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
            AimDeviation = 5;
            AttackFreq   = 20;

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

            // 锁定目标序列构建
            BTSequenceNode            lockTarget = new BTSequenceNode();
            TowerCondition_LockTarget lockCond   = new TowerCondition_LockTarget();
            TowerAction_LockTarget    lockAction = new TowerAction_LockTarget();

            lockTarget.Name      = "[锁定目标序列]";
            lockCond.Name        = "[锁定目标序列] - 条件节点";
            lockAction.Name      = "[锁定目标序列] - 动作节点";

            ai.AddNode(lockTarget);
            lockTarget.AddCond(lockCond);
            lockTarget.AddNode(lockAction);

            // 攻击目标序列构建
            BTSequenceNode              attackTarget = new BTSequenceNode();
            TowerCondition_AttackTarget attackCond   = new TowerCondition_AttackTarget();
            TowerAction_AimTarget       aimAction    = new TowerAction_AimTarget();
            TowerAction_AttackTarget    attackAction = new TowerAction_AttackTarget();

            attackTarget.Name      = "[攻击目标序列]";
            attackCond.Name        = "[攻击目标序列] - 条件节点";
            aimAction.Name         = "[攻击目标序列] - 瞄准目标动作节点";
            attackAction.Name      = "[攻击目标序列] - 攻击目标动作节点";

            ai.AddNode(attackTarget);
            attackTarget.AddCond(attackCond);
            attackTarget.AddNode(aimAction);
            attackTarget.AddNode(attackAction);
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

        public bool IsIntersected(GLCircle circle, GLRectangle rect)
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

        public void Attack()
        {
            int nCurTime = (int)Time.time * 1000;
            if (nCurTime - m_nLastAttackTime <= m_nAttackFreq)
            { // 攻击间隔未到，不能攻击
                return;
            }
            m_nLastAttackTime = nCurTime;
            m_RLTower.PlayAttackAnimation();

            Vector2   vecTowerTube = new Vector2(Mathf.Sin(Angle * Mathf.Deg2Rad), Mathf.Cos(Angle * Mathf.Deg2Rad));
            Vector2   towerPos     = new Vector2(RepresentCommon.LogicX2WorldX(LogicX), RepresentCommon.LogicY2WorldY(LogicY));
            GLMissile missile      = new GLMissile();
            GameWorld.Instance().m_stage.m_GLMissileList.Add(missile);

            missile.Init(9, vecTowerTube, towerPos, m_Target as GLNpc);
        }
    }
}
