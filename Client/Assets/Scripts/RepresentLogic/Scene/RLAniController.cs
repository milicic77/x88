using UnityEngine;
using System.Collections;

public class RLAniController : MonoBehaviour
{
    private Sprite[]       m_SpriteAnimation = null;                    // 攻击动画精灵
    private SpriteRenderer m_SpriteRenderer  = null;                    // 动画渲染
    public  float          m_FramesPerSecond = 10;                      // 动画帧速率

    public Sprite[] SpriteAnimation
    {
        get { return m_SpriteAnimation;  }
        set { m_SpriteAnimation = value; }
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return m_SpriteRenderer;  }
        set { m_SpriteRenderer = value; }
    }

    public float FramesPerSecond
    {
        get { return m_FramesPerSecond;  }
        set { m_FramesPerSecond = value; }
    }

    void Update ()
    {
        if (null == m_SpriteAnimation)
        {
            return;
        }

        if (null == m_SpriteRenderer)
        {
            return;
        }

        if (m_FramesPerSecond <= 0)
        {
            return;
        }

        int nIndex = (int)(Time.timeSinceLevelLoad * m_FramesPerSecond);
        nIndex = nIndex % m_SpriteAnimation.Length;
        m_SpriteRenderer.sprite = m_SpriteAnimation[nIndex];
    }
}
