using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Game.Common;
using Game.GameLogic;
using Game.RepresentLogic;
using Game.GameEvent;

namespace Game
{
    public class GameClient
    {
        private bool m_ClientInitialized = false;
        private uint m_LastShowFpsFrames = 0;
        private float m_FpsUpdateTime = 0;

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
                Represent.Instance().Init();

                //////////////////////////////////////////////////////////////////////////
                // 初始化游戏逻辑
                LogWriter.Write("Initializing GameWorld...");
                GameWorld.Instance().Init();

                m_ClientInitialized = true;
                GameEnv.LogicStartTime = Time.time;
                
                EventCenter.Event_ClientInitComplete(null, null);
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
            //GameWorld.Instance().UnInit();
            Represent.Instance().UnInit();

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
                GameEnv.CurrentLogicFrame++;
            }
            //////////////////////////////////////////////////////////////////////////
            // 处理绘制帧
            Represent.Instance().Update();

            // 显示FPS
            //ShowFPS();
        }

        private bool CanActive()
        {
            float fExpectedFrames = (Time.time - GameEnv.LogicStartTime) * GameDef.GAME_FPS;
            float fActualFrames = GameEnv.CurrentLogicFrame - GameEnv.StartLogicFrame;
            if (fExpectedFrames >= fActualFrames)
            {
                return true;
            }
            return false;
        }

        private void Activate()
        {
            GameWorld.Instance().Activate();
            Represent.Instance().Activate();
        }

        public void ShowFPS()
        {
            m_FpsUpdateTime += Time.deltaTime;
            if (1 - m_FpsUpdateTime < Time.fixedDeltaTime || m_FpsUpdateTime >= 1)
            {
                m_FpsUpdateTime = 0;
                UInt32 nFrames = GameEnv.CurrentLogicFrame - m_LastShowFpsFrames;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("当前FPS={0}\n", nFrames);
                Debug.Log(sb.ToString());
                m_LastShowFpsFrames = GameEnv.CurrentLogicFrame;
            }
        }
    }
}
