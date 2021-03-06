﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    //////////////////////////////////////////////////////////////////////////
    // 场景格子类型
    public enum GLSceneCellType
    {
        GLSceneCellType_Idel = 0, // 空闲
        GLSceneCellType_Obstacle = 1, // 障碍
    }
    //////////////////////////////////////////////////////////////////////////
    // 关卡模板
    public class GLStageTemplate
    {
        // 模板ID
        public int nTemplateId = 0;
        // 名字
        public string szName = null;
        // 逻辑场景模板ID
        public int nSceneId = 0;
        // 每两组Npc创建的间隔（毫秒）
        public int nNpcGroupInterval;
        // 场景中的物件配置
        public List<GLDoodadPos> asDoodad = new List<GLDoodadPos>();
        // 场景中的炮塔配置
        public List<GLTowerPos> asTower = new List<GLTowerPos>();
        // Npc配置（有多少波，每波有多少个，都使用哪个模板，走哪条路径）
        public List<GLStageNpcConfig> asNpc = new List<GLStageNpcConfig>();
        // 萝卜模板
        public int nRadishTemplateId = 0;
        // 萝卜CellX
        public int nRadishCellX = 0;
        // 萝卜CellY
        public int nRadishCellY = 0;
    }
    // 关卡中每一波Npc配置
    public class GLStageNpcConfig
    {
        // 行走路径ID
        public int nPathId = 0;
        // 出现间隔（毫秒）
        public int nInterval = 0;
        // Npc列表
        public List<int> asNpc = new List<int>();
    }
    //////////////////////////////////////////////////////////////////////////
    // 逻辑场景模板
    public class GLSceneTemplate
    {
        // 模板ID
        public int nTemplateId = 0;
        // 名字
        public string szName = null;
        // 表现场景ID
        public int nRepresentId = 0;
        // 格子类型配置
        public int[,] arrCellType = new int[RepresentDef.SCENE_CELL_COUNT_X, RepresentDef.SCENE_CELL_COUNT_Y];
    }
    //////////////////////////////////////////////////////////////////////////
    // 逻辑场景物件模板
    public class GLDoodadTemplate
    {
        // 模板ID
        public int nTemplateId = 0;
        // 名字
        public string szName = null;
        // 表现ID
        public int nRepresentId = 0;
        // 大小所占的格子数X
        public int nCellSizeX = 1;
        // 大小所占的格子数Y
        public int nCellSizeY = 1;
    }
    // 逻辑场景物件位置配置
    public class GLDoodadPos
    {
        // 模板ID
        public int nTemplateId = 0;
        // 格子坐标X
        public int nCellX = 0;
        // 格子坐标Y
        public int nCellY = 0;

        public GLDoodadPos(int nId, int nX, int nY)
        {
            nTemplateId = nId;
            nCellX = nX;
            nCellY = nY;
        }
    }
    // 逻辑场景炮塔位置配置
    public class GLTowerPos
    {
        // 模板ID
        public int nTemplateId = 0;
        // 格子坐标X
        public int nCellX = 0;
        // 格子坐标Y
        public int nCellY = 0;

        public GLTowerPos(int nId, int nX, int nY)
        {
            nTemplateId = nId;
            nCellX = nX;
            nCellY = nY;
        }
    }
    //////////////////////////////////////////////////////////////////////////
    // 逻辑场景NPC模板
    public class GLNpcTemplate
    {
        // NPC模板ID
        public int nTemplateId = 0;
        // NPC名字
        public string szName = null;
        // 表现ID
        public int nRepresentId = 0;
        // 高
        public int nHeight = 0;
        // 宽
        public int nWidth = 0;
        // 血量
        public int nLife = 0;
    }
    //////////////////////////////////////////////////////////////////////////
    // 逻辑场景萝卜模板
    public class GLRadishTemplate
    {
        // 模板ID
        public int nTemplateId = 0;
        // 名字
        public string szName = null;
        // 表现ID
        public int nRepresentId = 0;
        // 血量
        public int nLife = 0;
    }
    //////////////////////////////////////////////////////////////////////////
    // 逻辑场景特效模板
    public class GLEffectTemplate
    {
        // 模板ID
        public int nTemplateId = 0;
        // 名字
        public string szName = null;
        // 表现ID
        public int nRepresentId = 0;
    }
    //////////////////////////////////////////////////////////////////////////
    // 逻辑炮塔NPC模板
    public class GLTowerTemplate
    {
        public int    nTemplateId   = 0;                                // 模板ID
        public string szName        = null;                             // 名字
        public int    nRepresentId  = 0;                                // 表现ID
        public int    nBulletTempId = 0;                                // 子弹模板ID
        public int    nAITempId     = 0;                                // AI模板ID
    }
    //////////////////////////////////////////////////////////////////////////
    // 逻辑场景子弹模板
    public class GLMissileTemplate
    {
        public int templateId = 0;
        public string name = null;
        public int representId = 0;
        public BallisticMode ballisticMode = 0;
        public WarheadType warheadType = 0;
        public float speed = 0f;
        public int impactDamage = 0;
        public int exploseDamage = 0;
        public int exploseRadius = 0;
        public int childTemplateId = 0;
        public float lifespan = 0;
        public int warheadRadius = 0;
        public UnityEngine.Vector2 wardheadOffset = UnityEngine.Vector2.zero;
    }
    //////////////////////////////////////////////////////////////////////////
    class GameLogicDef
    {
        // 关卡配置文件
        public const string LOGIC_STAGE_TEMPLATE_FILE = "setting/logic_stagetemplate";

        // 逻辑场景配置文件
        public const string LOGIC_SCENE_TEMPLATE_FILE = "setting/logic_scenetemplate";

        // 逻辑场景路径配置文件
        public const string LOGIC_PATH_TEMPLATE_FILE = "setting/logic_pathtemplate";

        // 逻辑场景物件配置文件
        public const string LOGIC_DOODAD_TEMPLATE_FILE = "setting/logic_doodadtemplate";

        // 逻辑场景NPC配置文件
        public const string LOGIC_NPC_TEMPLATE_FILE = "setting/logic_npctemplate";

        // 逻辑场景萝卜配置文件
        public const string LOGIC_RADISH_TEMPLATE_FILE = "setting/logic_radishtemplate";

        // 逻辑场景特效配置文件
        public const string LOGIC_EFFECT_TEMPLATE_FILE = "setting/logic_effecttemplate";

        // 逻辑场景炮塔配置文件
        public const string LOGIC_TOWER_TEMPLATE_FILE = "setting/logic_towertemplate";

        // 逻辑场景子弹配置文件
        public const string LOGIC_TOWER_MISSILE_FILE = "setting/logic_missiletemplate";

        // 逻辑场景格子类型配置目录
        public const string LOGIC_SCENE_CELLTYPE_PATH = "setting/logic_scenecelltype/";

        // 关卡路径配置目录
        public const string LOGIC_STAGE_PATH_PATH = "setting/logic_scenepath/";

        // 关卡物件置配置目录
        public const string LOGIC_STAGE_DOODAD_PATH = "setting/logic_scenedoodad/";

        // 关卡炮塔置配置目录
        public const string LOGIC_STAGE_TOWER_PATH = "setting/logic_scenetower/";

        // 关卡Npc配置目录
        public const string LOGIC_STAGE_DOODAD_NPC = "setting/logic_scenenpc/";
    }
    //////////////////////////////////////////////////////////////////////////

    public struct GLCircle
    {
        public float x;                  // 圆心坐标x
        public float y;                  // 圆心坐标y
        public float r;                  // 圆半径
    }

    public struct GLRectangle
    {
        public float x;                  // 矩形中心坐标x
        public float y;                  // 矩形中心坐标y
        public float w;                  // 矩形宽度
        public float h;                  // 矩形高度
    }
}
