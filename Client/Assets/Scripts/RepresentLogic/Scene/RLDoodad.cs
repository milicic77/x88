using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common;

namespace Game.RepresentLogic
{
    public class RLDoodad : MonoBehaviour
    {
        // 表现模板
        public RLDoodadTemplate m_Template;

        public void Init(int nRepresentId, float fWorldX, float fWorldY, int nOrder)
        {
            RLDoodadTemplate template = RLResourceManager.Instance().GetRLDoodadTemplate(nRepresentId);
            if (template == null)
            {
                ExceptionTool.ThrowException("nRepresentId不合法！");
            }

            m_Template = template;

            // 精灵
            Sprite sprite = Sprite.Create(template.texture,
                new Rect(0, 0, template.texture.width, template.texture.height), 
                new Vector2(template.fPivotX, template.fPivotY)
            );

            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;

            gameObject.transform.position = new Vector3(fWorldX, fWorldY, 0);
        }

        virtual public void Update()
        {

        }
    }
}
