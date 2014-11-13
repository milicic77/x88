using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLBloodBar : MonoBehaviour
    {
        // 血条前景贴图
        public Texture2D m_BloodFG;
        // 血条背景贴图
        public Texture2D m_BloodBG;

        public GameObject m_ObjectBG;                              // 背景容器：存放背景图片
        public GameObject m_ObjectFG;                              // 前景容器：存放前景动画

        public Sprite m_sprites;

        public void Init()
        {
            m_BloodFG = Resources.Load("image/misc/MonsterHP01") as Texture2D;
            m_BloodBG = Resources.Load("image/misc/MonsterHP02") as Texture2D;

            // 背景容器初始化
            m_ObjectBG = new GameObject();
            Rect spriteBGRect = new Rect(0, 0, m_BloodBG.width, m_BloodBG.height);
            Sprite spriteBG = Sprite.Create(m_BloodBG, spriteBGRect, new Vector2(0.5f, 0.5f));
            m_ObjectBG.AddComponent<SpriteRenderer>().sprite = spriteBG;
            m_ObjectBG.GetComponent<SpriteRenderer>().sortingOrder = 2;
            m_ObjectBG.transform.position = new Vector3(0, 0, 0);
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
            gameObject.transform.position = new Vector3(0, 0, 0);

            m_ObjectBG.transform.localScale = new Vector3(0.5f,1,1);
        }

        private void UnInit()
        {

        }

        public static RLBloodBar Create()
        {
            GameObject gameobject = new GameObject();
            RLBloodBar bloodbar = gameobject.AddComponent<RLBloodBar>();

            bloodbar.Init();

            return bloodbar;
        }

        public static void Destroy(RLBloodBar bloodbar)
        {
            bloodbar.UnInit();
            GameObject.Destroy(bloodbar.gameObject);
        }

        virtual public void Update()
        {
            //float fBGTop = transform.position.y + m_BloodBG.height / 2;
            //float fBGLeft = transform.position.x - m_BloodBG.width / 2;

            //float fFGTop = transform.position.y + m_BloodFG.height / 2;
            //float fFGLeft = transform.position.x - m_BloodFG.width / 2;

            //// 绘制前景
            //GUI.DrawTexture(
            //    new Rect(fFGLeft, fFGTop, m_BloodFG.width, m_BloodFG.height), 
            //    m_BloodFG
            //);

            //// 绘制背景
            //GUI.DrawTexture(
            //    new Rect(fBGLeft, fBGTop, m_BloodBG.width, m_BloodBG.height), 
            //    m_BloodBG
            //);
        }
    }
}
