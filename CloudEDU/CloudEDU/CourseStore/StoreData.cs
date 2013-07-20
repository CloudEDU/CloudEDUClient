using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.CourseStore
{
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
