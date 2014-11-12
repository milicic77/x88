using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;
using Game.GameLogic;

namespace Game.RepresentLogic
{
    public class RLNpc : MonoBehaviour
    {
        public Sprite[] m_sprites = new Sprite[2]; // 动画贴图
        private SpriteRenderer spriteRenderer; // 动画渲染

        //private RLSceneObjectAnimationCtrl m_Animation = new RLSceneObjectAnimationCtrl();


        //public float moveSpeed;

        // 表现模板
        public RLNpcTemplate m_Template;

        // 每秒播放几帧动画
        public float framesPerSecond = 5;

        public GLNpc m_GLNpc = null;

        //// 逻辑X坐标
        //public int m_nLogicX;
        //// 逻辑Y坐标
        //public int m_nLogicY;
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

        public void Init(int nRepresentId, float fWorldX, float fWorldY, int nOrder, GLNpc npc)
        {
            RLNpcTemplate template = RLResourceManager.Instance().GetRLNpcTemplate(nRepresentId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nRepresentId不合法！");
            }

            m_Template = template;

            Rect spriteRect = new Rect(0, 0, 
                template.TexAni[0].width, template.TexAni[0].height);

            // 精灵
            Sprite sprite = Sprite.Create(template.TexAni[0],
                spriteRect,
                new Vector2(0.5f, 0.0f)
            );

            for (int i = 0; i < 2; ++i )
            {
                Rect rect = new Rect(0, 0, 
                    template.TexAni[i].width, template.TexAni[i].height);

                m_sprites[i] = Sprite.Create(
                    template.TexAni[i],
                    rect,
                    new Vector2(0.5f, 0.2f)
                );
            }

            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            Vector2 offset = new Vector2(0f, 0.24f);
            collider.offset = offset;
            collider.radius = 0.18f;
            collider.isTrigger = true;

            gameObject.AddComponent<Rigidbody2D>().isKinematic = true;

            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

            spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;

            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;

            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);

            m_GLNpc = npc;
        }

        public void UnInit()
        {

        }

        public void SetPosition(float fWorldX, float fWorldY)
        {
            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        virtual public void Update()
        {
            //m_DeltaTime = Time.time - m_LastRepresentFrameTime;
            //m_LastRepresentFrameTime = Time.time / 1000.0f;
            // 

            // 播放动画
            //m_Animation.ShowAnimation(m_nAni, m_Direction);
            // 
            int nIndex = (int)(Time.timeSinceLevelLoad * framesPerSecond);

            nIndex = nIndex % 2;

            spriteRenderer.sprite = m_sprites[nIndex];
        }

        void OnDrawGizmos()
        {
            DrawLogicPoint();
            DrawLogicCenterPoint();
            DrawNpcRange();
        }

        private void DrawLogicPoint()
        {
            float fRadius = 0.05f;
            float fTheta  = 0.1f;
            Color color   = Color.blue;

            if (transform == null)
                return;

            if (fTheta < 0.0001f)
                fTheta = 0.0001f;

            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // 设置颜色
            Color defaultColor = Gizmos.color;
            Gizmos.color = color;

            // 绘制圆环
            Vector3 beginPoint = Vector3.zero;                          // 每次画线的起始点
            Vector3 firstPoint = Vector3.zero;                          // 记录圆的第一个点

            for (float theta = 0; theta < 2 * Mathf.PI; theta += fTheta)
            {
                float x = fRadius * Mathf.Sin(theta);
                float y = fRadius * Mathf.Cos(theta);
                Vector3 endPoint = new Vector3(x, y, 0);                // 每次画线的结束点

                if (theta == 0)
                { // 画半径(这里不需要画)
                    firstPoint = endPoint;                              // 保存圆的第一个点
                    beginPoint = endPoint;                              // 下一次画线起始点
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

        private void DrawLogicCenterPoint()
        {
            float fRadius  = 0.05f;
            float fTheta   = 0.1f;
            float fYDiffer = (float)(m_GLNpc.LogicY - m_GLNpc.GetLogicCenterY()) / 100;
            Color color    = Color.red;

            if (transform == null)
                return;

            if (fTheta < 0.0001f)
                fTheta = 0.0001f;

            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            // 设置颜色
            Color defaultColor = Gizmos.color;
            Gizmos.color = color;

            // 绘制圆环
            Vector3 beginPoint = Vector3.zero;                          // 每次画线的起始点
            Vector3 firstPoint = new Vector3(0, fYDiffer, 0);           // 记录圆的第一个点

            for (float theta = 0; theta < 2 * Mathf.PI; theta += fTheta)
            {
                float x = fRadius * Mathf.Sin(theta);
                float y = fRadius * Mathf.Cos(theta) + fYDiffer;
                Vector3 endPoint = new Vector3(x, y, 0);                // 每次画线的结束点

                if (theta == 0)
                { // 画半径(这里不需要画)
                    firstPoint = endPoint;                              // 保存圆的第一个点
                    beginPoint = endPoint;                              // 下一次画线起始点
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

        private void DrawNpcRange()
        {
            float fCenterX = RepresentCommon.LogicX2WorldX((int)(m_GLNpc.GetLogicCenterX()));
            float fCenterY = RepresentCommon.LogicY2WorldY((int)(m_GLNpc.GetLogicCenterY()));

            int nLogicwidth = m_GLNpc.m_nWidth;
            int nLogicHeight = m_GLNpc.m_nHeight;

            float fWorldWidth = RepresentCommon.LogicDis2WorldDis(nLogicwidth);
            float fWorldHeight = RepresentCommon.LogicDis2WorldDis(nLogicHeight);

            Color VectorColor = Color.green;

            if (transform == null)
                return;

            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            //Gizmos.matrix = transform.localToWorldMatrix;

            // 设置颜色
            Color defaultColor = Gizmos.color;
            Gizmos.color = VectorColor;

            Gizmos.DrawWireCube(
                new Vector3(fCenterX, fCenterY, 0),
                new Vector3(fWorldWidth, fWorldHeight, 0));

            // 恢复默认颜色
            Gizmos.color = defaultColor;

            // 恢复默认矩阵
            Gizmos.matrix = defaultMatrix;
        }
    }
}
