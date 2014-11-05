using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GameWorld : Common.Singleton<GameWorld>
    {
        public GLScene m_Stage;

        public void Init()
        {
            GLSceneManager.Instance().Init();
            GLNpcManager.Instance().Init();

            // 创建关卡
            CreateStage(1);

        }

        public void UnInit()
        {
            GLNpcManager.Instance().UnInit();
            GLSceneManager.Instance().Init();
        }

        public void Activate()
        {
            //GLSceneManager.Instance().Activate();

            m_Stage.Activate();
        }

        public void Update()
        {

        }

        public void CreateStage(int nSceneId)
        {
            GLSceneConfig cfg = GLSceneManager.Instance().GetSceneConfig(nSceneId);

            m_Stage = new GLScene();
            m_Stage.Init(ref cfg);
            
            //GLScene scene = GLSceneManager.Instance().CreateScene(nSceneId);

        }
    }
}
