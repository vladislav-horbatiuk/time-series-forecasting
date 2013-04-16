using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BSWork.VisualizationHandling;

namespace Graph
{
    public partial class GraphForm : Form
    {
        public GraphForm()
        {
            InitializeComponent();
        }

        public GraphForm(IVisualizationHandler iVisualizer)
            : this()
        {
            visualizationHandler = iVisualizer;
        }

        public void AddGraph(double[] iDataArray, ConsoleColor iColor, double iOffset = 0, double iInterval = 1, string iName = null)
        {
            Series seriesToAdd;
            if (iName != null)
            {
                while (mainChart.Series.Any(serie => serie.Name.Equals(iName)))
                    iName = iName + '1';
                seriesToAdd = new Series(iName);
            }
            else
                seriesToAdd = new Series();
            seriesToAdd.Color = Color.FromName(Enum.GetName(typeof(ConsoleColor), iColor));
            seriesToAdd.ChartType = SeriesChartType.Line;
            double xValue = iOffset;
            for (int i = 0; i < iDataArray.Count(); i++)
            {
                seriesToAdd.Points.AddXY(xValue, iDataArray[i]);
                xValue += iInterval;
            }
            mainChart.Series.Add(seriesToAdd);
        }
        
        private void GraphForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (visualizationHandler != null)
                visualizationHandler.FormClosedHandler();

        }

        private IVisualizationHandler visualizationHandler;
    }
}
