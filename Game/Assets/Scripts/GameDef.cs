using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    class GameDef
    {
        // 逻辑帧数
        public const int GAME_FPS = 16;
    }

    class GameEnv
    {
        // 游戏当前已经运行的帧数
        public static uint CurrentLogicFrame = 0;
        // 游戏开始运行时的帧数（可以在真正进入游戏场景的时候设置此值）
        public static uint StartLogicFrame = 0;
        // 游戏开始的时间（完成初始化时）
        public static float LogicStartTime = 0;
    }
}
