using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;
using UnityEngine;

namespace Game.RepresentLogic
{
    public class RLSceneTemplateManager : Common.Singleton<RLSceneTemplateManager>
    {
        private Dictionary<int, RepresentSceneConfig> m_SceneCfgs = new Dictionary<int, RepresentSceneConfig>();

        public void Init()
        {
            LoadTemplateList();
        }

        public void UnInit()
        {
            m_SceneCfgs.Clear();
        }

        private void LoadTemplateList()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.SCENE_REPRESENT_LIST_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RepresentSceneConfig cfg = new RepresentSceneConfig();

                    int nTemplateId = 0;
                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemplateId);
                    cfg.nTemplateId = nTemplateId;

                    tabFile.GetString(i, "Name", "", ref cfg.szName);

                    tabFile.GetString(i, "Path", "", ref cfg.szPath);

                    cfg.szBackGroundImage = "setting/scene/" + cfg.szPath + "/bg";
                    cfg.szMiddleGroundImage = "setting/scene/" + cfg.szPath + "/mg";

                    cfg.nCellType = new int[RepresentDef.SCENE_CELL_MAX_X, RepresentDef.SCENE_CELL_MAX_Y];

                    //////////////////////////////////////////////////////////////////////////
                    string szCellFile = "setting/scene/" + cfg.szPath + "/cell";
                    Common.TableFile SubTabFile = Common.TableFile.LoadFromFile(szCellFile);
                    int nSubRowCount = SubTabFile.GetRowsCount();
                    for (int j = 1; j <= nSubRowCount; j++)
                    {
                        int nY = j - 1;
                        for (int k = 0; k < 20; ++k)
                        {
                            int nType = (int)SceneCellType.SceneCellType_None;
                            string szKey = "x" + k.ToString();
                            SubTabFile.GetInteger(
                                j, 
                                szKey, 
                                (int)SceneCellType.SceneCellType_None, 
                                ref nType
                            );

                            cfg.nCellType[k, nY] = nType;
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////

                    m_SceneCfgs[cfg.nTemplateId] = cfg;
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
                    m_SceneCfgs.Clear();
                }
            }
        }

        public RepresentSceneConfig GetSceneTemplateConfig(int nTemplateId)
        {
            if (m_SceneCfgs.ContainsKey(nTemplateId))
            {
                return m_SceneCfgs[nTemplateId];
            }
            return null;
        }
    }
}
