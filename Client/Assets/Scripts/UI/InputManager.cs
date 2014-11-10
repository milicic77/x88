using UnityEngine;
using Game.GameLogic;
using Game.RepresentLogic;
using Game.Common;

public class InputManager : MonoBehaviour
{
    Camera    m_SceneCamera = null;
    GameWorld m_GameWorld   = null;
    void Start ()
    {
        m_SceneCamera = RepresentEnv.MainCamera;
        m_GameWorld   = GameWorld.Instance();
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 屏幕坐标 -> 世界坐标
            Vector3 position = m_SceneCamera.ScreenToWorldPoint(Input.mousePosition);
            // 世界坐标 -> 逻辑坐标
            int lx = RepresentCommon.WorldX2LogicX(position.x);
            int ly = RepresentCommon.WorldY2LogicY(position.y);

            Console.Write(lx.ToString());
            Console.Write(ly.ToString());

            int cx = RepresentCommon.LogicX2CellX(lx);
            int cy = RepresentCommon.LogicY2CellY(ly);
            Console.Write(cx.ToString());
            Console.Write(cy.ToString());

            PlantTower(1, cx, cy);
        }
    }

    void PlantTower(int nTemplateId, int nCellX, int nCellY)
    {
        // 判断是否是合法位置
        GLTower tower = new GLTower();
        tower.Init(nTemplateId, nCellX, nCellY, m_GameWorld.Stage.Scene);
        m_GameWorld.Stage.Scene.AddTower(tower);
        m_GameWorld.Stage.TowerList.Add(tower);
    }
}
