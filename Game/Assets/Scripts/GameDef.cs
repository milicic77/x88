using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class GameDef
    {
        public const int GAME_FPS = 16;
    }

    class GameEnv
    {
        // 游戏当前已经运行的帧数
        public static uint CurrentLogicFrame = 0;
        // 游戏开始运行时的帧数（可以在真正进入游戏场景的时候设置此值）
        public static uint StartLogicFrame = 0;
        // 游戏开始运行时的TickCount（可以在真正进入游戏场景的时候设置此值）
        public static uint LogicStartTickCount = 0;
        // 上一次刷新FPS时的TickCount
        public static uint LastShowFPSTickCount = 0;
        // 上一次刷新FPS时已运行的逻辑帧数
        public static uint LastShowFPSLogicFrame = 0;
    }
}
