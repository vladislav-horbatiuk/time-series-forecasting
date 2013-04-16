using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace VisualizationHandling
    {
        /// <summary>
        /// Interface for the visualization handler objects
        /// Usage: Works like a handler for the graph form that it creates.
        /// </summary>
        public interface IVisualizationHandler
        {
            /// <summary>
            /// Call this method when the graph form gets closed.
            /// </summary>
            void FormClosedHandler();
        }
    }
}
