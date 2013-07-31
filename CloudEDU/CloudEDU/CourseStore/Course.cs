using CloudEDU.Common;
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

        private int? _id = null;
        public int? ID
        {
            get
            {
                return _id.HasValue ? _id.Value : int.MaxValue;
            }
            set
            {
                if (this._id != value)
                {
                    this._id = value;
                    this.OnPropertyChanged("ID");
                }
            }
        }

        private decimal? _price = null;
        public decimal? Price
        {
            get
            {
                return _price.HasValue ? _price.Value : decimal.MaxValue;
            }
            set
            {
                if (this._price != value)
                {
                   this._price = value;
                    this.OnPropertyChanged("PRICE");
                }
            }
        }

        private double? _rate = null;
        public double? Rate
        {
            get
            {
                return _rate.HasValue ? _rate.Value : double.MaxValue;
            }
            set
            {
                if (this._rate != value)
                {
                    this._rate = value;
                    this.OnPropertyChanged("RATE");
                }
            }
        }

        private string _title = String.Empty;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (this._title != value)
                {
                    this._title = value;
                    this.OnPropertyChanged("TITLE");
                }
            }
        }

        private string _intro = String.Empty;
        public string Intro
        {
            get
            {
                return _intro;
            }
            set
            {
                if (this._intro != value)
                {
                    this._intro = value;
                    this.OnPropertyChanged("INTRO");
                }
            }
        }

        private string _teacher = String.Empty;
        public string Teacher
        {
            get
            {
                return _teacher;
            }
            set
            {
                if (this._teacher != value)
                {
                    this._teacher = value;
                    this.OnPropertyChanged("TEACHER");
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
                    this.OnPropertyChanged("CATEGORY");
                }
            }
        }

        private string _courseState = String.Empty;
        public string CourseState
        {
            get
            {
                return _courseState;
            }
            set
            {
                if (this._courseState != value)
                {
                    this._courseState = value;
                    this.OnPropertyChanged("COURSE_STATE");
                }
            }
        }

        private int? _pg = null;
        public int? PG
        {
            get
            {
                return _pg.HasValue ? _pg.Value : int.MaxValue;
            }
            set
            {
                if (this._pg != value)
                {
                    this._pg = value;
                    this.OnPropertyChanged("PG");
                }
            }
        }

        private string _imageUri = null;
        public string ImageUri
        {
            get
            {
                return _imageUri;
            }
            set
            {
                if (this._imageUri != value)
                {
                    this._imageUri = value;
                    this.OnPropertyChanged("ICON_URL");
                }
            }
        }

        private int? _lessonNum = null;
        public int? LessonNum
        {
            get
            {
                return _lessonNum.HasValue ? _lessonNum.Value : 0;
            }
            set
            {
                if (this._lessonNum != value)
                {
                    this._lessonNum = value;
                    this.OnPropertyChanged("LESSON_NUM");
                }
            }
        }

        private DateTime? _startTime = null;
        public DateTime StartTime
        {
            get
            {
                return _startTime.HasValue ? _startTime.Value : DateTime.Now;
            }
            set
            {
                if (this._startTime != value)
                {
                    this._startTime = value;
                    this.OnPropertyChanged("START_TIME");
                }
            }
        }

        private int? _ratedUser = null;
        public int? RatedUser
        {
            get
            {
                return _ratedUser.HasValue ? _ratedUser.Value : 0;
            }
            set
            {
                if (this._ratedUser != value)
                {
                    this._ratedUser = value;
                    this.OnPropertyChanged("RATED_USER");
                }
            }
        }

        private GridViewItemContainerType _itemContainerType = GridViewItemContainerType.DefaultGridViewItemContainerSize;
        public GridViewItemContainerType ItemContainerType
        {
            get
            {
                return _itemContainerType;
            }
            set
            {
                if (this._itemContainerType != value)
                {
                    this._itemContainerType = value;
                    this.OnPropertyChanged("ITEM_CONTAINER_TYPE");
                }
            }
        }

        #region Will be cast off
        private bool _isBuy = false;
        public bool IsBuy
        {
            get
            {
                return _isBuy;
            }
            set
            {
                if (this._isBuy != value)
                {
                    this._isBuy = value;
                    this.OnPropertyChanged("IsBuy");
                }
            }
        }

        private bool _isTeach = false;
        public bool IsTeach
        {
            get
            {
                return _isTeach;
            }
            set
            {
                if (this._isTeach != value)
                {
                    this._isTeach = value;
                    this.OnPropertyChanged("IsTeaching");
                }
            }
        }
        #endregion
    }

    // Workaround: data binding works best with an enumeration of objects that does not implement IList
    public class CourseCollection : IEnumerable<object>
    {
        private ObservableCollection<Course> courseCollection = new ObservableCollection<Course>();

        public IEnumerator<object> GetEnumerator()
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

    public class GroupInfoList<T> : List<object>
    {
        public object Key { get; set; }
        public string CategoryImg { get; set; }

        public new IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>)base.GetEnumerator();
        }
    }
}
