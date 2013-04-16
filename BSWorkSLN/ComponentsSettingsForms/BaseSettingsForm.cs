using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BSWork.ComponentsSettingsForms
{
    public class BaseSettingsForm:Form
    {
    
        public virtual TableLayoutPanel MainTableLayout
        {
            get;
            private set;
        }

        public virtual TableLayoutPanel SecondaryTableLayout
        {
            get;
            private set;
        }

        public virtual TabControl MainTabControl
        {
            get;
            private set;
        }

    }
}
