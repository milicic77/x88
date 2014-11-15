using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

using Game.Common;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public interface IGLTowerAITemp
    {
        BehaviourTree Create(object self);
    }

    class GLTowerAITemp_1 : IGLTowerAITemp
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
                    GLNpc npc = enemies[i];
                    bool bIsInFireRange = tower.IsInFireRange(npc);

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
                Vector2 towerPos = new Vector2(RepresentCommon.LogicX2WorldX(tower.LogicX), RepresentCommon.LogicY2WorldY(tower.LogicY));
                Vector2 targetPos = new Vector2(RepresentCommon.LogicX2WorldX(target.GetLogicCenterX()), RepresentCommon.LogicY2WorldY(target.GetLogicCenterY()));
                Vector2 vecTower2Target = targetPos - towerPos;                         // 炮塔到目标的方向向量
                float angTower2Target = Vector2.Angle(vecTower2Target, Vector2.up);   // 炮塔到目标的方向向量和Y轴的夹角

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
                int nTowerAngle = 0;                                                 // 炮塔最终角度
                int nRotationAngle = tower.AngularSpeed;                                // 炮塔旋转角度

                // 根据炮管的方向向量A和炮塔到目标的向量B，求出A和B之间的夹角
                Vector2 vecTowerTube = new Vector2(Mathf.Sin(tower.Angle * Mathf.Deg2Rad), Mathf.Cos(tower.Angle * Mathf.Deg2Rad));
                float fAngleDiff = Vector2.Angle(vecTower2Target, vecTowerTube);
                int nAngleDiff = (int)Mathf.Abs(fAngleDiff);

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
                    nTowerAngle = nTowerAngle - 360;
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
                Vector2 bulletDirection = new Vector2(Mathf.Sin(tower.Angle * Mathf.Deg2Rad), Mathf.Cos(tower.Angle * Mathf.Deg2Rad));
                Vector2 bulletPosition = new Vector2(RepresentCommon.LogicX2WorldX(tower.LogicX), RepresentCommon.LogicY2WorldY(tower.LogicY));
                tower.Attack(bulletDirection, bulletPosition);
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

        public BehaviourTree Create(object self)
        {
            BehaviourTree m_TowerAI  = new BehaviourTree();
            BTNonPrioritySelector ai = new BTNonPrioritySelector();

            ai.Owner = m_TowerAI;
            ai.Name = "炮塔AI";

            m_TowerAI.RootNode = ai;
            m_TowerAI.Blackboard.AddData("self", self);

            // 锁定目标序列构建
            BTSequenceNode lockTarget = new BTSequenceNode();
            TowerCondition_LockTarget lockCond = new TowerCondition_LockTarget();
            TowerAction_LockTarget lockAction = new TowerAction_LockTarget();

            lockTarget.Name = "[锁定目标序列]";
            lockCond.Name = "[锁定目标序列] - 条件节点";
            lockAction.Name = "[锁定目标序列] - 动作节点";

            ai.AddNode(lockTarget);
            lockTarget.AddCond(lockCond);
            lockTarget.AddNode(lockAction);

            // 攻击目标序列构建
            BTSequenceNode attackTarget = new BTSequenceNode();
            TowerCondition_AttackTarget attackCond = new TowerCondition_AttackTarget();
            TowerAction_AimTarget aimAction = new TowerAction_AimTarget();
            TowerAction_AttackTarget attackAction = new TowerAction_AttackTarget();

            attackTarget.Name = "[攻击目标序列]";
            attackCond.Name = "[攻击目标序列] - 条件节点";
            aimAction.Name = "[攻击目标序列] - 瞄准目标动作节点";
            attackAction.Name = "[攻击目标序列] - 攻击目标动作节点";

            ai.AddNode(attackTarget);
            attackTarget.AddCond(attackCond);
            attackTarget.AddNode(aimAction);
            attackTarget.AddNode(attackAction);

            return m_TowerAI;
        }
    }

    public class GLTowerAI
    {
        private static Dictionary<int, IGLTowerAITemp> m_TowerAITempDict = new Dictionary<int, IGLTowerAITemp>()
        {
            {1, new GLTowerAITemp_1()},
        };

        public static BehaviourTree Create(int nAITempId, object self)
        {
            if (!m_TowerAITempDict.ContainsKey(nAITempId))
            {
                return null;
            }

            return m_TowerAITempDict[nAITempId].Create(self);
        }
    }
}
