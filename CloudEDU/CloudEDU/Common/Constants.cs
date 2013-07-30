using CloudEDU.CourseStore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.Common
{
    public class Constants
    {
        public static string BaseURI = "ms-appx:///";
        public static string DataServiceURI = "http://10.0.1.39:8080/CloudEDUServer/Service.svc/";

        public static string FillStar = "\x2605";
        public static string BlankStar = "\x2606";
        public static double StarWidth = 22.2133331298828;

        /// <summary>
        /// Cast the first character of every word in a string from lower to upper.
        /// </summary>
        /// <param name="v">The string to be transformed.</param>
        /// <returns>The string after transformed.</returns>
        public static string UpperInitialChar(string v)
        {
            string[] words = null;
            StringBuilder strBuff = null;

            words = System.Text.RegularExpressions.Regex.Split(v, @"\s+");

            strBuff = new StringBuilder();
            for (int i = 0; i < words.Length; ++i)
            {
                words[0] = words[i].ToLower();
                strBuff.AppendFormat("{0}{1}", Char.ToUpper(words[i][0]), words[i].Substring(1));
                strBuff.Append(' ');
            }

            return strBuff.ToString();
        }
    }

    /// <summary>
    /// Used as selector to select container size.
    /// </summary>
    public enum GridViewItemContainerType
    {
        DefaultGridViewItemContainerSize = 0,
        DoubleHeightGridViewItemContainerSize = 1,
        DoubleWidthGridViewItemContsinerSize = 2,
        SquareGridViewItemContainerSize = 3,
    }
}
