using CloudEDU.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.CourseStore
{
    /// <summary>
    /// The Model used to group the data according to the category, provide 
    /// GetSingleGroupByCategoryTitle(), GetGroupsByCategory(string categoryTitle) 
    /// and GetGroupsByAttendingOrTeaching() three methods.
    /// Here is the Demo.
    /// <code>
    /// Course course = new Course();
    /// List<Course> courses = new List<Course>();
    /// for (int i = 0; i < 10; ++)
    /// {
    ///     Course tmpCourse = new Course();
    ///     courses.Add(tempCourse);
    /// }
    /// StoreData storeData = new Store();
    /// storeData.Add(course);
    /// storeData.Add(courses);
    /// List<GroupInfoList<object>> singleGroupByPhysics = storeData.GetSingleGroupByCategoryTitle("Physics");
    /// List<GroupInfoList<object>> GroupedData = storeData.GetGroupsByCategory();
    /// List<GroupInfoList<object>> MyCourses = storeData.GetGroupsByAttendingOrTeaching();
    /// </code>
    /// </summary>
    public class StoreData
    {
        public StoreData()
        {
            Course course;
            Uri baseUri = new Uri(Constants.BaseURI);

            course = new Course();
            course.Title = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "newest";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            course = new Course();
            course.Title = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "newest";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            course = new Course();
            course.Title = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "newest";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            course = new Course();
            course.Title = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "newest";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            course = new Course();
            course.Title = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "hottest";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            course = new Course();
            course.Title = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "hottest";
            course.IsBuy = true;
            course.IsTeach = false;
            Collection.Add(course);

            course = new Course();
            course.Title = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "hottest";
            course.IsBuy = true;
            course.IsTeach = false;
            Collection.Add(course);

            course = new Course();
            course.Title = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "Computer Science";
            course.IsBuy = true;
            course.IsTeach = false;
            Collection.Add(course);

            course = new Course();
            course.Title = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "Computer Science";
            course.IsBuy = true;
            course.IsTeach = false;
            Collection.Add(course);

            course = new Course();
            course.Title = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "Computer Science";
            course.IsBuy = true;
            course.IsTeach = false;
            Collection.Add(course);

            course = new Course();
            course.Title = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "Psychology";
            course.IsBuy = true;
            course.IsTeach = false;
            Collection.Add(course);

            course = new Course();
            course.Title = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "Psychology";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            course = new Course();
            course.Title = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "Psychology";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            course = new Course();
            course.Title = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "Psychology";
            course.IsBuy = false;
            course.IsTeach = true;
            Collection.Add(course);

            for (int j = 0; j < 15; ++j)
            {
                for (int i = 0; i < 1; ++i)
                {
                    course = new Course();
                    course.Title = "the power of smile";
                    course.Rate = 4.0;
                    course.setImage(baseUri, "Images/Courses/course1.png");
                    course.Category = "Psychology";
                    course.IsBuy = false;
                    course.IsTeach = true;
                    Collection.Add(course);
                }

                for (int i = 0; i < 2; ++i)
                {
                    course = new Course();
                    course.Title = "sun shower";
                    course.Rate = 4.0;
                    course.setImage(baseUri, "Images/Courses/course2.png");
                    course.Category = "Psychology";
                    course.IsBuy = true;
                    course.IsTeach = false;
                    Collection.Add(course);
                }

                for (int i = 0; i < 4; ++i)
                {
                    course = new Course();
                    course.Title = "the evolution";
                    course.Rate = 4.0;
                    course.setImage(baseUri, "Images/Courses/course3.png");
                    course.Category = "Psychology";
                    course.IsBuy = false;
                    course.IsTeach = true;
                    Collection.Add(course);
                }
            }
        }

        private CourseCollection _collection = new CourseCollection();
        public CourseCollection Collection
        {
            get
            {
                return _collection;
            }
        }

        #region Add
        /// <summary>
        /// Add course to the list used to display.
        /// </summary>
        /// <param Title="course">The course to be added.</param>
        public void AddCourse(Course course)
        {
            Collection.Add(course);
        }

        /// <summary>
        /// Add a group of courses to the list used to display.
        /// </summary>
        /// <param Title="courses">The course list to be added.</param>
        public void AddCourses(List<Course> courses)
        {
            foreach (Course course in courses)
            {
                Collection.Add(course);
            }
        }
        #endregion

        //internal List<object> GetCoursesByCategoryTitle(string categoryTitle)
        //{
        //    List<object> courses = new List<object>();

        //    var query = from course in Collection
        //                where ((Course)course).Category == categoryTitle
        //                select course;

        //    foreach (var g in query)
        //    {
        //        courses.Add(g);
        //    }

        //    return courses;
        //}

        #region Different ways used to get the groups
        /// <summary>
        /// Get the single group classified by the category Title.
        /// </summary>
        /// <param Title="categoryTitle">The category Title need to be group.</param>
        /// <returns>A list only contain a single GroupInfoList, which contain the 
        /// elements that had been grouped.</returns>
        internal List<GroupInfoList<object>> GetSingleGroupByCategoryTitle(string categoryTitle)
        {
            List<GroupInfoList<object>> group = new List<GroupInfoList<object>>();

            var query = from course in Collection
                        where ((Course)course).Category == categoryTitle
                        group course by ((Course)course).Category into g
                        select new { GroupTitle = g.Key, Courses = g };

            foreach (var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupTitle;
                foreach (var course in g.Courses)
                {
                    info.Add(course);
                }
                group.Add(info);
            }

            return group;
        }

        /// <summary>
        /// Get the list that has been grouped by the category.
        /// </summary>
        /// <returns>A list of GroupInfoList, each GroupInfoList contains the data 
        /// classified according to the category.</returns>
        internal List<GroupInfoList<object>> GetGroupsByCategory()
        {
            List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();

            var query = from course in Collection
                        group course by ((Course)course).Category into g
                        select new { GroupTitle = g.Key, Courses = g };

            foreach (var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupTitle;
                foreach (var course in g.Courses)
                {
                    info.Add(course);
                }
                groups.Add(info);
            }

            return groups;
        }

        /// <summary>
        /// Get the list that has been grouped by whether user has bought or teached.
        /// </summary>
        /// <returns>A list contains two GroupInfoList, one is attending list, another 
        /// is teaching list.</returns>
        internal List<GroupInfoList<object>> GetGroupsByAttendingOrTeaching()
        {
            List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();

            GroupInfoList<object> attending = new GroupInfoList<object>();
            GroupInfoList<object> teaching = new GroupInfoList<object>();
            attending.Key = "Attending";
            teaching.Key = "Teaching";

            foreach (Course g in Collection)
            {
                if (g.IsBuy)
                {
                    attending.Add(g);
                }
                if (g.IsTeach)
                {
                    teaching.Add(g);
                }
            }

            groups.Add(attending);
            groups.Add(teaching);

            return groups;
        }
        #endregion
    }
}
