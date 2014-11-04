using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.RepresentLogic
{
    public class SceneDef
    {
        // 表现场景配置文件
        public const string SCENE_REPRESENT_LIST_FILE = "setting/scene/representscenetemplatelist";
        // 表现对象配置文件（NPC等）
        public const string SCENEOBJECT_REPRESENT_LIST_FILE = "setting/sceneobject/representsceneobjecttemplatelist";
    }

    public class SceneObjectDef
    {
        public const int ANI_TEXTURE_COUNT = 2; // 每个动作两张贴图
    }

    // 场景格子类型
    public enum SceneCellType
    {
        SceneCellType_None = 0, // 无
        SceneCellType_Road = 1, // 道路（可以通过，不可建造）
        SceneCellType_Obstacle = 2, // 障碍点（不可通过，不可建造）
        SceneCellType_Idel = 3, // 空闲点（不可通过，可以建造）
    }

    // 场景层深度
    public enum SceneLayerZ
    {
        SceneGroundZ_Back = 5,      // 场景底层
        SceneGroundZ_Middle = 4,    // 场景中层
        SceneGroundZ_Force = 3,     // 场景上层
        SceneGroundZ_Object = 2,    // 场景对象
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
        public SceneLayerZ nZ;
    };

    public class RepresentDef
    {
        // 摄像机对象名
        public const string SCENE_CAMERA_NAME = "SceneCamera";
        // 场景根对象名
        public const string SCENE_ROOT_NAME = "SceneRoot";

        // 场景每行的CELL数量
        public const int SCENE_CELL_MAX_X = 20;
        // 场景每列的CELL数量
        public const int SCENE_CELL_MAX_Y = 15;

        // 场景像素大小X
        public const int SCENE_PIXEL_X = 800;
        // 场景像素大小Y
        public const int SCENE_PIXEL_Y = 600;
        // 场景对象像素大小X
        public const int SCENE_OBJECT_PIXEL_X = 40;
        // 场景对象像素大小Y
        public const int SCENE_OBJECT_PIXEL_Y = 40;
    }

    // 表现场景配置
    public class RepresentSceneConfig
    {
        // 表现场景ID
        public int nTemplateId;
        // 表现场景名字
        public string szName;
        // 资源路径
        public string szPath;
        // 背景图路径
        public string szBackGroundImage;
        // 中间层图片路径
        public string szMiddleGroundImage;
        // 格子类型
        public int [,] nCellType;
    }

    // 表现对象配置
    public class RepresentSceneObjectConfig
    {
        // 表现ID
        public int nRepresentId;
        // 表现名字
        public string szName;
        // 资源路径
        public string szPath;
        // 默认贴图
        public Texture2D DefaultTexture;

        // 动作贴图 SceneObjectDef.ANI_TEXTURE_COUNT
        // 站立 向上
        public Texture2D[] arrTexStandUp;
        // 站立 向下
        public Texture2D[] arrTexStandDown;
        // 站立 向左
        public Texture2D[] arrTexStandLeft;
        // 站立 向右
        public Texture2D[] arrTexStandRight;
        // 移动 向上
        public Texture2D[] arrTexRunUp;
        // 移动 向下
        public Texture2D[] arrTexRunDown;
        // 移动 向左
        public Texture2D[] arrTexRunLeft;
        // 移动 向右
        public Texture2D[] arrTexRunRight;
        // 攻击 向上
        public Texture2D[] arrTexAttackUp;
        // 攻击 向下
        public Texture2D[] arrTexAttackDown;
        // 攻击 向左
        public Texture2D[] arrTexAttackLeft;
        // 攻击 向右
        public Texture2D[] arrTexAttackRight;
        // 受伤 向上
        public Texture2D[] arrTexHurtUp;
        // 受伤 向下
        public Texture2D[] arrTexHurtDown;
        // 受伤 向左
        public Texture2D[] arrTexHurtLeft;
        // 受伤 向右
        public Texture2D[] arrTexHurtRight;

    }

    public class RepresentEnv
    {
        // 场景根对象
        static public GameObject SceneRoot;
        // 场景摄像机对象
        static public Camera SceneCamera;
    }
}
