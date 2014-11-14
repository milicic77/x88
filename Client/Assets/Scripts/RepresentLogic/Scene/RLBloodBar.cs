using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLBloodBar : MonoBehaviour
    {
        private Texture2D m_BloodFG; // 血条前景贴图
        private Texture2D m_BloodBG; // 血条背景贴图

        private GameObject m_ObjectBG; // 背景容器：存放背景图片
        private GameObject m_ObjectFG; // 前景容器：存放前景动画

        private SpriteRenderer spriteRenderer; // 渲染组件

        private int m_nStartShowTime = 0; // 开始显示的时间（毫秒）
        private int m_nShowTime = 0; // 显示时间（毫秒）

        public void Init(float fWorldX, float fWorldY)
        {
            m_BloodFG = Resources.Load("image/misc/MonsterHP01") as Texture2D;
            m_BloodBG = Resources.Load("image/misc/MonsterHP02") as Texture2D;

            // 背景容器初始化
            m_ObjectBG = new GameObject();
            Rect spriteBGRect = new Rect(0, 0, m_BloodBG.width, m_BloodBG.height);
            Sprite spriteBG = Sprite.Create(m_BloodBG, spriteBGRect, new Vector2(1.0f, 0.5f));
            m_ObjectBG.AddComponent<SpriteRenderer>().sprite = spriteBG;
            m_ObjectBG.GetComponent<SpriteRenderer>().sortingOrder = 2;
            m_ObjectBG.transform.position = new Vector3(RepresentCommon.LogicDis2WorldDis(m_BloodBG.width) / 2, 0, 0);
            m_ObjectBG.transform.parent = gameObject.transform;

            // 前景容器初始化
            m_ObjectFG = new GameObject();
            Rect spriteFGRect = new Rect(0, 0, m_BloodFG.width, m_BloodFG.height);
            Sprite spriteFG = Sprite.Create(m_BloodFG, spriteFGRect, new Vector2(0.5f, 0.5f));
            m_ObjectFG.AddComponent<SpriteRenderer>().sprite = spriteFG;
            m_ObjectFG.GetComponent<SpriteRenderer>().sortingOrder = 1;
            m_ObjectFG.transform.position = new Vector3(0, 0, 0);
            m_ObjectFG.transform.parent = gameObject.transform;

            gameObject.name = "bloodbar";
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);

            // 初始设为不可见
            SetVisible(false);
        }

        private void UnInit()
        {

        }

        // 设置是否可见
        private void SetVisible(bool bShow)
        {
            gameObject.SetActive(bShow);
        }

        // 显示血条 （血量百分比 显示时间毫秒）
        public void Show(int nPercent, int nTime)
        {
            float fScale = (float)(100 - nPercent) / 100f;
            m_ObjectBG.transform.localScale = new Vector3(fScale, 1, 1);

            m_nShowTime = nTime;
            m_nStartShowTime = Environment.TickCount;

            SetVisible(true);
        }

        public static RLBloodBar Create(float fWorldX, float fWorldY)
        {
            GameObject gameobject = new GameObject();
            RLBloodBar bloodbar = gameobject.AddComponent<RLBloodBar>();

            bloodbar.Init(fWorldX, fWorldY);

            return bloodbar;
        }

        public static void Destroy(RLBloodBar bloodbar)
        {
            bloodbar.UnInit();
            GameObject.Destroy(bloodbar.gameObject);
        }

        virtual public void Update()
        {
            if (m_nShowTime != 0) // 需要显示
            {
                if (Environment.TickCount - m_nStartShowTime >= m_nShowTime)
                {
                    // 显示时间够了，不显示
                    m_nShowTime = 0;
                    m_nStartShowTime = 0;
                    SetVisible(false);
                }
            }
            
        }
    }
}
