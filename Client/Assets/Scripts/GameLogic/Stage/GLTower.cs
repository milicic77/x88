using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using Game.Common;
using UnityEngine;

namespace Game.GameLogic
{
    class GLTowerFindTargetAction : BTActionNode
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
                    Game.Common.Console.Write("锁定一个npc");
                    return;
                }
            }

            m_State = BTTaskState.FAILURE;
            return;
        }
    }

    class GLTowerAimTargetAction : BTActionNode
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
            GLNpc npc = tower.Target as GLNpc;
            Vector2 vNpc   = new Vector2(RepresentCommon.LogicX2WorldX(npc.LogicX),   RepresentCommon.LogicY2WorldY(npc.LogicY));
            Vector2 vTower = new Vector2(RepresentCommon.LogicX2WorldX(tower.LogicX), RepresentCommon.LogicY2WorldY(tower.LogicY));

            float angle = Vector2.Angle(vTower, vNpc);
            Debug.Log(angle);

            int nAngle = tower.Angle;
            nAngle = (nAngle + 200) % 360;
            tower.Angle = nAngle;
            m_State = BTTaskState.RUNNING;
            return;
        }
    }
    public class GLTower
    {
        public  RLTower       m_RLTower    = null;                      // 表现炮塔
        private BehaviourTree m_TowerAI    = null;                      // 炮塔AI
        private int           m_nLogicX    = 0;                         // 逻辑坐标X
        private int           m_nLogicY    = 0;                         // 逻辑坐标Y
        private int           m_nAngle     = 0;                         // 炮塔当前角度
        private int           m_nLastAngle = 0;                         // 炮塔上次角度
        private int           m_nFireRange = 300;                       // 炮塔射程
        private object        m_Target     = null;                      // 锁定目标

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

            GLTowerFindTargetAction lockAction = new GLTowerFindTargetAction();
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

            GLTowerAimTargetAction aimAction = new GLTowerAimTargetAction();
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
                return false;
            return true;
        }

        public bool AttackTargetCondition(object arg)
        {
            if (null != m_Target)
                return true;
            return false;
        }

        public object Target
        {
            get { return m_Target;  }
            set { m_Target = value; }
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

        public int FireRange
        {
            get { return m_nFireRange; }
            set { m_nFireRange = value; }
        }
        public int Angle
        {
            get { return m_nAngle;  }
            set
            {
                m_nLastAngle    = m_nAngle;
                m_nAngle        = value;
                m_RLTower.Angle = m_nAngle;
                m_RLTower.LastAngle = m_nLastAngle;
            }
        }

        public void UnInit()
        {

        }

        public void Activate()
        {
            m_TowerAI.Activate();
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
