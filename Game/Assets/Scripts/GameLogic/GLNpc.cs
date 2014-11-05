using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GLNpc
    {
        // 对应的表现逻辑中的场景对象
        private RLSceneObject m_RLSceneObject;

        // NPC在游戏中的逻辑ID
        private int m_nId;

        // NPC所属场景
        private GLScene m_GLScene;

        private int m_nLogicX = 0;
        private int m_nLogicY = 0;

        private int m_nDoing = (int)SceneObjectAni.SceneObjectAni_Stand;
        private int m_nDirection = (int)SceneObjectDirection.SceneObjectDirection_Right;

        // 行走路径
        private GLScenePath m_Path;
        private int m_nPathIndex = 0;

        private int nTime1 = Environment.TickCount;

        public void Init(int nTemplateId, GLScene scene)
        {
            GLNpcTemplate template = GLNpcManager.Instance().GetNpcTemplate(nTemplateId);
            int nRepresentId = template.nRepresentId;

            m_GLScene = scene;

            m_RLSceneObject = m_GLScene.RepresentScene.AddSceneObject(nRepresentId);
        }

        public void UnInit()
        {

        }

        public void Activate()
        {
            int nNow = Environment.TickCount;
            if (nNow - nTime1 >= 500)
            {
                GLScenePoint point = m_Path.m_PointList[m_nPathIndex];
                SetPosition(point.nX, point.nY);
                m_nPathIndex++;
                m_nPathIndex = m_nPathIndex % m_Path.m_PointList.Count();
                nTime1 = nNow;
            }
            
        }

        public void SetPosition(int nLogicX, int nLogicY)
        {
            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;

            m_RLSceneObject.SetPosition(nLogicX, nLogicY);
        }

        public void SetDoing(int nDoing)
        {
            m_nDoing = nDoing;

            m_RLSceneObject.DOING = nDoing;
        }

        public void SetDirection(int nDirection)
        {
            m_nDirection = nDirection;

            m_RLSceneObject.DIRECTION = nDirection;
        }

        public void SetPath(GLScenePath path)
        {
            m_Path = path;
            m_nPathIndex = 0;
        }

    }
}
