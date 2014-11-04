using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.RepresentLogic
{
    // 场景层深度
    public enum SceneLayerZ
    {
        SceneGroundZ_Back = 3,
        SceneGroundZ_Middle = 2,
        SceneGroundZ_Force = 1,
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

    // 场景对象信息
    public class SceneObjectInfo
    {
        public int nLogicX; // 逻辑X坐标
        public int nLogicY; // 逻辑Y坐标
    }

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

    public class RepresentEnv
    {
        // 场景根对象
        static public GameObject SceneRoot;
        // 场景摄像机对象
        static public Camera SceneCamera;
    }
}
