using UnityEngine;
using System.Collections;
using Game;
using Game.Common;

public class Main : MonoBehaviour {
    public static Game.GameClient m_Client = new Game.GameClient();
    [SerializeField]
    public bool isOpenScreenUI = false;

	void Start ()
    {
        //Console.Init();
        m_Client.Init(isOpenScreenUI);
	}
	
	void FixedUpdate () {
        // 在此处理逻辑帧和绘制帧
        m_Client.Loop();
	}

    void OnDestroy()
    {
        //Console.UnInit();
    }
}
