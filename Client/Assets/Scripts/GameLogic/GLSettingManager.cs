using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;
using Game.RepresentLogic;

namespace Game.GameLogic
{
    public class GLSettingManager : Common.Singleton<GLSettingManager>
    {
        //////////////////////////////////////////////////////////////////////////
        // 关卡配置模板
        private Dictionary<int, GLStageTemplate> m_StageTemplateList = new Dictionary<int, GLStageTemplate>();
        // 逻辑场景配置模板
        private Dictionary<int, GLSceneTemplate> m_GLSceneTemplateList = new Dictionary<int, GLSceneTemplate>();
        // 逻辑场景物件配置模板
        private Dictionary<int, GLDoodadTemplate> m_GLDoodadTemplateList = new Dictionary<int, GLDoodadTemplate>();
        // 逻辑场景NPC配置模板
        private Dictionary<int, GLNpcTemplate> m_GLNpcTemplateList = new Dictionary<int, GLNpcTemplate>();
        // 逻辑场景萝卜配置模板
        private Dictionary<int, GLRadishTemplate> m_GLRadishTemplateList = new Dictionary<int, GLRadishTemplate>();
        // 逻辑场景特效配置模板
        private Dictionary<int, GLEffectTemplate> m_GLEffectTemplateList = new Dictionary<int, GLEffectTemplate>();
        // 逻辑场景炮塔配置模板
        private Dictionary<int, GLTowerTemplate> m_GLTowerTemplateList = new Dictionary<int, GLTowerTemplate>();
        // 逻辑场景路径配置模板
        private Dictionary<int, GLPath> m_GLPathList = new Dictionary<int, GLPath>();
        //////////////////////////////////////////////////////////////////////////

        public void Init()
        {
            // 加载关卡配置模板
            LoadStageTemplate();

            // 加载逻辑场景配置模板
            LoadGLSceneTemplate();

            // 加载逻辑场景物件配置模板
            LoadGLDoodadTemplate();

            // 加载逻辑场景NPC配置模板
            LoadGLNpcTemplate();

            // 加载逻辑场景萝卜配置模板
            LoadGLRadishTemplate();

            // 加载逻辑场景特效配置模板
            LoadGLEffectTemplate();

            // 加载逻辑炮塔配置模板
            LoadGLTowerTemplate();

            // 加载路径
            LoadGLPathList();
        }

        public void UnInit()
        {
            m_GLSceneTemplateList.Clear();
            m_StageTemplateList.Clear();
            m_GLDoodadTemplateList.Clear();
            m_GLNpcTemplateList.Clear();
            m_GLRadishTemplateList.Clear();
            m_GLTowerTemplateList.Clear();
            m_GLEffectTemplateList.Clear();
            m_GLPathList.Clear();
        }

        //////////////////////////////////////////////////////////////////////////
        // 加载关卡配置模板
        public void LoadStageTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_STAGE_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLStageTemplate template = new GLStageTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    tabFile.GetInteger(i, "SceneId", 0, ref nTemp);
                    template.nSceneId = nTemp;

                    tabFile.GetInteger(i, "NpcGroupInterval", 0, ref nTemp);
                    template.nNpcGroupInterval = nTemp;

