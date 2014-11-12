using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.UI.UIEvent
{
    public class UIEventDef
    {
        //----------------------通用事件-----------------------------------------------
        public delegate void UIEvent_Common(object sender, BaseUIEventArgs args);
        public class BaseUIEventArgs : EventArgs
        {
            public int[] eventParam = new int[5];
        }

        //----------------------冒险模式游戏开始----------------------------------------
        public delegate void UIEvent_AdventureGameStart(object sender, BaseUIEventArgs args);

        //----------------------打开系统菜单--------------------------------------------
        public delegate void UIEvent_OpenGameMenu(object sender, BaseUIEventArgs args);
    }
}
