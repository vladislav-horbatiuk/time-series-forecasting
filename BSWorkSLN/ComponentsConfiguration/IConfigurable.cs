using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace ComponentsConfiguration
    {
        public interface IConfigurable
        {
            void SetValueForKey(string iKey, object iValue);
            object GetValueForKey(string iKey);
        }
    }
}
