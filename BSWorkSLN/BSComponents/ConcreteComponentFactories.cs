using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BSWork
{
    namespace BSComponents
    {
        public class BSInputComponentsFactory : IComponentsFactory
        {
            public BSBaseComponent NewComponent(Rectangle iOnScreenRectangle)
            {
                return new BSInputComponent(iOnScreenRectangle);
            }
        }

        public class BSPreprocessorComponentsFactory : IComponentsFactory
        {
            public BSBaseComponent NewComponent(Rectangle iOnScreenRectangle)
            {
                return new BSPreprocessorComponent(iOnScreenRectangle);
            }
        }

        public class BSGraphComponentsFactory : IComponentsFactory
        {
            public BSBaseComponent NewComponent(Rectangle iOnScreenRectangle)
            {
                return new BSGraphComponent(iOnScreenRectangle);
            }
        }

        public class BSModelBuilderComponentsFactory : IComponentsFactory
        {
            public BSBaseComponent NewComponent(Rectangle iOnScreenRectangle)
            {
                return new BSModelBuilderComponent(iOnScreenRectangle);
            }
        }

        public class BSForecastingComponentsFactory : IComponentsFactory
        {
            public BSBaseComponent NewComponent(Rectangle iOnScreenRectangle)
            {
                return new BSForecastingComponent(iOnScreenRectangle);
            }
        }

        public class BSSaveDataComponentsFactory : IComponentsFactory
        {
            public BSBaseComponent NewComponent(Rectangle iOnScreenRectangle)
            {
                return new BSSaveDataComponent(iOnScreenRectangle);
            }
        }
    }
}
