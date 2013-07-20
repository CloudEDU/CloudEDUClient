using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CloudEDU.CourseStore
{
    /// <summary>
    /// Course Model
    /// </summary>
    public class Course : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _name = String.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        private double _rate;
        public double Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                if (this._rate != value)
                {
                    this._rate = value;
                    this.OnPropertyChanged("Rate");
                }
            }
        }

        private ImageSource _imageSource = null;
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                if (this._imageSource != value)
                {
                    this._imageSource = value;
                    this.OnPropertyChanged("ImageSource");
                }
            }
        }

        private string _category = String.Empty;
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (this._category != value)
                {
                    this._category = value;
                    this.OnPropertyChanged("Category");
                }
            }
        }

        public void setImage(Uri baseUri, string path)
        {
            ImageSource = new BitmapImage(new Uri(baseUri, path));
        }
    }

    // Workaround: data binding works best with an enumeration of objects that does not implement IList
    public class CourseCollection : IEnumerable<Object>
    {
        private ObservableCollection<Course> courseCollection = new ObservableCollection<Course>();

        public IEnumerator<Object> GetEnumerator()
        {
            return courseCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Course course)
        {
            courseCollection.Add(course);
        }
    }

    public class GroupInfoList<T> : List<Object>
    {
        public object Key { get; set; }

        public new IEnumerator<Object> GetEnumerator()
        {
            return (IEnumerator<Object>)base.GetEnumerator();
        }
    }
}
