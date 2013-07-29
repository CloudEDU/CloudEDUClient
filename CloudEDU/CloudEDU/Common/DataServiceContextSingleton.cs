using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.Common
{
    class DataServiceContextSingleton
    {
        static CloudEDUEntities ctx = null;

        public static CloudEDUEntities SharedDataServiceContext()
        {
            if (ctx == null)
            {
                ctx = new CloudEDUEntities(new Uri(Constants.WCFUri));
            }
            return ctx;
        }
    }
}
