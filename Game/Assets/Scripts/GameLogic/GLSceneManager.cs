using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameLogic
{
    public class GLSceneManager : Common.Singleton<GLSceneManager>
    {
        private Dictionary<int, GLSceneConfig> m_SceneCfgs = new Dictionary<int, GLSceneConfig>();

        //private GLScene m_CurScene;

        public void Init()
        {
            LoadSceneList();
        }

        public void UnInit()
        {
            m_SceneCfgs.Clear();
        }

        //public void Activate()
        //{
        //    if (m_CurScene != null)
        //    {
        //        m_CurScene.Activate();
        //    }
        //}
        public void LoadSceneList()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.SCENE_LIST_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLSceneConfig cfg = new GLSceneConfig();

                    int nSceneId = 0;
                    tabFile.GetInteger(i, "SceneId", 0, ref nSceneId);
                    cfg.nSceneId = nSceneId;

                    tabFile.GetString(i, "Name", "", ref cfg.szName);

                    int nTemplateId = 0;
                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemplateId);
                    cfg.nTemplateId = nTemplateId;

                    for (int j = 1; j <= 5; ++j)
                    {
                        string szFileName = null;
                        string szKey = "ScenePath" + i.ToString();
                        tabFile.GetString(i, szKey, "", ref szFileName);

                        if (szFileName == null || szFileName == "")
                            break;

                        string szFilePath = "setting/scene/scenepath/" + szFileName;

                        Common.TableFile SubTabFile = Common.TableFile.LoadFromFile(szFilePath);
                        int rowCount2 = SubTabFile.GetRowsCount();
                        cfg.ScenePathList[j - 1] = new GLScenePath();
                        for (int k = 1; k <= rowCount2; ++k)
                        {
                            int nTemp = 0;
                            
                            GLScenePoint point = new GLScenePoint();

                            SubTabFile.GetInteger(k, "x", 0, ref nTemp);
                            point.nX = nTemp;

                            SubTabFile.GetInteger(k, "y", 0, ref nTemp);
                            point.nY = nTemp;

                            if (k == 1) // 起点
                            {
                                cfg.ScenePathList[j - 1].m_Start = point;
                            }
                            if (k == rowCount2) // 终点
                            {
                                cfg.ScenePathList[j - 1].m_End = point;
                            }

                            cfg.ScenePathList[j - 1].m_PointList.Add(point);
                        }
                    }

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

        public GLSceneConfig GetSceneConfig(int nTemplateId)
        {
            if (m_SceneCfgs.ContainsKey(nTemplateId))
            {
                return m_SceneCfgs[nTemplateId];
            }
            return null;
        }

        //public GLScene CreateScene(int nSceneId)
        //{
        //    GLSceneConfig cfg = GetSceneConfig(nSceneId);

        //    m_CurScene = new GLScene();
        //    m_CurScene.Init(ref cfg);

        //    return m_CurScene;
        //}
    }
}
