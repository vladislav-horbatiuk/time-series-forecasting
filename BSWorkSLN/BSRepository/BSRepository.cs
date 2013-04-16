using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BSWork.DataObjects;
using DataObjectsSupply;

namespace BSWork
{
    /// <summary>
    /// Class for abstracting from data access (google "repository pattern").
    /// </summary>
    public static class BSRepository
    {
        /// <summary>
        /// Creates a new DataObject using data in file at <paramref name="iFilePath"/> path.
        /// </summary>
        /// <param name="iFilePath">A path to the file with data - relative or absolute.</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.IO.PathTooLongException"/>
        /// <exception cref="System.FormatException"/>
        /// <returns>New Dataobject.</returns>
        public static BSDataObject DataObjectFromFile(string iFilePath)
        {
            BSDataObject result = null;
            using (FileStream sr = new FileStream(iFilePath, FileMode.Open))
            {
                result = DataObjectSupplier.GetDataObjectForStream(sr);
            }
            return result;
        }

        public static void SaveDataObjectToFile(BSDataObject iObj, string iFilePath)
        {
            if (null == iObj || null == iFilePath)
                throw new ArgumentNullException();
            using (StreamWriter sw = new StreamWriter(iFilePath))
            {
                foreach (double number in iObj.DataArray)
                    sw.WriteLine(number);
            }
        }
    }
}
