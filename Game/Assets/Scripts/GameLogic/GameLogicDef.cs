using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameLogic
{
    public class GLSceneConfig
    {
        public int nSceneId = 0;
        public string szName = null;
        public int nTemplateId = 0;
    }

    class GameLogicDef
    {
        // 表现场景配置文件
        public const string SCENE_LIST_FILE = "setting/scene/scenelist";

        // NPC配置文件
        public const string NPC_TEMPLATE_LIST_FILE = "setting/npc/npc";
    }
}
