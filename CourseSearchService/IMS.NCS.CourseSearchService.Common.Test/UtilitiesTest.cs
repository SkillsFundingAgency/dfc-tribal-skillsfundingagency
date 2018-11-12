using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IMS.NCS.CourseSearchService.Common.Test
{
    /// <summary>
    ///This is a test class for Utilities.cs and is intended to contain all Utilities.cs Unit Tests.
    ///</summary>
    [TestClass]
    public class UtilitiesTest
    {
        /// <summary>
        /// A test for Utilities.ConvertToDelimitedString(string[], string)
        /// </summary>
        [TestMethod]
        public void ConvertToDelimitedString_InputStringArray_Test()
        {
            string[] list = { "alpha", "bravo", "charlie", "delta" };
            string delimiter = "|";

            string result = Utilities.ConvertToDelimitedString(list, delimiter);

            Assert.AreEqual("alpha|bravo|charlie|delta", result);
        }

        /// <summary>
        /// A test for Utilities.ConvertToDelimitedString(List<long>, string)
        /// </summary>
        [TestMethod]
        public void ConvertToDelimitedString_InputListLong_Test()
        {
            List<long> list = new List<long> { 1, 2, 3, 4, 5 };
            string delimiter = ",";

            string result = Utilities.ConvertToDelimitedString(list, delimiter);

            Assert.AreEqual("1,2,3,4,5", result);
        }

        /// <summary>
        /// A test for Utilities.ConvertDelimitedStringToArray(string, string[])
        /// </summary>
        [TestMethod]
        public void ConvertDelimitedStringToArray_Test()
        {
            string str = "alpha|bravo,charlie|delta";
            string[] delimiter = { ",", "|" };

            string[] result = Utilities.ConvertDelimitedStringToArray(str, delimiter);

            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("alpha", result[0]);
            Assert.AreEqual("bravo", result[1]);
            Assert.AreEqual("charlie", result[2]);
            Assert.AreEqual("delta", result[3]);
        }
    }
}
