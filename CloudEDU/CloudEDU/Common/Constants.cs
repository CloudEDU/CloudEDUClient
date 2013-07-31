using CloudEDU.CourseStore;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using CloudEDU.Login;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace CloudEDU.Common
{
    public class Constants
    {
        public static string BaseURI = "http://10.0.1.65/Upload/";
        public static string DataServiceURI = "http://10.0.1.16/Service.svc/";

        public static string FillStar = "\x2605";
        public static string BlankStar = "\x2606";
        public static double StarWidth = 22.2133331298828;

        public static List<string> ResourceType = new List<string> { "DOCUMENT", "VIDEO", "AUDIO" };
        public static Coursing coursing;
        //public static CUSTOMER User;
        public static User User;
        /// <summary>
        /// Cast the first character of every word in a string from lower to upper.
        /// </summary>
        /// <param name="v">The string to be transformed.</param>
        /// <returns>The string after transformed.</returns>
        public static string UpperInitialChar(string v)
        {
            string[] words = null;
            StringBuilder strBuff = null;

            v = v.Trim();
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

        /// <summary>
        /// Convert a dataservice COURSE_AVAIL view to Course model.
        /// </summary>
        /// <param name="c">COURSE_AVAIL to be converted.</param>
        /// <returns>Course after converted.</returns>
        public static Course CourseAvail2Course(COURSE_AVAIL c)
        {
            Course course = new Course();
 
            course.Title = c.TITLE;
            course.Intro = c.INTRO;
            course.ID = c.ID;
            course.Teacher = c.TEACHER_NAME;
            course.Category = c.CATE_NAME;
            course.Price = c.PRICE;
            course.Rate = c.RATE;
            course.PG = c.RESTRICT_AGE;
            course.LessonNum = c.LESSON_NUM;
            course.RatedUser = c.RATED_USERS;
            course.ImageUri = BaseURI + c.ICON_URL.Replace('\\', '/');
            course.IsBuy = true;
            course.IsTeach = true;

            return course;
        }
        public static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
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

