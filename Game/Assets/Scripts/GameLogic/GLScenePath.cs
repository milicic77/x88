using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameLogic
{
    // 关卡中的逻辑坐标
    public class GLScenePoint
    {
        public int nX = 0;
        public int nY = 0;
    }

    // 关卡中怪物行走路径
    public class GLScenePath
    {
        public GLScenePoint m_Start = null;
        public GLScenePoint m_End = null;

        public List<GLScenePoint> m_PointList = new List<GLScenePoint>();
    }
}
