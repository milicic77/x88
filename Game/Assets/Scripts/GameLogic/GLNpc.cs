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

    }
}
