using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.CourseStore
{
    /// <summary>
    /// Course Model
    /// </summary>
    class Course
    {
        public string Name { set; get; }
        public double Rate { set; get; }
        public string ImageSource { set; get; }

        public Course(string name, double rate, string imageSource)
        {
            Name = name;
            Rate = rate;
            ImageSource = imageSource;
        }
    }
}
