using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.UI.UIEvent
{
    public class UIEventCenter
    {
        public static event UIEventDef.UIEvent_AdventureGameStart m_EventAdventureGameStart;
        public static UIEventDef.UIEvent_AdventureGameStart EventAdventureGameStart
        {
            get
            {
                return m_EventAdventureGameStart;
            }
            set
            {
                m_EventAdventureGameStart = value;
            }
        }

        public static event UIEventDef.UIEvent_OpenGameMenu m_EventOpenGameMenu;
        public static UIEventDef.UIEvent_OpenGameMenu EventOpenGameMenu
        {
            get
            {
                return m_EventOpenGameMenu;
            }
            set
            {
                m_EventOpenGameMenu = value;
            }
        }
    }
}
