using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Common;
using Game.UI.UIEvent;

namespace Game.UI
{
    public class RLUIGameMain : MonoBehaviour
    {
        //可交互控件的名字
        private const string BTN_OPENGAMEMENU = "btn_opengamemenu";     //打开系统菜单

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
            if (strObjName.Equals(BTN_OPENGAMEMENU))
            {
                UIEventCenter.EventOpenGameMenu(null, null);
            }
        }
    }
}