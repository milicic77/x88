using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RepresentLogic
{
    public class Represent
    {
        public void Init()
        {
            CreateSceneRoot();

            CreateCamera();

            CreateScene();
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

        private void CreateSceneRoot()
        {
            GameObject sceneRoot = new GameObject(RepresentDef.SCENE_ROOT_NAME);
            sceneRoot.transform.position = new Vector3(0, 0, 0);
            RepresentEnv.SceneRoot = sceneRoot;
        }

        // 创建摄像机
        private void CreateCamera()
        {
            GameObject sceneCameraObj = new GameObject(RepresentDef.SCENE_CAMERA_NAME);
            sceneCameraObj.transform.position = new Vector3(0, 0, -3);
            Camera sceneCamera = sceneCameraObj.AddComponent<Camera>();
            sceneCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            sceneCamera.clearFlags = CameraClearFlags.Skybox;


            RepresentEnv.SceneCamera = sceneCamera;
        }

        private void CreateScene()
        {
            //GameObject scene = new GameObject("test");
            //scene.AddComponent("BackGround");
            //scene.AddComponent("MiddleGround");
            //scene.AddComponent("ForceGround");
            //Sprite sceneSprite = sceneCameraObj.AddComponent<Sprite>();
            RepresentScene RScene = new RepresentScene();
            RScene.Create();
        }
    }
}