                    //////////////////////////////////////////////////////////////////////////
                    // 加载Doodad配置
                    string szDoodadPath = null;
                    tabFile.GetString(i, "Doodad", "", ref szTemp);
                    szDoodadPath = GameLogicDef.LOGIC_STAGE_DOODAD_PATH + szTemp;
                    Common.TableFile DoodadTabFile = Common.TableFile.LoadFromFile(szDoodadPath);
                    int nDoodadCount = DoodadTabFile.GetRowsCount();
                    for (int j = 1; j <= nDoodadCount; j++)
                    {
                        // Doodad的逻辑模板ID
                        int nDoodadId = 0;
                        DoodadTabFile.GetInteger(j, "DoodadId", 0, ref nDoodadId);
                        // Doodad的格子位置X
                        int nDoodadCellX = 0;
                        DoodadTabFile.GetInteger(j, "CellX", 0, ref nDoodadCellX);
                        // Doodad的格子位置Y
                        int nDoodadCellY = 0;
                        DoodadTabFile.GetInteger(j, "CellY", 0, ref nDoodadCellY);

                        template.asDoodad.Add(new GLDoodadPos(nDoodadId, nDoodadCellX, nDoodadCellY));
                    }
                    //////////////////////////////////////////////////////////////////////////
                    // 加载炮塔配置
                    string szTowerPath = null;
                    tabFile.GetString(i, "Tower", "", ref szTemp);
                    szTowerPath = GameLogicDef.LOGIC_STAGE_TOWER_PATH + szTemp;
                    Common.TableFile TowerTabFile = Common.TableFile.LoadFromFile(szTowerPath);
                    int nTowerCount = TowerTabFile.GetRowsCount();
                    for (int j2 = 1; j2 <= nTowerCount; j2++)
                    {
                        // Tower的逻辑模板ID
                        int nTowerId = 0;
                        TowerTabFile.GetInteger(j2, "TowerId", 0, ref nTowerId);
                        // Tower的格子位置X
                        int nTowerCellX = 0;
                        TowerTabFile.GetInteger(j2, "CellX", 0, ref nTowerCellX);
                        // Tower的格子位置Y
                        int nTowerCellY = 0;
                        TowerTabFile.GetInteger(j2, "CellY", 0, ref nTowerCellY);

                        template.asTower.Add(new GLTowerPos(nTowerId, nTowerCellX, nTowerCellY));
                    }
                    //////////////////////////////////////////////////////////////////////////
                    // 加载Npc配置
                    string szNpcPath = null;
                    tabFile.GetString(i, "Npc", "", ref szTemp);
                    szNpcPath = GameLogicDef.LOGIC_STAGE_DOODAD_NPC + szTemp;
                    Common.TableFile NpcTabFile = Common.TableFile.LoadFromFile(szNpcPath);
                    // 波数
                    int nCount = NpcTabFile.GetRowsCount();
                    for (int k = 1; k <= nCount; k++)
                    {
                        GLStageNpcConfig stagenpc = new GLStageNpcConfig();

                        NpcTabFile.GetInteger(k, "PathId", 0, ref stagenpc.nPathId);
                        NpcTabFile.GetInteger(k, "Interval", 0, ref stagenpc.nInterval);

                        for (int nNpcIdx = 1; nNpcIdx <= 30; ++nNpcIdx)
                        {
                            string szKey = "Npc" + nNpcIdx.ToString();
                            int nNpcTemplateId = 0;
                            NpcTabFile.GetInteger(k, szKey, 0, ref nNpcTemplateId);

                            if (nNpcTemplateId > 0)
                            {
                                stagenpc.asNpc.Add(nNpcTemplateId);
                            }
                        }

                        template.asNpc.Add(stagenpc);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    // 萝卜配置
                    tabFile.GetString(i, "Radish", "", ref szTemp);
                    string[] szParam = szTemp.Split(new char[] { ',' });
                    template.nRadishTemplateId = Convert.ToInt32(szParam[0]);
                    template.nRadishCellX = Convert.ToInt32(szParam[1]);
                    template.nRadishCellY = Convert.ToInt32(szParam[2]);
                    //////////////////////////////////////////////////////////////////////////
                    m_StageTemplateList[template.nTemplateId] = template;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_StageTemplateList.Clear();
                }
            }
        }

        // 获取关卡配置模板
        public GLStageTemplate GetStageTemplate(int nTemplateId)
        {
            if (m_StageTemplateList.ContainsKey(nTemplateId))
            {
                return m_StageTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////

        // 加载逻辑场景配置模板
        public void LoadGLSceneTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_SCENE_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLSceneTemplate template = new GLSceneTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    //////////////////////////////////////////////////////////////////////////
                    // 加载格子类型配置
                    string szCellTypeFile = null;
                    tabFile.GetString(i, "CellType", "", ref szTemp);
                    szCellTypeFile = GameLogicDef.LOGIC_SCENE_CELLTYPE_PATH + szTemp;
                    Common.TableFile TabCellTypeFile = Common.TableFile.LoadFromFile(szCellTypeFile);
                    for (int nCellY = 0; nCellY < RepresentDef.SCENE_CELL_COUNT_Y; ++nCellY)
                    {
                        for (int nCellX = 0; nCellX < RepresentDef.SCENE_CELL_COUNT_X; ++nCellX)
                        {
                            string szKey = "x" + nCellX.ToString();
                            TabCellTypeFile.GetInteger(nCellY, szKey, 0, ref nTemp);
                            template.arrCellType[nCellX, nCellY] = nTemp;
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////

                    m_GLSceneTemplateList[template.nTemplateId] = template;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_GLSceneTemplateList.Clear();
                }
            }
        }

        // 获取逻辑场景配置模板
        public GLSceneTemplate GetGLSceneTemplate(int nTemplateId)
        {
            if (m_GLSceneTemplateList.ContainsKey(nTemplateId))
            {
                return m_GLSceneTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载场景物件配置模板
        public void LoadGLDoodadTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_DOODAD_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLDoodadTemplate template = new GLDoodadTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetInteger(i, "CellSizeX", 0, ref nTemp);
                    template.nCellSizeX = nTemp;

                    tabFile.GetInteger(i, "CellSizeY", 0, ref nTemp);
                    template.nCellSizeY = nTemp;

                    m_GLDoodadTemplateList[template.nTemplateId] = template;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_GLDoodadTemplateList.Clear();
                }
            }
        }

        // 获取场景物件配置模板
        public GLDoodadTemplate GetGLDoodadTemplate(int nTemplateId)
        {
            if (m_GLDoodadTemplateList.ContainsKey(nTemplateId))
            {
                return m_GLDoodadTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载逻辑场景NPC模板
        public void LoadGLNpcTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_NPC_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLNpcTemplate template = new GLNpcTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetInteger(i, "Width", 0, ref nTemp);
                    template.nWidth = nTemp;

                    tabFile.GetInteger(i, "Height", 0, ref nTemp);
                    template.nHeight = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    m_GLNpcTemplateList[template.nTemplateId] = template;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_GLNpcTemplateList.Clear();
                }
            }
        }
        // 获取逻辑场景NPC模板
        public GLNpcTemplate GetGLNpcTemplate(int nTemplateId)
        {
            if (m_GLNpcTemplateList.ContainsKey(nTemplateId))
            {
                return m_GLNpcTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载逻辑场景萝卜模板
        public void LoadGLRadishTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_RADISH_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLRadishTemplate template = new GLRadishTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetInteger(i, "Life", 0, ref nTemp);
                    template.nLife = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    m_GLRadishTemplateList[template.nTemplateId] = template;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_GLRadishTemplateList.Clear();
                }
            }
        }
        // 获取逻辑场景萝卜模板
        public GLRadishTemplate GetGLRadishTemplate(int nTemplateId)
        {
            if (m_GLRadishTemplateList.ContainsKey(nTemplateId))
            {
                return m_GLRadishTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载逻辑场景特效模板
        public void LoadGLEffectTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_EFFECT_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLEffectTemplate template = new GLEffectTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    m_GLEffectTemplateList[template.nTemplateId] = template;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_GLEffectTemplateList.Clear();
                }
            }
        }
        // 获取逻辑场景萝卜模板
        public GLEffectTemplate GetGLEffectTemplate(int nTemplateId)
        {
            if (m_GLEffectTemplateList.ContainsKey(nTemplateId))
            {
                return m_GLEffectTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载逻辑场景炮塔模板
        public void LoadGLTowerTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_TOWER_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLTowerTemplate template = new GLTowerTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    int nRepresentId = 0;
                    tabFile.GetInteger(i, "RepresentId", 0, ref nRepresentId);
                    template.nRepresentId = nRepresentId;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    m_GLTowerTemplateList[template.nTemplateId] = template;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_GLTowerTemplateList.Clear();
                }
            }
        }
        // 获取逻辑场景炮塔模板
        public GLTowerTemplate GetGLTowerTemplate(int nTemplateId)
        {
            if (m_GLTowerTemplateList.ContainsKey(nTemplateId))
            {
                return m_GLTowerTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载路径
        public void LoadGLPathList()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.LOGIC_PATH_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLPath path = new GLPath();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "PathId", 0, ref nTemp);
                    int nPathId = nTemp;

                    for (int j = 1; j <= 30; ++j )
                    {
                        string szKey = "Point" + j.ToString();
                        tabFile.GetString(i, szKey, "", ref szTemp);

                        if (szTemp == null || szTemp == "")
                            break;

                        string[] szPoints = szTemp.Split(new char[] { ',' });
                        int nCellX = Convert.ToInt32(szPoints[0]);
                        int nCellY = Convert.ToInt32(szPoints[1]);

                        path.m_PointList.Add(new GLPoint(nCellX, nCellY));
                    }

                    m_GLPathList[nPathId] = path;
                }
            }
            catch (Exception e)
            {
                success = false;
                Common.ExceptionTool.ProcessException(e);
            }
            finally
            {
                if (!success)
                {
                    m_GLPathList.Clear();
                }
            }
        }
        // 获取路径
        public GLPath GetGLPath(int nTemplateId)
        {
            if (m_GLPathList.ContainsKey(nTemplateId))
            {
                return m_GLPathList[nTemplateId];
            }
            return null;
        }
    }
}
