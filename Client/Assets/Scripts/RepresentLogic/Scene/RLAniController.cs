using UnityEngine;
using System.Collections;

public interface IRLAniController
{
    void Play();
    void Update();
}

public abstract class ARLAniController : IRLAniController
{
    protected Sprite[]       m_SpriteAnimation = null;                    // 攻击动画精灵
    protected SpriteRenderer m_SpriteRenderer  = null;                    // 动画渲染
    protected float          m_FramesPerSecond = 10;                      // 动画帧速率

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

    public virtual void Update ()
    {
    }

    public virtual void Play()
    {
    }
}

public class RLAniController : ARLAniController
{
    public override void Update ()
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

    public override void Play()
    {
    }
}

public class RLSingleAniController : ARLAniController
{
    protected bool m_bPlaying      = false;
    protected int  m_nIndex        = 0;
    protected int  m_nLastPlayTime = 0;

    public override void Update()
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

        if (!m_bPlaying)
        {
            return;
        }

        int nCurTime  = (int)(Time.time * 1000);                          // ms
        int nTimeSpan = nCurTime - m_nLastPlayTime;

        if (nTimeSpan * m_FramesPerSecond < 1000)
        { // 播放间隔未到
            return;
        }

        m_nLastPlayTime = nCurTime;

        m_nIndex++;
        m_nIndex = m_nIndex % m_SpriteAnimation.Length;

        m_SpriteRenderer.sprite = m_SpriteAnimation[m_nIndex];

        if (m_nIndex <= 0)
        { // 播放完毕
            m_nIndex   = 0;
            m_bPlaying = false;
        }
    }

    public override void Play()
    {
        if (m_bPlaying)
        { // 当前动画正在播放
            return;
        }

        m_bPlaying = true;
        m_nIndex   = 0;
    }
}
