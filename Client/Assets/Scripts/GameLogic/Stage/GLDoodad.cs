using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GLDoodad
    {
        // 表现物件
        public RLDoodad m_RLDoodad = null;

        // 逻辑坐标
        public int m_nLogicX = 0;
        public int m_nLogicY = 0;

        public void Init(int nTemplateId, int nCellX, int nCellY, GLScene scene)
        {
            GLDoodadTemplate template = GLSettingManager.Instance().GetGLDoodadTemplate(nTemplateId);

            // 格子坐标 => 逻辑坐标
            int nLogicX = RepresentCommon.CellX2LogicX(nCellX);
            int nLogicY = RepresentCommon.CellY2LogicY(nCellY);

            // 受Doodad大小限制，需要调整逻辑坐标
            nLogicX += (template.nCellSizeX - 1) * (RepresentDef.SCENE_CELL_SIZE_PIXEL_X / 2);
            nLogicY += (template.nCellSizeY - 1) * (RepresentDef.SCENE_CELL_SIZE_PIXEL_Y / 2);

            // 逻辑坐标 => 世界坐标
            float fWorldX = RepresentCommon.LogicX2WorldX(nLogicX);
            float fWorldY = RepresentCommon.LogicY2WorldY(nLogicY);

            m_RLDoodad = Represent.Instance().CreateDoodad(template.nRepresentId, fWorldX, fWorldY);

            m_nLogicX = nLogicX;
            m_nLogicY = nLogicY;
        }

        public void UnInit()
        {

        }
    }
}
