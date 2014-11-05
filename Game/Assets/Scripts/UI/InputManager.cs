using UnityEngine;
using Game.GameLogic;
using Game.RepresentLogic;

public class InputManager : MonoBehaviour
{
    Camera    m_SceneCamera = null;
    GameWorld m_GameWorld   = null;
    void Start ()
    {
        m_SceneCamera = RepresentEnv.SceneCamera;
        m_GameWorld   = GameWorld.Instance();
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 屏幕坐标 -> 世界坐标
            Vector3 position = m_SceneCamera.ScreenToWorldPoint(Input.mousePosition);
            // 世界坐标 -> 逻辑坐标
            int x = RepresentCommon.WorldX2LogicX(position.x * 100);
            int y = RepresentCommon.WorldY2LogicY(position.y * 100);
            Debug.Log(position.x);
            Debug.Log(position.y);
            Debug.Log(x);
            Debug.Log(y);
            PlantNpc(10, x, y);
        }
    }

    void PlantNpc(int nTemplateId, int nLogicX, int nLogicY)
    {
        // 判断是否是合法位置

        GLNpc npc = m_GameWorld.m_Stage.AddNpc(nTemplateId, nLogicX, nLogicY);
        npc.SetDoing((int)SceneObjectAni.SceneObjectAni_Stand);
        npc.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Right);
    }
}
