using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.RepresentLogic
{
    public class RLSceneObjectAnimationCtrl
    {
        // 场景对象
        RLSceneObject m_SceneObject;
        // 动画精灵帧
        public Sprite[, ,] m_AniSprites;

        public float framesPerSecond = 5;

        public void Init(RLSceneObject SceneObject, ref RepresentSceneObjectConfig cfg)
        {
            m_AniSprites = new Sprite[(int)SceneObjectAni.SceneObjectAni_Count - 1, (int)SceneObjectDirection.SceneObjectDirection_Count - 1, SceneObjectDef.ANI_TEXTURE_COUNT];
            m_SceneObject = SceneObject;
            //////////////////////////////////////////////////////////////////////////
            // 站立向上
            int nAniType = (int)SceneObjectAni.SceneObjectAni_Stand - 1;
            int nDirection = (int)SceneObjectDirection.SceneObjectDirection_Up - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexStandUp[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 站立向下
            nAniType = (int)SceneObjectAni.SceneObjectAni_Stand - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Down - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexStandDown[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 站立向左
            nAniType = (int)SceneObjectAni.SceneObjectAni_Stand - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Left - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexStandLeft[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 站立向右
            nAniType = (int)SceneObjectAni.SceneObjectAni_Stand - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Right - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexStandRight[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 移动向上
            nAniType = (int)SceneObjectAni.SceneObjectAni_Run - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Up - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexRunUp[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 移动向下
            nAniType = (int)SceneObjectAni.SceneObjectAni_Run - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Down - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexRunDown[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 移动向左
            nAniType = (int)SceneObjectAni.SceneObjectAni_Run - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Left - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexRunLeft[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 移动向右
            nAniType = (int)SceneObjectAni.SceneObjectAni_Run - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Right - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexRunRight[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 攻击向上
            nAniType = (int)SceneObjectAni.SceneObjectAni_Attack - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Up - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexAttackUp[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 攻击向下
            nAniType = (int)SceneObjectAni.SceneObjectAni_Attack - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Down - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexAttackDown[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 攻击向左
            nAniType = (int)SceneObjectAni.SceneObjectAni_Attack - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Left - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexAttackLeft[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 攻击向右
            nAniType = (int)SceneObjectAni.SceneObjectAni_Attack - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Right - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexAttackRight[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 受伤向上
            nAniType = (int)SceneObjectAni.SceneObjectAni_Hurt - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Up - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexHurtUp[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 受伤向下
            nAniType = (int)SceneObjectAni.SceneObjectAni_Hurt - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Down - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexHurtDown[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 受伤向左
            nAniType = (int)SceneObjectAni.SceneObjectAni_Hurt - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Left - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexHurtLeft[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }
            //////////////////////////////////////////////////////////////////////////
            // 受伤向右
            nAniType = (int)SceneObjectAni.SceneObjectAni_Hurt - 1;
            nDirection = (int)SceneObjectDirection.SceneObjectDirection_Right - 1;
            for (int i = 0; i < SceneObjectDef.ANI_TEXTURE_COUNT; ++i)
            {
                m_AniSprites[nAniType, nDirection, i] = Sprite.Create(
                    cfg.arrTexHurtRight[i],
                    new Rect(
                        0, 0,
                        RepresentDef.SCENE_OBJECT_PIXEL_X,
                        RepresentDef.SCENE_OBJECT_PIXEL_Y
                    ),
                    new Vector2(0.5f, 0.5f)
                );
            }

        }

        public void ShowAnimation(int nAni, int nDirection)
        {
            int nIndex = (int)(Time.timeSinceLevelLoad * framesPerSecond);

            nIndex = nIndex % SceneObjectDef.ANI_TEXTURE_COUNT;

            m_SceneObject.SceneObjectSpriteRenderer.sprite = m_AniSprites[nAni - 1, nDirection - 1, nIndex];
        }

    }
}
