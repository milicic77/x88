using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Common;
using Game.GameEvent;
using Game.UI.UIEvent;

namespace Game.UI
{
    public class RLUIGameOver : MonoBehaviour
    {
        //可交互控件的名字
        private const string BTN_SELECTLEVEL = "btn_selectlevel";       //选择关卡按钮
        private const string BTN_TRYAGAIN = "btn_tryagain";             //再试一次按钮

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
            if (strObjName.Equals(BTN_SELECTLEVEL))
            {
                UIEventCenter.EventAdventureGameStart(null, null);
                Destroy(gameObject);
            }
            else if (strObjName.Equals(BTN_TRYAGAIN))
            {
                EventCenter.Event_LevelStart(null, null);
                Destroy(gameObject);
            }
        }
    }
}