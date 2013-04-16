using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BSWork.DataVisualization;
using BSWork.DataObjects;

namespace BSWork
{
    namespace BSComponents
    {
        public class BSGraphComponent : BSBaseComponent
        {
            public BSGraphComponent(Rectangle iOnScreenRectangle)
                : base(iOnScreenRectangle)
            {
            }

            public override string ComponentType
            {
                get { return "O"; }
            }

            public override void Start()
            {
                CheckIfInputsAreProvided();
                foreach (BSDataObject dataObj in Inputs)
                {
                    visualizer.AddToGraph(dataObj);
                }

                visualizer.ShowGraph();
                //Will possibly need refactoring.
                visualizer = new SimpleVisualizationHandler();
                //
            }
            public override void ShowSettingsForm()
            {
                //throw new NotImplementedException();
            }

            public override bool IsReady()
            {
                return true;
            }

            public override string WhatsWrong()
            {
                return "Компонент 'Графік' налаштований коректно.";
            }

            SimpleVisualizationHandler visualizer = new SimpleVisualizationHandler();
        }
    }
}
