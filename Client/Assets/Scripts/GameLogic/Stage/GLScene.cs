using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;
using UnityEngine;

namespace Game.GameLogic
{
    // 关卡中的逻辑场景
    public class GLScene
    {
        

        //// 关卡中路径集合 Key从1开始
        //private Dictionary<int, GLPath> m_PathList = new Dictionary<int, GLPath>();

        // 表现逻辑场景
        public RLScene m_RLScene;

        public void Init(int nSceneId)
        {
            GLSceneTemplate template = GLSettingManager.Instance().GetGLSceneTemplate(nSceneId);

            // 创建游戏场景
            m_RLScene = Represent.Instance().CreateScene(template.nRepresentId);
        }

        public void UnInit()
        {

        }

        public void AddDoodad(GLDoodad doodad)
        {
            m_RLScene.AddDoodad(doodad.m_RLDoodad);
        }

        public void AddNpc(GLNpc npc)
        {
            m_RLScene.AddNpc(npc.m_RLNpc);
        }

        public void AddRadish(GLRadish radish)
        {
            m_RLScene.AddRadish(radish.m_RLRadish);
        }

        public void AddEffect(GLEffect effect)
        {
            m_RLScene.AddEffect(effect.m_RLEffect);
        }

        public void AddTower(GLTower tower)
        {
            m_RLScene.AddTower(tower.m_RLTower);
        }

        public void Activate()
        {

        }
    }
}
