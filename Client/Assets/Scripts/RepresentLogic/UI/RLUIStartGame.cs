using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Common;
using Game.UI.UIEvent;

namespace Game.UI
{
    public class RLUIStartGame : MonoBehaviour
    {
        //可交互控件的名字
        private const string BTN_ADVENTURE = "btn_adventure";       //冒险模式按钮
        private const string BTN_BOSS = "btn_boss";                 //BOSS模式按钮
        private const string BTN_MONSTER = "btn_monster";           //怪物窝按钮
        private const string BTN_SETTING = "btn_setting";           //系统设置按钮
        private const string BTN_HELP = "btn_help";                 //帮助按钮

        void Start()
        {
            Init();
        }

        public void Init()
        {

        }

        //各种button按钮的点击事件回调，参数为触发点击事件的控件名
        public void OnButtonClick(string strObjName)
        {
            if(strObjName.Equals(BTN_ADVENTURE))
            {
                UIEventCenter.EventAdventureGameStart(null, null);
                Destroy(gameObject);
            }
        }
    }
}