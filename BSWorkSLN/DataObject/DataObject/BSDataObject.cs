using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace DataObjects
    {
        /// <summary>
        /// A basic class for usage in the "Forecasting" program; If you need more specific types - subclass it.
        /// </summary>
        public class BSDataObject
        {
            /// <summary>
            /// Main constructor of the DataObject class.
            /// </summary>
            /// <param name="iDataArray">WARNING: array elements won't be copied!
            /// Any changes to the iDataArray after the DataObject creation will
            /// affect the DataObject itself.</param>
            /// <param name="iName">The name of the DataObject (optional)</param>
            public BSDataObject(double[] iDataArray, string iName = null)
            {
                DataArray = iDataArray;
                ObjName = iName;
            }

            public BSDataObject(double[] iDataArray, double iOffset, string iName = null)
                : this(iDataArray, iName)
            {
                Offset = iOffset;
            }

            private double[] dataArray;

            public double[] DataArray
            {
                get { return dataArray; }
                set { dataArray = value; }
            }

            private string objName;

            public string ObjName
            {
                get { return objName; }
                set { objName = value; }
            }

            private double offset = 0;

            public double Offset
            {
                get { return offset; }
                set { offset = value; }
            }
            public bool Equals(BSDataObject iObj)
            {
                const double delta = 0.0000000001;

                if (iObj.DataArray.Length != this.DataArray.Length)
                    return false;

                for (int i = 0; i < this.DataArray.Length; i++)
                    if (Math.Abs(this.DataArray[i] - iObj.DataArray[i]) > delta)
                        return false;
                return this.ObjName.Equals(iObj.ObjName);
            }
        }
    }
}