using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class RLScreenUIRoot : MonoBehaviour
    {
        private GameObject m_objUGUIEventSystem = null;
        private Canvas m_canvas = null;

        public void Init()
        {
            gameObject.name = "ScreenCanvas";
            gameObject.layer = 5;
            m_canvas = gameObject.AddComponent<Canvas>();

            gameObject.AddComponent<GraphicRaycaster>();

            m_canvas.renderMode = RenderMode.Overlay;

            CreateUGUIEventSystem();
        }

        public void CreateChild(string resourcePath)
        {
            GameObject child = Instantiate(Resources.Load(resourcePath)) as GameObject;
            RectTransform childTrans = child.GetComponent<RectTransform>();
            childTrans.parent = gameObject.GetComponent<RectTransform>();
            childTrans.offsetMin = new Vector2(0.0f, 0.0f);
            childTrans.offsetMax = new Vector2(0.0f, 0.0f);
        }

        private void CreateUGUIEventSystem()
        {
            m_objUGUIEventSystem = new GameObject();
            m_objUGUIEventSystem.name = "EventSystem";
            m_objUGUIEventSystem.AddComponent<EventSystem>();
            m_objUGUIEventSystem.AddComponent<StandaloneInputModule>();
            m_objUGUIEventSystem.AddComponent<TouchInputModule>();
        }
    }
}
