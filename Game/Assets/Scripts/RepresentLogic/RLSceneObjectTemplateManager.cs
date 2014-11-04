using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;
using UnityEngine;

namespace Game.RepresentLogic
{
    public class RLSceneObjectTemplateManager : Common.Singleton<RLSceneObjectTemplateManager>
    {
        private Dictionary<int, RepresentSceneObjectConfig> m_SceneObjectCfgs = new Dictionary<int, RepresentSceneObjectConfig>();

        public void Init()
        {
            LoadTemplateList();
        }

        public void UnInit()
        {
            m_SceneObjectCfgs.Clear();
        }

        private void LoadTemplateList()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.SCENEOBJECT_REPRESENT_LIST_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RepresentSceneObjectConfig cfg = new RepresentSceneObjectConfig();

                    int nRepresentId = 0;
                    tabFile.GetInteger(i, "RepresentId", 0, ref nRepresentId);
                    cfg.nRepresentId = nRepresentId;

                    tabFile.GetString(i, "Name", "", ref cfg.szName);

                    tabFile.GetString(i, "Path", "", ref cfg.szPath);

                    //////////////////////////////////////////////////////////////////////////
                    string szFilePath = "setting/sceneobject/" + cfg.szPath + "/" + cfg.szName;

                    cfg.DefaultTexture = Resources.Load(szFilePath + "_defaulttexture") as Texture2D;

                    cfg.arrTexStandUp = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexStandDown = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexStandLeft = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexStandRight = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexRunUp = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexRunDown = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexRunLeft = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexRunRight = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexAttackUp = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexAttackDown = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexAttackLeft = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexAttackRight = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexHurtUp = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexHurtDown = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexHurtLeft = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    cfg.arrTexHurtRight = new Texture2D[SceneObjectDef.ANI_TEXTURE_COUNT];
                    string szTexFile;
                    for (int j = 1; j <= SceneObjectDef.ANI_TEXTURE_COUNT; ++j)
                    {
                        szTexFile = szFilePath + "_stand_up_" + j.ToString();
                        cfg.arrTexStandUp[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_stand_down_" + j.ToString();
                        cfg.arrTexStandDown[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_stand_left_" + j.ToString();
                        cfg.arrTexStandLeft[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_stand_right_" + j.ToString();
                        cfg.arrTexStandRight[j - 1] = Resources.Load(szTexFile) as Texture2D;


                        szTexFile = szFilePath + "_run_up_" + j.ToString();
                        cfg.arrTexRunUp[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_run_down_" + j.ToString();
                        cfg.arrTexRunDown[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_run_left_" + j.ToString();
                        cfg.arrTexRunLeft[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_run_right_" + j.ToString();
                        cfg.arrTexRunRight[j - 1] = Resources.Load(szTexFile) as Texture2D;


                        szTexFile = szFilePath + "_attack_up_" + j.ToString();
                        cfg.arrTexAttackUp[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_attack_down_" + j.ToString();
                        cfg.arrTexAttackDown[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_attack_left_" + j.ToString();
                        cfg.arrTexAttackLeft[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_attack_right_" + j.ToString();
                        cfg.arrTexAttackRight[j - 1] = Resources.Load(szTexFile) as Texture2D;


                        szTexFile = szFilePath + "_hurt_up_" + j.ToString();
                        cfg.arrTexHurtUp[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_hurt_down_" + j.ToString();
                        cfg.arrTexHurtDown[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_hurt_left_" + j.ToString();
                        cfg.arrTexHurtLeft[j - 1] = Resources.Load(szTexFile) as Texture2D;

                        szTexFile = szFilePath + "_hurt_right_" + j.ToString();
                        cfg.arrTexHurtRight[j - 1] = Resources.Load(szTexFile) as Texture2D;

                    }
                    //////////////////////////////////////////////////////////////////////////

                    m_SceneObjectCfgs[cfg.nRepresentId] = cfg;
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
                    m_SceneObjectCfgs.Clear();
                }
            }
        }

        public RepresentSceneObjectConfig GetSceneObjectTemplateConfig(int nRepresent)
        {
            if (m_SceneObjectCfgs.ContainsKey(nRepresent))
            {
                return m_SceneObjectCfgs[nRepresent];
            }
            return null;
        }
    }
}
