using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graph;
using BSWork.DataObjects;
using System.Collections;
using System.Windows;
using System.Threading;
using BSWork.VisualizationHandling;
using System.Windows.Forms;

namespace BSWork
{
    namespace DataVisualization
    {
        public delegate void FormShower();
        /// <summary>
        /// Interface for the visualizer object
        /// Usage: First: Call AddToGraph for every DataObject that you want to graph.
        ///        Second: Call ShowGraph().
        /// </summary>
        public interface IVisualizer
        {
            /// <summary>
            /// A method to add a DataObject to the visualization list
            /// </summary>
            /// <param name="iObj">A data object, containing array of values, that needs to be graphed</param>
            void AddToGraph(BSDataObject iObj);

            void ShowGraph();
        }



        /// <summary>
        /// Basic visualizer that implements IVisualizer and IVisualizationHander interfaces interface
        /// </summary>
        public class SimpleVisualizationHandler : IVisualizer, IVisualizationHandler
        {
            /// <summary>
            /// This constructor will create a new graph form, but won't visualize it right now.
            /// To visualize (after adding all needed graphs) call GraphShow().
            /// </summary>
            public SimpleVisualizationHandler()
            {
                gForm = new GraphForm(this);
                dataObjectsAndColors = new Dictionary<BSDataObject, ConsoleColor>();
            }

            /// <summary>
            /// If iObj was already added to the visualization list, function will do nothing.
            /// If number of already added objects exceed the number of allowed colors (14), than the next
            /// object will receive already assigned color (so, avoid this type of situations).
            /// </summary>
            /// <param name="iObj">See IVisualizer interface doc string</param>
            public void AddToGraph(BSDataObject iObj)
            {
                if (dataObjectsAndColors.ContainsKey(iObj))
                    return;
                while ((ConsoleColor)colorIndex == ConsoleColor.White || (ConsoleColor)colorIndex == ConsoleColor.DarkYellow) //White and DarkYellow colors are invisible on white graph
                    colorIndex++;
                ConsoleColor value = (ConsoleColor)colorIndex;

                if (colorIndex == (int)ConsoleColor.Yellow) //We exceeded maximum number of console colors
                    colorIndex = (int)ConsoleColor.Black; //So, start from begining
                else
                    colorIndex++;
                AddToDictAndGraphForm(iObj, value);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iObj">See IVisualizer interface doc string</param>
            /// <param name="iColor">A Console color for the future graph</param>
            public void AddToGraph(BSDataObject iObj, ConsoleColor iColor)
            {
                if (dataObjectsAndColors.ContainsKey(iObj))
                    return;
                AddToDictAndGraphForm(iObj, iColor);
            }

            public void ShowGraph()
            {
                //Dirty HACK for showing form on the main UI thread, but let's assume its according to KISS
                Application.OpenForms[0].Invoke(new FormShower(gForm.Show));  
            }

            public void _TestShow()
            {
                while (gForm.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    ;
            }

            private void AddToDictAndGraphForm(BSDataObject iObj, ConsoleColor iColor)
            {
                dataObjectsAndColors.Add(iObj, iColor);
                gForm.AddGraph(iObj.DataArray, iColor, iOffset: iObj.Offset, iName: iObj.ObjName);
            }

            
            public void FormClosedHandler()
            {
                dataObjectsAndColors.Clear();
                gForm = new GraphForm(this);
            }

            private Dictionary<BSDataObject, ConsoleColor> dataObjectsAndColors;
            private uint colorIndex = (int)ConsoleColor.Black;
            private GraphForm gForm;
        }
    }
}
