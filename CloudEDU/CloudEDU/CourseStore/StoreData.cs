using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.CourseStore
{
    /// <summary>
    /// The Model used to group the data according to the category, provide 
    /// GetSingleGroupByCategoryName() and GetGroupsByCategory() two methods.
    /// The Demo.
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
    /// List<GroupInfoList<Object>> singleGroupByPhysics = storeData.GetSingleGroupByCategoryName("Physics");
    /// List<GroupInfoList<Object>> GroupedData = storeData.GetGroupsByCategory();
    /// </code>
    /// </summary>
    public class StoreData
    {
        public StoreData()
        {
            Course course;
            Uri baseUri = new Uri("ms-appx:///");

            course = new Course();
            course.Name = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "newest";
            Collection.Add(course);

            course = new Course();
            course.Name = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "newest";
            Collection.Add(course);

            course = new Course();
            course.Name = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "newest";
            Collection.Add(course);

            course = new Course();
            course.Name = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "newest";
            Collection.Add(course);

            course = new Course();
            course.Name = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "hottest";
            Collection.Add(course);

            course = new Course();
            course.Name = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "hottest";
            Collection.Add(course);

            course = new Course();
            course.Name = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "hottest";
            Collection.Add(course);

            course = new Course();
            course.Name = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "Computer Science";
            Collection.Add(course);

            course = new Course();
            course.Name = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "Computer Science";
            Collection.Add(course);

            course = new Course();
            course.Name = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "Computer Science";
            Collection.Add(course);

            course = new Course();
            course.Name = "sun shower";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course2.png");
            course.Category = "Psychology";
            Collection.Add(course);

            course = new Course();
            course.Name = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "Psychology";
            Collection.Add(course);

            course = new Course();
            course.Name = "the power of smile";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course1.png");
            course.Category = "Psychology";
            Collection.Add(course);

            course = new Course();
            course.Name = "the evolution";
            course.Rate = 4.0;
            course.setImage(baseUri, "Images/Courses/course3.png");
            course.Category = "Psychology";
            Collection.Add(course);
        }

        private CourseCollection _collection = new CourseCollection();
        public CourseCollection Collection
        {
            get
            {
                return _collection;
            }
        }

        /// <summary>
        /// Add course to the list used to display.
        /// </summary>
        /// <param name="course">The course to be added.</param>
        public void AddCourse(Course course)
        {
            Collection.Add(course);
        }

        /// <summary>
        /// Add a group of courses to the list used to display.
        /// </summary>
        /// <param name="courses">The course list to be added.</param>
        public void AddCourses(List<Course> courses)
        {
            foreach (Course course in courses)
            {
                Collection.Add(course);
            }
        }

        //internal List<Object> GetCoursesByCategoryName(string categoryName)
        //{
        //    List<Object> courses = new List<Object>();

        //    var query = from course in Collection
        //                where ((Course)course).Category == categoryName
        //                select course;

        //    foreach (var g in query)
        //    {
        //        courses.Add(g);
        //    }

        //    return courses;
        //}

        /// <summary>
        /// Get the single group classified by the category name.
        /// </summary>
        /// <param name="categoryName">The category name need to be group.</param>
        /// <returns>A list only contain a single GroupInfoList, which contain the 
        /// elements that had been grouped.</returns>
        internal List<GroupInfoList<Object>> GetSingleGroupByCategoryName(string categoryName)
        {
            List<GroupInfoList<Object>> groups = new List<GroupInfoList<Object>>();

            var query = from course in Collection
                        where ((Course)course).Category == categoryName
                        group course by ((Course)course).Category into g
                        select new { GroupName = g.Key, Courses = g };

            foreach (var g in query)
            {
                GroupInfoList<Object> info = new GroupInfoList<Object>();
                info.Key = g.GroupName;
                foreach (var course in g.Courses)
                {
                    info.Add(course);
                }
                groups.Add(info);
            }

            return groups;
        }

        /// <summary>
        /// Get the list that has been grouped by the category.
        /// </summary>
        /// <returns>A list of GroupInfoList, each GroupInfoList contains the data 
        /// classified according to the category.</returns>
        internal List<GroupInfoList<Object>> GetGroupsByCategory()
        {
            List<GroupInfoList<Object>> groups = new List<GroupInfoList<Object>>();

            var query = from course in Collection
                        group course by ((Course)course).Category into g
                        select new { GroupName = g.Key, Courses = g };

            foreach (var g in query)
            {
                GroupInfoList<Object> info = new GroupInfoList<Object>();
                info.Key = g.GroupName;
                foreach (var course in g.Courses)
                {
                    info.Add(course);
                }
                groups.Add(info);
            }

            return groups;
        }
    }
}
