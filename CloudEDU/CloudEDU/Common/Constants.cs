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
using Windows.Storage;
using System.Text.RegularExpressions;

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
        public static CUSTOMER UserEntity;
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
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Save<T>(string key, T value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static T Read<T>(string key)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                return (T)ApplicationData.Current.LocalSettings.Values[key];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>成功true/失败false</returns>
        public static bool Remove(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.Remove(key);
        }

        public static bool isUserNameAvailable(string un)
        {
            string Regextest = "^[a-zA-Z_][a-zA-Z0-9_]{2,14}$";
            return Regex.IsMatch(un, Regextest);
        }

        public static bool isEmailAvailable(string em)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            return re.IsMatch(em);
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

