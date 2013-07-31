using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using System.Data.Services.Client;

namespace CloudEDU.Login
{
    /// <summary>
    /// User model
    /// </summary>
    public class User 
    {
        [SQLite.PrimaryKey]
        public string NAME { get; set; }
        public bool ALLOW { get; set; }
        public string ImageSource { get; set; }
        public int ID { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public DateTime BIRTHDAY { get; set; }
        public string DEGREE { get; set; }
        public double LEARN_RATE { get; set; }
        public double TEACH_RATE { get; set; }
        public decimal BALANCE { get; set; }
        public int ATTEND_COUNT { get; set; }
        public int TEACH_COUNT { get; set; }
        private CloudEDUEntities ctx = null;
        private DataServiceQuery<CUSTOMER> customerDsq = null;
        private List<CUSTOMER> csl;
        private DataServiceQuery<COURSE_AVAIL> teachDsq = null;

        //public NOTE NOTE { get; set; }
        public User(CUSTOMER c)
        {
            NAME = c.NAME;
            ID = c.ID;
            EMAIL = c.EMAIL;
            PASSWORD = c.PASSWORD;
            TEACH_RATE = c.TEACH_RATE;
            LEARN_RATE = c.LEARN_RATE;
            DEGREE = c.DEGREE;
            BIRTHDAY = c.BIRTHDAY;
            BALANCE = c.BALANCE;
            ALLOW = c.ALLOW;
            SetAttendTeachNumber();
            //TaskFactory<IEnumerable<CUSTOMER>> tf = new TaskFactory<IEnumerable<CUSTOMER>>();
            //customerDsq = (DataServiceQuery<CUSTOMER>)(from user in ctx.CUSTOMER where user.NAME.Equals(InputUsername.Text) select user);
            //IEnumerable<CUSTOMER> cs = await tf.FromAsync(customerDsq.BeginExecute(null, null), iar => customerDsq.EndExecute(iar));
            //csl = new List<CUSTOMER>(cs);
            Constants.Save<string>("LastUser", NAME);
            ImageSource = "http://www.gravatar.com/avatar/" + Constants.ComputeMD5(c.EMAIL)+"?s=400";
            CreateDBAndInsert();
        }

        private async void SetAttendTeachNumber()
        {
            teachDsq = (DataServiceQuery<COURSE_AVAIL>)(from course in ctx.COURSE_AVAIL
                                                        where course.TEACHER_NAME == Constants.User.NAME
                                                        select course);
            TaskFactory<IEnumerable<COURSE_AVAIL>> tf = new TaskFactory<IEnumerable<COURSE_AVAIL>>();
            IEnumerable<COURSE_AVAIL> attends = await tf.FromAsync(ctx.BeginExecute<COURSE_AVAIL>(
                new Uri("/GetAllCoursesAttendedByCustomer?customer_id=13", UriKind.Relative), null, null),
                iar => ctx.EndExecute<COURSE_AVAIL>(iar));
            ATTEND_COUNT = attends.Count();
            IEnumerable<COURSE_AVAIL> teaches = await tf.FromAsync(teachDsq.BeginExecute(null, null), iar => teachDsq.EndExecute(iar));
            TEACH_COUNT = teaches.Count();
        }

        public User(string un, string ims)
        {
            NAME = un;
            ImageSource = ims;
        }

        public User()
        { }

        private void CreateDBAndInsert()
        {
            using (SQLiteConnection db = CreateSQLiteConnection())
            {
                db.CreateTable<User>();
                if (db.Table<User>().Count() != 0)
                {
                    db.DeleteAll<User>();
                }
                db.InsertOrReplace(this);
                db.Close();
            }
        }

        private static SQLiteConnection CreateSQLiteConnection()
        {
            var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "User.db");
            return new SQLite.SQLiteConnection(dbPath);
        }

        public static List<User> Select()
        {
            SQLiteConnection db = User.CreateSQLiteConnection();
            return db.Query<User>("select * from User");
        }

        public static User SelectLastUser()
        {
            SQLiteConnection db = User.CreateSQLiteConnection();
            return db.Query<User>("select * from User where NAME =? ", Constants.Read<string>("LastUser"))[0];
        }
    }
}
