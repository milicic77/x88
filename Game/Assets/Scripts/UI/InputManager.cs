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

            PlantTurret(10, x, y);
        }
    }

    void PlantTurret(int nTemplateId, int nLogicX, int nLogicY)
    {
        // 判断是否是合法位置

        GLTurret turret = m_GameWorld.m_Stage.AddTurret(nTemplateId, nLogicX, nLogicY);
        turret.SetDoing((int)SceneObjectAni.SceneObjectAni_Stand);
        turret.SetDirection((int)SceneObjectDirection.SceneObjectDirection_Right);
    }
}
