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

        /// <summary>
        /// SharedDataServiceContext Method.
        /// </summary>
        /// <returns>Singleton DataServiceContext.</returns>
        public static CloudEDUEntities SharedDataServiceContext()
        {
            if (ctx == null)
            {
                ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
            }
            return ctx;
        }
    }
}
