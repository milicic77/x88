using UnityEngine;
using System.Collections;
using Game;
using Game.Common;

public class Main : MonoBehaviour {

    public static Game.GameClient m_Client = new Game.GameClient();
	void Start ()
    {
        Console.Init();
        m_Client.Init();
	}
	
	void FixedUpdate () {

        // 在此处理逻辑帧和绘制帧
        m_Client.Loop();
	}

    void OnDestroy()
    {
        Console.UnInit();
    }
}
