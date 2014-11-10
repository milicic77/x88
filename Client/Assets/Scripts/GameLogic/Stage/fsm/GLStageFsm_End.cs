using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameLogic
{
    public class FSMStateEnd : KFsmState
    {
        // 关卡
        private GLStage m_Stage;

        public FSMStateEnd(GLStage stage)
        {
            m_Stage = stage;
        }

        public override void Enter(object[] args = null)
        {
            Common.Console.Write("Enter FSMStateEnd");
            m_Stage.DoEnd();
            Common.Console.Write("游戏结束完成");
            // 退出关卡
        }

        public override void OnEvent(KFsmEvent evt)
        {
            Common.Console.Write("FSMStateEnd OnEvent");
        }

        public override void Update()
        {
            
        }
    }
}
