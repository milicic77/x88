using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.RepresentLogic
{
    // 场景层
    class RLLayer : MonoBehaviour
    {
        public void Init(string szImage, int nOrder)
        {
            // 贴图
            Texture2D texture = Resources.Load(szImage) as Texture2D;

            // 精灵
            Sprite sprite = Sprite.Create(
                texture, 
                new Rect(0, 0, RepresentDef.SCENE_SIZE_PIXEL_X, RepresentDef.SCENE_SIZE_PIXEL_Y), 
                new Vector2(0.5f, 0.5f)
            );

            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = nOrder;

            // TODO 资源问题临时这样做，中间层向下移80个像素
            if (nOrder == (int)RLSceneObjectOrder.RLSceneOrder_MiddleLayer)
            {
                float fDiffY = (float)RepresentDef.SCENE_CELL_SIZE_PIXEL_Y / (float)RepresentDef.PIXEL_UNITY_SCALE;
                gameObject.transform.position = new Vector3(0, 0 - fDiffY, 0);
            }
            else
            {
                gameObject.transform.position = new Vector3(0, 0, 0);
            }

        }

        public void UnInit()
        {

        }
    }
}
