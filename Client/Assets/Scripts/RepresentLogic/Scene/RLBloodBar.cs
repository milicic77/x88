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

        //public float m_fWorldX;
        //public float m_fWorldY;

        private void Init()
        {
            m_BloodFG = Resources.Load("image/misc/MonsterHP01") as Texture2D;
            m_BloodBG = Resources.Load("image/misc/MonsterHP02") as Texture2D;
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
