using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Common;
using Game.GameEvent;

namespace Game.UI
{
    public class RLUILevelSelect : MonoBehaviour
    {
        //可交互控件的名字
        private const string BTN_GOMAP1 = "btn_gomap1";         //地图选择按钮
        private const string BTN_GOLEVEL1   = "btn_golevel1";       //关卡选择按钮

        private Transform m_selfTransform = null;
        private Transform m_mapSelectTrans = null;
        private Transform m_levelSelectTrans = null;

        void Start ()
        {
            Init();
        }

        public void Init()
        {
            m_selfTransform = gameObject.GetComponent<RectTransform>();
            m_mapSelectTrans = m_selfTransform.FindChild("UIMapSelect");
            m_levelSelectTrans = m_selfTransform.FindChild("UIMapLevelSelect");

            OpenMapSelectWindow();
        }

        //各种button按钮的点击事件回调，参数为触发点击事件的控件名
        public void OnButtonClick(string strObjName)
        {
            if (strObjName.Equals(BTN_GOMAP1))
            {
                OpenLevelSelectWindow();
            }
            else if(strObjName.Equals(BTN_GOLEVEL1))
            {
                GameEvent.EventCenter.Event_LevelStart(null, null);
                Destroy(gameObject);
            }
        }

        private void OpenMapSelectWindow()
        {
            m_mapSelectTrans.gameObject.SetActive(true);
            m_levelSelectTrans.gameObject.SetActive(false);
        }

        private void OpenLevelSelectWindow()
        {
            m_mapSelectTrans.gameObject.SetActive(false);
            m_levelSelectTrans.gameObject.SetActive(true);
        }
    }
}