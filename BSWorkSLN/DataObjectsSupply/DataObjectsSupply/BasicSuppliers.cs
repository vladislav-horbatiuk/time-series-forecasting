using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BSWork.DataObjects;

namespace DataObjectsSupply
{
    public static class DataObjectSupplier
    {
        /// <summary>
        /// Creates and returns new DataObject with array of values corresponding to values in iStream.
        /// </summary>
        /// <param name="iStream">A stream to read values from it.WARNING: The method uses ReadLine() method to get next value, so all the values should be written using WriteLine() method!(Otherwise, the data will be screwed up)</param>
        /// <param name="iName">An optional string name for new DataObject.</param>
        /// <returns></returns>
        public static BSDataObject GetDataObjectForStream(Stream iStream, string iName = null)
        {
            StreamReader sr = new StreamReader(iStream);
            List<double> aList = new List<double>(100);
            while (!sr.EndOfStream)
                aList.Add(Convert.ToDouble(sr.ReadLine()));
            return new BSDataObject(aList.ToArray(), iName);
        }
    }
}
