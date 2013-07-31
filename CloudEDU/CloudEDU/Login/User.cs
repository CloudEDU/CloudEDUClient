using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.Login
{
    /// <summary>
    /// User model
    /// </summary>
    public class User 
    {
        public CUSTOMER c;
        public string NAME { get; set; }
        public string ImageSource { get; set; }
        public int ID { get; set; }
        public User(CUSTOMER c)
        {
            NAME = c.NAME;
            ID = c.ID;
            ImageSource = "http://www.gravatar.com/avatar/" + Constants.ComputeMD5(c.EMAIL);
        }

        public User(string un, string ims)
        {
            NAME = un;
            ImageSource = ims;
        }
    }
}
