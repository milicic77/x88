using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;
using UnityEngine;

namespace Game.RepresentLogic
{
    public class RLResourceManager : Common.Singleton<RLResourceManager>
    {
        // 表现场景模板
        private Dictionary<int, RLSceneTemplate> m_RLSceneTemplateList = new Dictionary<int, RLSceneTemplate>();

        // 表现物品模板
        private Dictionary<int, RLDoodadTemplate> m_RLDoodadTemplateList = new Dictionary<int, RLDoodadTemplate>();

        // 表现怪物模板
        private Dictionary<int, RLNpcTemplate> m_RLNpcTemplateList = new Dictionary<int, RLNpcTemplate>();

        // 表现萝卜模板
        private Dictionary<int, RLRadishTemplate> m_RLRadishTemplateList = new Dictionary<int, RLRadishTemplate>();

        // 表现炮塔模板
        private Dictionary<int, RLTowerTemplate> m_RLTowerTemplateList = new Dictionary<int, RLTowerTemplate>();

        // 表现子弹模板
        private Dictionary<int, RLMissileTemplate> m_RLMissileTemplateList = new Dictionary<int, RLMissileTemplate>();
        
        // 表现特效模板
        private Dictionary<int, RLEffectTemplate> m_RLEffectTemplateList = new Dictionary<int, RLEffectTemplate>();

        // UI配置
        private Dictionary<int, RLUISetting> m_RLUISettingList = new Dictionary<int, RLUISetting>();

        public void Init()
        {
            // 加载表现场景模板
            LoadRLSceneTemplate();

            // 加载表现物品模板
            LoadRLDoodadTemplate();

            // 加载表现怪物模板
            LoadRLNpcTemplate();

            // 加载表现萝卜模板
            LoadRLRadishTemplate();

            // 加载表现特效模板
            LoadRLEffectTemplate();

            // 加载表现炮塔模板
            LoadRLTowerTemplate();

            // 加载表现子弹模板
            LoadRLMissileTemplate();

            // 加载UI配置
            LoadRLUISetting();
        }

        public void UnInit()
        {
            m_RLSceneTemplateList.Clear();
            m_RLDoodadTemplateList.Clear();
            m_RLNpcTemplateList.Clear();
            m_RLRadishTemplateList.Clear();
            m_RLTowerTemplateList.Clear();
            m_RLEffectTemplateList.Clear();
            m_RLMissileTemplateList.Clear();
        }

        //////////////////////////////////////////////////////////////////////////
        // 表现场景模板
        private void LoadRLSceneTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.SCENE_REPRESENT_TEMPLATE_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLSceneTemplate template = new RLSceneTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemp);
                    template.nTemplateId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    tabFile.GetString(i, "BackGround", "", ref szTemp);
                    template.szBackGroundImage = SceneDef.SCENE_REPRESENT_BG_PATH + szTemp;

                    tabFile.GetString(i, "MiddleGround", "", ref szTemp);
                    template.szMiddleGroundImage = SceneDef.SCENE_REPRESENT_MG_PATH + szTemp;

                    tabFile.GetString(i, "ForceGround", "", ref szTemp);
                    template.szForceGroundImage = SceneDef.SCENE_REPRESENT_FG_PATH + szTemp;

