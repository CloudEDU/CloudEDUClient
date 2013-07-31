using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.CourseStore
{
    class DBAccessAPIs
    {
        private CloudEDUEntities ctx = null;


        public DBAccessAPIs()
        {


            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
        }


        public DataServiceQuery<CUSTOMER> customerDsq = null;
        public delegate string deleMethod(int a, string b);

        public static string test(deleMethod m)
        {

            
            return m(1, "aaa");
            
        }



        public delegate void onUserQueryComplete(IAsyncResult result);
        private onUserQueryComplete onUQC;


        public void getUserById(int id, onUserQueryComplete onComplete)
        {
            customerDsq = (DataServiceQuery<CUSTOMER>)(from cus in ctx.CUSTOMER where cus.ID == Constants.User.ID select cus);
            this.onUQC = onComplete;
            customerDsq.BeginExecute(onUserQueryComplete2, null);
        }


        private void onUserQueryComplete2(IAsyncResult result)
        {
            this.onUQC(result);

        }


    }
}
