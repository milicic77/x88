﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class Represent : Common.Singleton<Represent>
    {
        public void Init()
        {
            // 资源管理器
            RLResourceManager.Instance().Init();

            // 创建游戏根对象
            CreateGameRoot();

            // 创建摄像机
            CreateCamera();   
        }

        public void UnInit()
        {

        }

        public void Activate()
        {

        }

        public void Update()
        {

        }

        private void CreateGameRoot()
        {
            GameObject GameRoot = new GameObject(RepresentDef.GAME_ROOT_NAME);
            GameRoot.transform.position = new Vector3(0, 0, 0);
            RepresentEnv.GameRoot = GameRoot;
        }

        // 创建摄像机，初始化摄像机参数
        private void CreateCamera()
        {
            // 创建摄像机对象
            GameObject CameraObject = new GameObject(RepresentDef.MAIN_CAMERA_NAME);
            // 位置
            CameraObject.transform.position = new Vector3(0, 0, 0);
            // 添加摄像机组件
            Camera MainCamera = CameraObject.AddComponent<Camera>();
            // 背景色设置为黑色
            MainCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            MainCamera.clearFlags = CameraClearFlags.Skybox;
            // 正交模式
            MainCamera.orthographic = true;
            // 视口的大小（宽度的一半），Unity中默认1个单位是100像素
            // 在不改变这个值的情况下设置为3.2说明背景图片的宽是640像素
            //MainCamera.orthographicSize = 3.2f;
            MainCamera.orthographicSize = (float)RepresentDef.SCENE_SIZE_PIXEL_Y / 2 / RepresentDef.PIXEL_UNITY_SCALE;
            // 设置屏幕长宽比，背景图片是960*640像素的，所以此处就是3：2
            //MainCamera.aspect = 960.0f / 640.0f; // 3:2
            MainCamera.aspect = (float)RepresentDef.SCENE_SIZE_PIXEL_X / (float)RepresentDef.SCENE_SIZE_PIXEL_Y;
            // 近裁剪平面
            MainCamera.nearClipPlane = 0;
            // 远裁剪平台
            MainCamera.farClipPlane = 1000;

            // TODO 设置屏幕大小（拉伸或留黑边）

            RepresentEnv.MainCamera = MainCamera;
        }

        // 创建表现场景
        public RLScene CreateScene(int nRepresentId)
        {
            GameObject SceneObject = new GameObject();
            RLScene scene = SceneObject.AddComponent<RLScene>();

            scene.Init(nRepresentId);

            //scene.AddNpc(1, 6, 6);
            //scene.AddNpc(2, 8, 4);

            SceneObject.transform.parent = RepresentEnv.GameRoot.transform;

            return scene;
        }

        // 创建表现Doodad
        public RLDoodad CreateDoodad(int nRepresentId, float fWorldX, float fWorldY)
        {
            GameObject DoodadObject = new GameObject();
            RLDoodad doodad = DoodadObject.AddComponent<RLDoodad>();

            doodad.Init(nRepresentId, fWorldX, fWorldY,
                (int)RLSceneObjectOrder.RLSceneOrder_Doodad);

            return doodad;
        }

        // 创建表现Npc
        public RLNpc CreateNpc(int nRepresentId, float fWorldX, float fWorldY)
        {
            GameObject NpcObject = new GameObject();
            RLNpc npc = NpcObject.AddComponent<RLNpc>();

            npc.Init(nRepresentId, fWorldX, fWorldY,
                (int)RLSceneObjectOrder.RLSceneOrder_Npc);

            return npc;
        }

        public void DestroyNpc(RLNpc npc)
        {
            GameObject.Destroy(npc.gameObject);
        }

        // 创建表现萝卜
        public RLRadish CreateRadish(int nRepresentId, float fWorldX, float fWorldY)
        {
            GameObject RadishObject = new GameObject();
            RLRadish radish = RadishObject.AddComponent<RLRadish>();

            radish.Init(nRepresentId, fWorldX, fWorldY,
                (int)RLSceneObjectOrder.RLSceneOrder_Radish);

            return radish;
        }

        // 创建表现炮塔
        public RLTower CreateTower(int nRepresentId, float fWorldX, float fWorldY)
        {
            GameObject TowerObject = new GameObject();
            RLTower tower = TowerObject.AddComponent<RLTower>();

            tower.Init(nRepresentId, fWorldX, fWorldY,
                (int)RLSceneObjectOrder.RLSceneOrder_Tower);

            return tower;
        }
    }
}