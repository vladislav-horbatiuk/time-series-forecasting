using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BSWork
{
    namespace BSComponents
    {
        public interface IComponentsFactory
        {
            BSBaseComponent NewComponent(Rectangle iOnScreenRectangle);
        }
    }
}
