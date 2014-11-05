using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Common;

namespace Game.GameLogic
{
    // NPC模板
    class GLNpcTemplate
    {
        public int nTemplateId;     // NPC模板ID
        public int nRepresentId;    // 表现ID
        public string szName;       // NPC名字
    }

    class GLNpcManager : Common.Singleton<GLNpcManager>
    {
        private Dictionary<int, GLNpcTemplate> m_NpcTemplates = new Dictionary<int, GLNpcTemplate>();

        public void Init()
        {
            LoadNpcTemplate();
        }

        public void UnInit()
        {
            m_NpcTemplates.Clear();
        }

        public void LoadNpcTemplate()
        {
            bool success = true;
            try
            {
                Common.TableFile tabFile = Common.TableFile.LoadFromFile(GameLogicDef.NPC_TEMPLATE_LIST_FILE);
                int rowCount = tabFile.GetRowsCount();
                for (int i = 1; i <= rowCount; i++)
                {
                    GLNpcTemplate cfg = new GLNpcTemplate();

                    int nTemplateId = 0;
                    tabFile.GetInteger(i, "TemplateId", 0, ref nTemplateId);
                    cfg.nTemplateId = nTemplateId;

                    int nRepresentId = 0;
                    tabFile.GetInteger(i, "RepresentId", 0, ref nRepresentId);
                    cfg.nRepresentId = nRepresentId;

                    tabFile.GetString(i, "Name", "", ref cfg.szName);

                    m_NpcTemplates[cfg.nTemplateId] = cfg;
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
                    m_NpcTemplates.Clear();
                }
            }
        }

        public GLNpcTemplate GetNpcTemplate(int nTemplateId)
        {
            if (m_NpcTemplates.ContainsKey(nTemplateId))
            {
                return m_NpcTemplates[nTemplateId];
            }
            return null;
        }
    }
}
