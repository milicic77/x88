using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game.Common;
using Game.UI;
using Game.UI.UIEvent;
using Game.GameEvent;

namespace Game.RepresentLogic
{
    class RLUIManager : Singleton<RLUIManager>
    {
        private const string SCREEN_UIROOT_PATH = "Prefabs\\UI\\ScreenCavas";
        private RLScreenUIRoot m_screenUIRoot = null;

        public void Init()
        {
            //创建屏幕UI背景画布
            CreateScreenUIRoot(SCREEN_UIROOT_PATH);

            //游戏逻辑事件注册
            EventCenter.Event_ClientInitComplete += OnGameStart;
            EventCenter.Event_LevelStart += OnLevelStart;
            EventCenter.Event_GameOver += OnGameOver;

            //UI事件注册
            UIEventCenter.EventAdventureGameStart += OnAdventureGameStart;
            UIEventCenter.EventOpenGameMenu += OnGameMenuOpen;
            
        }

        //游戏逻辑事件回调
        public void OnGameStart(object sender, EventDef.BaseEventArgs args)
        {
            RLUISetting uiSetting = RLResourceManager.Instance().GetRLUISetting((int)UITypeDef.UI_STARTGAME);
            if (uiSetting != null)
            {
                m_screenUIRoot.CreateChild(UIPathDef.UI_RESOURCE_PATH + uiSetting.resourceName);
            }
        }
        public void OnLevelStart(object sender, EventDef.BaseEventArgs args)
        {
            RLUISetting uiSetting = RLResourceManager.Instance().GetRLUISetting((int)UITypeDef.UI_GAMEMAIN);
            if (uiSetting != null)
            {
                m_screenUIRoot.CreateChild(UIPathDef.UI_RESOURCE_PATH + uiSetting.resourceName);
            }
        }
        public void OnGameOver(object sender, EventDef.GameOverArgs args)
        {
            if(args.nWin == 0)
            {
                RLUISetting uiSetting = RLResourceManager.Instance().GetRLUISetting((int)UITypeDef.UI_GAMEOVER);
                if (uiSetting != null)
                {
                    m_screenUIRoot.CreateChild(UIPathDef.UI_RESOURCE_PATH + uiSetting.resourceName);
                }
            }
        }

        //UI事件回调
        public void OnAdventureGameStart(object sender, UIEventDef.BaseUIEventArgs args)
        {
            RLUISetting uiSetting = RLResourceManager.Instance().GetRLUISetting((int)UITypeDef.UI_LEVELSELECT);
            if (uiSetting != null)
            {
                m_screenUIRoot.CreateChild(UIPathDef.UI_RESOURCE_PATH + uiSetting.resourceName);

            }
        }
        public void OnGameMenuOpen(object sender, UIEventDef.BaseUIEventArgs args)
        {
            RLUISetting uiSetting = RLResourceManager.Instance().GetRLUISetting((int)UITypeDef.UI_GAMEMENU);
            if (uiSetting != null)
            {
                m_screenUIRoot.CreateChild(UIPathDef.UI_RESOURCE_PATH + uiSetting.resourceName);
            }
        }

        private void CreateScreenUIRoot(string resourcePath)
        {
            GameObject objScreenUIRoot = new GameObject();
            m_screenUIRoot = objScreenUIRoot.AddComponent<RLScreenUIRoot>();
            m_screenUIRoot.Init();
        }
    }
}
