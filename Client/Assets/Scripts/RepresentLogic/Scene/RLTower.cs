using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLTower : MonoBehaviour
    {
        // 表现模板
        public RLTowerTemplate m_Template;                  // 炮塔模板
        public GameObject      m_ObjectBG;                  // 背景容器：存放背景图片
        public GameObject      m_ObjectFG;                  // 前景容器：存放前景动画

        // 动画控制器
        private RLAniController m_AniController = new RLAniController();

        // 同步逻辑的数据
        private int m_nAngle         = 0;
        private int m_nFireRange     = 0;                   // 炮塔射程(像素)
        private int m_nRotationAngle = 0;                   // 逻辑帧1帧转的角度

        public int Angle
        {
            get { return m_nAngle;  }
            set { m_nAngle = value; }
        }

        public int FireRange
        {
            get { return m_nFireRange;  }
            set { m_nFireRange = value; }
        }

        public int RotationAngle
        {
            get { return m_nRotationAngle;  }
            set { m_nRotationAngle = value; }
        }

        public void Init(int nRepresentId, float fWorldX, float fWorldY, int nOrder)
        {
            // 初始化模板
            RLTowerTemplate template = RLResourceManager.Instance().GetRLTowerTemplate(nRepresentId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nRepresentId不合法！");
            }
            m_Template = template;

            // 背景容器初始化
            m_ObjectBG = new GameObject();
            Rect   spriteBGRect = new Rect(0, 0, template.BGTexture.width, template.BGTexture.height);
            Sprite spriteBG     = Sprite.Create(template.BGTexture, spriteBGRect, new Vector2(0.5f, 0.5f));
            m_ObjectBG.AddComponent<SpriteRenderer>().sprite       = spriteBG;
            m_ObjectBG.GetComponent<SpriteRenderer>().sortingOrder = 1;
            m_ObjectBG.transform.position = new Vector3(0, 0, 0);
            m_ObjectBG.transform.parent   = gameObject.transform;

            // 前景容器初始化
            m_ObjectFG = new GameObject();
            Rect   spriteFGRect = new Rect(0, 0, template.DefaultTexture.width, template.DefaultTexture.height);
            Sprite spriteFG     = Sprite.Create(template.DefaultTexture, spriteFGRect, new Vector2(0.5f, 0.5f));
            m_ObjectFG.AddComponent<SpriteRenderer>().sprite = spriteFG;
            m_ObjectFG.GetComponent<SpriteRenderer>().sortingOrder = 2;
            m_ObjectFG.transform.position = new Vector3(0, 0, 0);
            m_ObjectFG.transform.parent   = gameObject.transform;

            // 动画控制器初始化 - m_SpriteAnimation初始化
            // -- step 1 : 初始化动画控制器组件
            m_AniController.SpriteAnimation = new Sprite[template.TexFireAni.Count];
            for (int i = 0; i < template.TexFireAni.Count; ++i)
            {
                Rect rect = new Rect(0, 0, template.TexFireAni[i].width, template.TexFireAni[i].height);

                m_AniController.SpriteAnimation[i] = Sprite.Create(
                    template.TexFireAni[i],
                    rect,
                    new Vector2(0.5f, 0.5f)
                );
            }
            m_AniController.SpriteRenderer = m_ObjectFG.GetComponent<Renderer>() as SpriteRenderer;

            // 整个炮塔容器初始化
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
            m_AniController.Update();

            if (m_nRotationAngle > 0)
            {
                //transform.Rotate(Vector3.back,    m_nRotationAngle);
                m_ObjectFG.transform.Rotate(Vector3.back, m_nRotationAngle);
            }
            else
            {
                //transform.Rotate(Vector3.forward, -m_nRotationAngle);
                m_ObjectFG.transform.Rotate(Vector3.forward, -m_nRotationAngle);
            }

            m_nRotationAngle = 0;
        }

        void OnDrawGizmos()
        {
            //DrawFireRange(transform);                                   // 整个容器
            DrawFireRange(m_ObjectFG.transform);                        // 前景容器
        }

        private void DrawFireRange(Transform transform)
        {
            float fRadius         = m_nFireRange / RepresentDef.PIXEL_UNITY_SCALE;
            float fTheta          = 0.1f;
            Color fireRangeColor  = Color.red;
            Color fireVectorColor = Color.green;

            if (null == transform)
                return;

            if (fTheta < 0.0001f)
                fTheta = 0.0001f;

            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // 设置颜色
            Color defaultColor = Gizmos.color;
            Gizmos.color = fireRangeColor;

            // 绘制圆环
            Vector3 beginPoint = Vector3.zero;                          // 每次画线的起始点
            Vector3 firstPoint = Vector3.zero;                          // 记录圆的第一个点

            for (float theta = 0; theta < 2 * Mathf.PI; theta += fTheta)
            {
                float   x        = fRadius * Mathf.Sin(theta);
                float   y        = fRadius * Mathf.Cos(theta);
                Vector3 endPoint = new Vector3(x, y, 0);                // 每次画线的结束点

                if (theta == 0)
                { // 画半径(本地坐标Y轴正方向)
                    Gizmos.color = fireVectorColor;

                    firstPoint = endPoint;                              // 保存圆的第一个点
                    Gizmos.DrawLine(beginPoint, endPoint);              // 画出炮管方向向量
                    beginPoint = endPoint;                              // 下一次画线起始点

                    Gizmos.color = fireRangeColor;
                    continue;
                }

                Gizmos.DrawLine(beginPoint, endPoint);
                beginPoint = endPoint;                                  // 下一次画线起始点
            }

            // 绘制最后一条线段
            Gizmos.DrawLine(firstPoint, beginPoint);

            // 恢复默认颜色
            Gizmos.color = defaultColor;

            // 恢复默认矩阵
            Gizmos.matrix = defaultMatrix;
        }
    }
}
