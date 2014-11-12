using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Common;
using Game.UI.UIEvent;

namespace Game.UI
{
    public class RLUIGameMenu : MonoBehaviour
    {
        //可交互控件的名字
        private const string BTN_GOON           = "btn_goon";               //继续游戏按钮
        private const string BTN_RESTART        = "btn_restart";            //重新开始按钮
        private const string BTN_SELECTLEVEL    = "btn_selectlevel";        //重新选择关卡按钮

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
            if (strObjName.Equals(BTN_GOON))
            {
                Destroy(gameObject);
            }
            else if (strObjName.Equals(BTN_RESTART))
            {

            }
            else if (strObjName.Equals(BTN_SELECTLEVEL))
            {

            }
        }
    }
}