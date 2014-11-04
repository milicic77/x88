using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Game.Common;
using Game.GameLogic;
using Game.RepresentLogic;

namespace Game
{
    public class GameClient
    {
        public static Represent m_Represent = new Represent();
        public static GameWorld m_GameWorld = new GameWorld();
        private bool m_ClientInitialized = false;

        public void Init()
        {
            bool success = true;

            try
            {
                //////////////////////////////////////////////////////////////////////////
                LogWriter.Write("Initializing Client...");

                //////////////////////////////////////////////////////////////////////////
                // 初始化表现逻辑
                LogWriter.Write("Initializing Represent...");
                m_Represent.Init();

                //////////////////////////////////////////////////////////////////////////
                // 初始化游戏逻辑
                LogWriter.Write("Initializing GameWorld...");
                m_GameWorld.Init();

                //////////////////////////////////////////////////////////////////////////
                // 记录启动游戏时的TickCount
                Game.GameEnv.LogicStartTickCount = (uint)Environment.TickCount;

                m_ClientInitialized = true;
            }
            catch (Exception e)
            {
                success = false;
                ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    UnInit();
                }
            }
        }

        public void UnInit()
        {
            m_GameWorld.UnInit();
            m_Represent.UnInit();

            m_ClientInitialized = false;
        }

        public void Loop()
        {
            if (!m_ClientInitialized)
                return;

            //////////////////////////////////////////////////////////////////////////
            // 处理逻辑帧
            while (CanActive())
            {
                Activate();
                Game.GameEnv.CurrentLogicFrame++;
            }
            //////////////////////////////////////////////////////////////////////////
            // 处理绘制帧
            m_Represent.Update();

            // 显示FPS
            ShowFPS();
        }

        private bool CanActive()
        {
            // 从启动游戏到现在实际已经走过的逻辑帧数*1000
            UInt64 nLogicFrameTime = (UInt64)(Game.GameEnv.CurrentLogicFrame - Game.GameEnv.StartLogicFrame) * 1000;
            // 从启动游戏到现在经过的TickCount
            UInt64 nTickCountDiff = (UInt64)(Environment.TickCount - Game.GameEnv.LogicStartTickCount);
            // 从启动游戏到现在应该走过的帧数*1000
            UInt64 nLogicFlowTime = nTickCountDiff * Game.GameDef.GAME_FPS;

            if (nLogicFrameTime < nLogicFlowTime)
            {
                return true;
            }

            return false;
        }

        private void Activate()
        {
            m_GameWorld.Activate();
            m_Represent.Activate();
        }

        public void ShowFPS()
        {
            UInt64 nTickCountDiff = (UInt64)(Environment.TickCount - Game.GameEnv.LastShowFPSTickCount);
            if (nTickCountDiff >= 1000) // 一秒刷新一次FPS
            {
                uint nFPS = Game.GameEnv.CurrentLogicFrame - Game.GameEnv.LastShowFPSLogicFrame;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("当前FPS={0}\n", nFPS);
                Debug.Log(sb.ToString());

                Game.GameEnv.LastShowFPSTickCount = (uint)Environment.TickCount;
                Game.GameEnv.LastShowFPSLogicFrame = Game.GameEnv.CurrentLogicFrame;
            }
        }
    }
}
