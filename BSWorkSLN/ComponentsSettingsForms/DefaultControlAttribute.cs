using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork.ComponentsSettingsForms
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple=false)]
    public class DefaultControlAttribute:Attribute
    { 
    }
}
