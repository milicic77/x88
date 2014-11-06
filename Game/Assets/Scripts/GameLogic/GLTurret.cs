using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GLTurret
    {
        private int m_nId        = 0;                                                       // NPC逻辑Id
        private int m_nLogicX    = 0;                                                       // 逻辑坐标X
        private int m_nLogicY    = 0;                                                       // 逻辑坐标Y
        private int m_nDoing     = (int)SceneObjectAni.SceneObjectAni_Stand;                // NPC动作
        private int m_nDirection = (int)SceneObjectDirection.SceneObjectDirection_Right;    // NPC方向

        private GLScene       m_GLScene;                                                    // NPC所属场景
        private RLSceneObject m_RLSceneObject;                                              // 对应表现逻辑中的场景对象

        public void Init(int nTemplateId, GLScene scene)
        {
            GLNpcTemplate t  = GLNpcManager.Instance().GetNpcTemplate(nTemplateId);
            int nRepresentId = t.nRepresentId;
            m_GLScene        = scene;
            m_RLSceneObject  = m_GLScene.RepresentScene.AddSceneObject(nRepresentId);
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
            m_nDoing              = nDoing;
            m_RLSceneObject.DOING = nDoing;
        }

        public void SetDirection(int nDirection)
        {
            m_nDirection              = nDirection;
            m_RLSceneObject.DIRECTION = nDirection;
        }
    }
}
