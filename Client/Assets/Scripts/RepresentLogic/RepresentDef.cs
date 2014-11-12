using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.RepresentLogic
{
    public class SceneDef
    {
        // 表现场景模板
        public const string SCENE_REPRESENT_TEMPLATE_FILE = "setting/represent_scenetemplate";
        // 表现场景底层背景图片路径
        public const string SCENE_REPRESENT_BG_PATH = "image/bg/";
        // 表现场景中层背景图片路径
        public const string SCENE_REPRESENT_MG_PATH = "image/mg/";
        // 表现场景上层背景图片路径
        public const string SCENE_REPRESENT_FG_PATH = "image/fg/";

        // 表现物品模板
        public const string DOODAD_REPRESENT_TEMPLATE = "setting/represent_doodadtemplate";
        // 表现物品图片路径
        public const string DOODAD_REPRESENT_TEXTURE_PATH= "image/doodad/";
        // 表现物品每个部分的像素长和宽的大小（一个物品可以由多个部分组成）
        public const int DOODAD_REPRESENT_PART_PIXEL_SIZE = 64;

        // 表现怪物模板
        public const string NPC_REPRESENT_TEMPLATE = "setting/represent_npctemplate";
        // 表现怪物图片路径
        public const string NPC_REPRESENT_TEXTURE_PATH = "image/npc/";

        // 表现萝卜模板
        public const string RADISH_REPRESENT_TEMPLATE = "setting/represent_radishtemplate";
        // 表现萝卜图片路径
        public const string RADISH_REPRESENT_TEXTURE_PATH = "image/radish/";

        // 表现特效模板
        public const string EFFECT_REPRESENT_TEMPLATE = "setting/represent_effecttemplate";
        // 表现特效图片路径
        public const string EFFECT_REPRESENT_TEXTURE_PATH = "image/effect/";

        // 表现炮塔模板
        public const string TOWER_REPRESENT_TEMPLATE = "setting/represent_towertemplate";
        // 表现子弹模板
        public const string MISSILE_REPRESENT_TEMPLATE = "setting/represent_missiletemplate";
        // 表现炮塔图片路径
        public const string TOWER_REPRESENT_TEXTURE_PATH = "image/tower/";
        // 表现子弹图片路径
        public const string MISSILE_REPRESENT_TEXTURE_PATH = "image/tower/";
    }

    public class SceneObjectDef
    {
        public const int ANI_TEXTURE_COUNT = 2; // 每个动作两张贴图
    }


    // 场景层深度，也就是渲染覆盖关系（值越大越先渲染）
    public enum RLSceneObjectOrder
    {
        RLSceneOrder_BackLayer = 0,      // 场景底层
        RLSceneOrder_MiddleLayer = 1,    // 场景中层
        RLSceneOrder_ForceLayer = 2,     // 场景上层
        RLSceneOrder_Doodad = 3,         // 场景Doodad
        RLSceneOrder_Radish = 4,         // 场景萝卜
        RLSceneOrder_Tower = 5,         // 场景炮塔
        RLSceneOrder_Npc = 6,           // 场景Npc
        RLSceneOrder_Missile = 7,           // 场景子弹
        RLSceneOrder_Effect = 7,         // 场景特效
    }

    // 场景对象动作
    public enum SceneObjectAni
    {
        SceneObjectAni_Stand = 1,  // 站立
        SceneObjectAni_Run = 2,    // 移动
        SceneObjectAni_Attack = 3,     // 攻击
        SceneObjectAni_Hurt = 4,    // 受伤

        SceneObjectAni_Count,
    }

    // 场景对象方向
    public enum SceneObjectDirection
    {
        SceneObjectDirection_Up = 1, // 上
        SceneObjectDirection_Down = 2, // 下
        SceneObjectDirection_Left = 3, // 左
        SceneObjectDirection_Right = 4, // 右

        SceneObjectDirection_Count,
    }

    // 场景层信息
    public class SceneLayerInfo
    {
        // 层对象名
        public string szLayerName;
        // 层贴图文件名
        public string szTextureName;
        // 层贴图文件大小
        public Rect sTextureRect;
        // 层深度
        public RLSceneObjectOrder nZ;
    };

    public class RepresentDef
    {
        // 主摄像机对象名
        public const string MAIN_CAMERA_NAME = "MainCamera";

        // 场景根对象名
        public const string GAME_ROOT_NAME = "GameRoot";

        // 场景每行的CELL数量
        public const int SCENE_CELL_COUNT_X = 12;
        // 场景每列的CELL数量
        public const int SCENE_CELL_COUNT_Y = 8;

        // 场景预留的空白像素大小X
        //public const int SCENE_SPACE_SIZEPIXEL_X = 60;

        // 场景像素大小X
        public const int SCENE_SIZE_PIXEL_X = 960;
        // 场景像素大小Y
        public const int SCENE_SIZE_PIXEL_Y = 640;

        // 场景格子像素大小X 960 / 12 = 80
        public const int SCENE_CELL_SIZE_PIXEL_X = 80;
        // 场景格子像素大小Y 640 / 8 = 80
        public const int SCENE_CELL_SIZE_PIXEL_Y = 80;

        // Unity单位场景大小X
        public const float SCENE_SIZE_UNITY_X = 960.0f / (float)PIXEL_UNITY_SCALE;
        // Unity单位场景大小Y
        public const float SCENE_SIZE_UNITY_Y = 640.0f / (float)PIXEL_UNITY_SCALE;

        // 像素与Unity单位的大小比 默认是100
        public const int PIXEL_UNITY_SCALE = 100;
    }

    // 表现场景配置
    public class RLSceneTemplate
    {
        // 表现场景ID
        public int nTemplateId;
        // 表现场景名字
        public string szName;
        // 资源路径
        //public string szPath;
        // 底层背景图路径
        public string szBackGroundImage;
        // 中间层图片路径
        public string szMiddleGroundImage;
        // 上层图片路径
        public string szForceGroundImage;
        // 格子类型
        //public int [,] nCellType;
    }

    // 表现物品配置
    public class RLDoodadTemplate
    {
        // 表现ID
        public int nRepresentId;
        // 表现名字
        public string szName;
        // 贴图路径
        public string szImage;
        // 贴图
        public Texture2D texture;
        // 中心点X
        public float fPivotX;
        // 中心点Y
        public float fPivotY;
    }

    // 表现怪物配置
    public class RLNpcTemplate
    {
        // 表现ID
        public int nRepresentId;
        // 表现名字
        public string szName;
        // 动画贴图
        public Texture2D [] TexAni = new Texture2D [2];
    }

    // 表现萝卜配置
    public class RLRadishTemplate
    {
        // 表现ID
        public int nRepresentId;
        // 表现名字
        public string szName;
        // 站立动画1贴图
        public List<Texture2D> TexStandAni1 = new List<Texture2D>();
        // 站立动画2贴图
        public List<Texture2D> TexStandAni2 = new List<Texture2D>();
        // 默认贴图
        public Texture2D DefaultTexture = null;
    }
    // 表现特效模板
    public class RLEffectTemplate
    {
        // 表现ID
        public int nRepresentId;
        // 表现名字
        public string szName;
        // 站立动画1贴图
        public List<Texture2D> TexAni = new List<Texture2D>();
    }

    // 表现炮塔配置
    public class RLTowerTemplate
    {
        // 表现ID
        public int nRepresentId;
        // 表现名字
        public string szName;
        // 攻击动画贴图
        public List<Texture2D> TexFireAni = new List<Texture2D>();
        // 默认贴图
        public Texture2D DefaultTexture = null;
        // 背景贴图
        public Texture2D BGTexture = null;
    }

    // 表现子弹配置
    public class RLMissileTemplate
    {
        // 表现ID
        public int nRepresentId;
        // 表现名字
        public string szName;
        // 攻击动画贴图
        public List<Texture2D> TexFlyAni = new List<Texture2D>();
    }

    public class RepresentEnv
    {
        // 场景根对象
        static public GameObject GameRoot;
        // 场景摄像机对象
        static public Camera MainCamera;
    }

    public enum UITypeDef
    {
        UI_STARTGAME    = 1,        //游戏开始界面
        UI_LEVELSELECT  = 2,        //关卡选择界面
        UI_GAMEMAIN     = 3,        //游戏主界面
        UI_GAMEMENU     = 4,        //游戏菜单界面
    }
    public class UIPathDef
    {
        public const string UI_RESOURCE_PATH = "Prefabs/UI/";
        public const string STR_UICONFIG_PATH = "setting/UIConfig";
    }
    //UI配置
    public class RLUISetting
    {
        //编号
        public int nUINumber;
        
        //资源名称
        public string resourceName; 
   
        //是否场景UI（不是则为屏幕UI）
        public bool bIsSceneUI;         
    }
}
