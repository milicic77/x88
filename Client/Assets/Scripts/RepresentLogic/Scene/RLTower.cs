using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLTower : MonoBehaviour
    {
        public Sprite[] m_SpriteFireAni = null; // 攻击动画精灵

        private SpriteRenderer spriteRenderer; // 动画渲染


        // 表现模板
        public RLTowerTemplate m_Template;

        public float framesPerSecond = 10;

        public GameObject m_ObjectBG;
        public GameObject m_ObjectFG;

        //// 当前动作
        //public int m_nAni = (int)SceneObjectAni.SceneObjectAni_Stand;
        //// 当前方向
        //public int m_Direction = (int)SceneObjectDirection.SceneObjectDirection_Right;

        //public int DOING
        //{
        //    set
        //    {
        //        m_nAni = value;
        //    }
        //    get
        //    {
        //        return m_nAni;
        //    }
        //}

        //public int DIRECTION
        //{
        //    set
        //    {
        //        m_Direction = value;
        //    }
        //    get
        //    {
        //        return m_Direction;
        //    }
        //}

        //public SpriteRenderer SceneObjectSpriteRenderer
        //{
        //    set
        //    {
        //        spriteRenderer = value;
        //    }
        //    get
        //    {
        //        return spriteRenderer;
        //    }
        //}

        public void Init(int nRepresentId, float fWorldX, float fWorldY, int nOrder)
        {
            RLTowerTemplate template = RLResourceManager.Instance().GetRLTowerTemplate(nRepresentId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nRepresentId不合法！");
            }

            m_Template = template;

            //////////////////////////////////////////////////////////////////////////
            // 背景
            m_ObjectBG = new GameObject();
            Rect spriteBGRect = new Rect(0, 0,
                template.BGTexture.width, template.BGTexture.height);
            Sprite spriteBG = Sprite.Create(template.BGTexture,
                spriteBGRect,
                new Vector2(0.5f, 0.5f)
            );
            m_ObjectBG.AddComponent<SpriteRenderer>().sprite = spriteBG;

            m_ObjectBG.GetComponent<SpriteRenderer>().sortingOrder = 1;

            m_ObjectBG.transform.position = new Vector3(0, 0, 0);
            m_ObjectBG.transform.parent = gameObject.transform;

            //////////////////////////////////////////////////////////////////////////
            // 前景
            m_ObjectFG = new GameObject();
            Rect spriteRect = new Rect(0, 0,
                template.DefaultTexture.width, template.DefaultTexture.height);
            // 精灵
            Sprite sprite = Sprite.Create(template.DefaultTexture,
                spriteRect,
                new Vector2(0.5f, 0.5f)
            );

            // 攻击
            m_SpriteFireAni = new Sprite[template.TexFireAni.Count];
            for (int i = 0; i < template.TexFireAni.Count; ++i)
            {
                Rect rect = new Rect(0, 0,
                    template.TexFireAni[i].width, template.TexFireAni[i].height);

                m_SpriteFireAni[i] = Sprite.Create(
                    template.TexFireAni[i],
                    rect,
                    new Vector2(0.5f, 0.5f)
                );
            }

            m_ObjectFG.AddComponent<SpriteRenderer>().sprite = sprite;

            spriteRenderer = m_ObjectFG.GetComponent<Renderer>() as SpriteRenderer;

            m_ObjectFG.GetComponent<SpriteRenderer>().sortingOrder = 2;

            m_ObjectFG.transform.position = new Vector3(0, 0, 0);
            m_ObjectFG.transform.parent = gameObject.transform;

            //////////////////////////////////////////////////////////////////////////
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;
            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        public void SetPosition(float fWorldX, float fWorldY)
        {
            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        virtual public void Update()
        {
            int nIndex = (int)(Time.timeSinceLevelLoad * framesPerSecond);

            nIndex = nIndex % m_Template.TexFireAni.Count;

            spriteRenderer.sprite = m_SpriteFireAni[nIndex];
        }
    }
}