                    m_RLSceneTemplateList[template.nTemplateId] = template;
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
                    m_RLSceneTemplateList.Clear();
                }
            }
        }

        // 获取场景模板
        public RLSceneTemplate GetRLSceneTemplate(int nTemplateId)
        {
            if (m_RLSceneTemplateList.ContainsKey(nTemplateId))
            {
                return m_RLSceneTemplateList[nTemplateId];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载表现物品模板
        private void LoadRLDoodadTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.DOODAD_REPRESENT_TEMPLATE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLDoodadTemplate template = new RLDoodadTemplate();

                    int nTemp = 0;
                    string szTemp = null;
                    float fTemp = 0.0f;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    tabFile.GetString(i, "Image", "", ref szTemp);
                    template.szImage = SceneDef.DOODAD_REPRESENT_TEXTURE_PATH + szTemp;

                    template.texture = Resources.Load(template.szImage) as Texture2D;

                    tabFile.GetFloat(i, "PivotX", 0.0f, ref fTemp);
                    template.fPivotX = fTemp;

                    tabFile.GetFloat(i, "PivotY", 0.0f, ref fTemp);
                    template.fPivotY = fTemp;

                    m_RLDoodadTemplateList[template.nRepresentId] = template;
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
                    m_RLDoodadTemplateList.Clear();
                }
            }
        }

        // 获取表现物品模板
        public RLDoodadTemplate GetRLDoodadTemplate(int nRepresent)
        {
            if (m_RLDoodadTemplateList.ContainsKey(nRepresent))
            {
                return m_RLDoodadTemplateList[nRepresent];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载怪物表现模板
        public void LoadRLNpcTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.NPC_REPRESENT_TEMPLATE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLNpcTemplate template = new RLNpcTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    tabFile.GetString(i, "Ani_1", "", ref szTemp);
                    template.TexAni[0] = Resources.Load(SceneDef.NPC_REPRESENT_TEXTURE_PATH + szTemp) as Texture2D;

                    tabFile.GetString(i, "Ani_2", "", ref szTemp);
                    template.TexAni[1] = Resources.Load(SceneDef.NPC_REPRESENT_TEXTURE_PATH + szTemp) as Texture2D;

                    m_RLNpcTemplateList[template.nRepresentId] = template;
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
                    m_RLNpcTemplateList.Clear();
                }
            }
        }

        // 获取表现怪物模板
        public RLNpcTemplate GetRLNpcTemplate(int nRepresent)
        {
            if (m_RLNpcTemplateList.ContainsKey(nRepresent))
            {
                return m_RLNpcTemplateList[nRepresent];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载萝卜表现模板
        public void LoadRLRadishTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.RADISH_REPRESENT_TEMPLATE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLRadishTemplate template = new RLRadishTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    //////////////////////////////////////////////////////////////////////////
                    tabFile.GetString(i, "StandAni_1", "", ref szTemp);
                    string[] szStandAni1 = szTemp.Split(new char[] { ',' });
                    for (int nIndex = 0; nIndex < szStandAni1.Count(); ++nIndex )
                    {
                        Texture2D tex = Resources.Load(SceneDef.RADISH_REPRESENT_TEXTURE_PATH + szStandAni1[nIndex]) as Texture2D;
                        template.TexStandAni1.Add(tex);
                        
                    }

                    tabFile.GetString(i, "StandAni_2", "", ref szTemp);
                    string[] szStandAni2 = szTemp.Split(new char[] { ',' });
                    for (int nIndex = 0; nIndex < szStandAni2.Count(); ++nIndex)
                    {
                        Texture2D tex = Resources.Load(SceneDef.RADISH_REPRESENT_TEXTURE_PATH + szStandAni2[nIndex]) as Texture2D;
                        template.TexStandAni2.Add(tex);

                    }

                    tabFile.GetString(i, "Texture", "", ref szTemp);
                    template.DefaultTexture = Resources.Load(SceneDef.RADISH_REPRESENT_TEXTURE_PATH + szTemp) as Texture2D;
                    //////////////////////////////////////////////////////////////////////////

                    m_RLRadishTemplateList[template.nRepresentId] = template;
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
                    m_RLRadishTemplateList.Clear();
                }
            }
        }

        // 获取表现萝卜模板
        public RLRadishTemplate GetRLRadishTemplate(int nRepresent)
        {
            if (m_RLRadishTemplateList.ContainsKey(nRepresent))
            {
                return m_RLRadishTemplateList[nRepresent];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载特效表现模板
        public void LoadRLEffectTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.EFFECT_REPRESENT_TEMPLATE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLEffectTemplate template = new RLEffectTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    //////////////////////////////////////////////////////////////////////////
                    tabFile.GetString(i, "Ani", "", ref szTemp);
                    string[] szAni = szTemp.Split(new char[] { ',' });
                    for (int nIndex = 0; nIndex < szAni.Count(); ++nIndex)
                    {
                        Texture2D tex = Resources.Load(SceneDef.EFFECT_REPRESENT_TEXTURE_PATH + szAni[nIndex]) as Texture2D;
                        template.TexAni.Add(tex);
                    }

                    //////////////////////////////////////////////////////////////////////////

                    m_RLEffectTemplateList[template.nRepresentId] = template;
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
                    m_RLEffectTemplateList.Clear();
                }
            }
        }

        // 获取表现特效模板
        public RLEffectTemplate GetRLEffectTemplate(int nRepresent)
        {
            if (m_RLEffectTemplateList.ContainsKey(nRepresent))
            {
                return m_RLEffectTemplateList[nRepresent];
            }
            return null;
        }
        //////////////////////////////////////////////////////////////////////////
        // 加载表现炮塔模板
        public void LoadRLTowerTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.TOWER_REPRESENT_TEMPLATE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLTowerTemplate template = new RLTowerTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    //////////////////////////////////////////////////////////////////////////
                    tabFile.GetString(i, "FireAni", "", ref szTemp);
                    string[] szFireAni = szTemp.Split(new char[] { ',' });
                    for (int nIndex = 0; nIndex < szFireAni.Count(); ++nIndex)
                    {
                        Texture2D tex = Resources.Load(SceneDef.TOWER_REPRESENT_TEXTURE_PATH + szFireAni[nIndex]) as Texture2D;
                        template.TexFireAni.Add(tex);

                    }

                    tabFile.GetString(i, "bg", "", ref szTemp);
                    template.BGTexture = Resources.Load(SceneDef.TOWER_REPRESENT_TEXTURE_PATH + szTemp) as Texture2D;

                    tabFile.GetString(i, "Texture", "", ref szTemp);
                    template.DefaultTexture = Resources.Load(SceneDef.TOWER_REPRESENT_TEXTURE_PATH + szTemp) as Texture2D;
                    //////////////////////////////////////////////////////////////////////////

                    m_RLTowerTemplateList[template.nRepresentId] = template;
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
                    m_RLTowerTemplateList.Clear();
                }
            }
        }

        // 获取表现炮塔模板
        public RLTowerTemplate GetRLTowerTemplate(int nRepresent)
        {
            if (m_RLTowerTemplateList.ContainsKey(nRepresent))
            {
                return m_RLTowerTemplateList[nRepresent];
            }
            return null;
        }

        //////////////////////////////////////////////////////////////////////////
        // 加载表现子弹模板
        public void LoadRLMissileTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(SceneDef.MISSILE_REPRESENT_TEMPLATE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLMissileTemplate template = new RLMissileTemplate();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "RepresentId", 0, ref nTemp);
                    template.nRepresentId = nTemp;

                    tabFile.GetString(i, "Name", "", ref szTemp);
                    template.szName = szTemp;

                    //////////////////////////////////////////////////////////////////////////
                    tabFile.GetString(i, "FlyAni", "", ref szTemp);
                    string[] szFlyAni = szTemp.Split(new char[] { ',' });
                    for (int nIndex = 0; nIndex < szFlyAni.Count(); ++nIndex)
                    {
                        Texture2D tex = Resources.Load(SceneDef.MISSILE_REPRESENT_TEXTURE_PATH + szFlyAni[nIndex]) as Texture2D;
                        template.TexFlyAni.Add(tex);
                    }

                    //////////////////////////////////////////////////////////////////////////

                    m_RLMissileTemplateList[template.nRepresentId] = template;
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
                    m_RLTowerTemplateList.Clear();
                }
            }
        }

        // 获取表现子弹模板
        public RLMissileTemplate GetRLMissileTemplate(int nRepresent)
        {
            if (m_RLMissileTemplateList.ContainsKey(nRepresent))
            {
                return m_RLMissileTemplateList[nRepresent];
            }
            return null;
        }

        //////////////////////////////////////////////////////////////////////////
        // 加载UI配置
        public void LoadRLUISetting()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(UIPathDef.STR_UICONFIG_PATH);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    RLUISetting template = new RLUISetting();

                    int nTemp = 0;
                    string szTemp = null;

                    tabFile.GetInteger(i, "Number", -1, ref nTemp);
                    template.nUINumber = nTemp;

                    tabFile.GetString(i, "ResourceName", "", ref szTemp);
                    template.resourceName = szTemp;

                    tabFile.GetInteger(i, "IsSceneUI", 0, ref nTemp);
                    template.bIsSceneUI = (nTemp>0?true:false);

                    m_RLUISettingList[template.nUINumber] = template;
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
                    m_RLUISettingList.Clear();
                }
            }
        }

        // 获取UI配置
        public RLUISetting GetRLUISetting(int nUINumber)
        {
            if (m_RLUISettingList.ContainsKey(nUINumber))
            {
                return m_RLUISettingList[nUINumber];
            }
            return null;
        }
    }
}
