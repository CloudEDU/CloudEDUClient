﻿using CloudEDU.CourseService;
using CloudEDU.CourseStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.Common
{
    public class Constants
    {
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

        //public static Course CourseOkToCourse(COURSE_OK course_ok)
        //{
        //    Course course = new Course();
            
            
        //}
    }
}
