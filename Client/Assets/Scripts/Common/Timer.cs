using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Timers;

namespace Game.Common
{
    enum GameTimerType
    {
        TIME,
        FRAME
    }
    public delegate void GameTimerDelegate(object param);

    // 用于执行
    public class GameTimerComponent : MonoBehaviour
    {
        public GameTimerDelegate TheDelegate;
        public GameTimer TheTimer;

        // 强制立即执行，并删除自己Timer!
        public void RunAndDelete()
        {
            TheDelegate(null);
            Destroy(this.gameObject);
        }

        void Update()
        {
            if (!TheTimer.IsRunning())
            {
                TheDelegate(null);

                // 结束这个TimerComponent
                Destroy(this.gameObject);
            }
        }

        public static GameTimerComponent CreateTimer(float seconds, GameTimerDelegate del)
        {
            GameObject timerObj = new GameObject("QTimer");
            timerObj.transform.parent = Game.RepresentLogic.RepresentEnv.GameRoot.transform;
            GameTimerComponent timerCom = timerObj.AddComponent<GameTimerComponent>();

            timerCom.TheTimer = new GameTimer(seconds);
            timerCom.TheDelegate = del;
            GameObject.DontDestroyOnLoad(timerObj);

            return timerCom;
        }

        public static void CreateTimer(int frames, GameTimerDelegate del)
        {
            GameObject timerObj = new GameObject("QTimer");
            GameTimerComponent timerCom = timerObj.AddComponent<GameTimerComponent>();

            timerCom.TheTimer = new GameTimer(frames);
            timerCom.TheDelegate = del;
        }
    }

    // 普通计时
    public class GameTimer
    {
        GameTimerType m_type;

        // 计秒
        float m_StartTime;
        float m_WaitSeconds;

        // 计帧
        int m_StartFrame;
        int m_WaitFrames;

        public GameTimer(float waitSeconds)
        {
            m_type = GameTimerType.TIME;
            m_StartTime = Time.time;
            m_WaitSeconds = waitSeconds;
        }

        // 时间重新来过
        public void Reset()
        {
            if (m_type == GameTimerType.TIME)
            {
                m_StartTime = Time.time;
            }
            else
            {
                m_StartFrame = (int)Game.GameEnv.CurrentLogicFrame;
            }
        }


        public GameTimer(int waitFrames)
        {
            m_type = GameTimerType.FRAME;
            m_StartFrame = (int)Game.GameEnv.CurrentLogicFrame;
            m_WaitFrames = waitFrames;
        }

        private GameTimer() { }

        // 判断计时器结束没有
        public bool IsRunning()
        {
            if (m_type == GameTimerType.FRAME)
            {
                return ((int)Game.GameEnv.CurrentLogicFrame - m_StartFrame < m_WaitFrames);
            }
            else if (m_type == GameTimerType.TIME)
            {
                return (Time.time - m_StartTime < m_WaitSeconds);
            }
            else
            {
                return false;  // Error, maybe impoosible
            }
        }

        // 获取时间,  (不包括帧)
        public float GetTime()
        {
            if (m_type == GameTimerType.TIME)
            {
                return Time.time - m_StartTime;
            }

            Debug.LogError("Error! GetTime for Time Type Timer");
            return 0;
        }

        // 定时执行函数方法
        public static GameTimerComponent CreateTimerFunction(float seconds, GameTimerDelegate del)
        {
            return Common.GameTimerComponent.CreateTimer(seconds, del);
        }
    }
}
