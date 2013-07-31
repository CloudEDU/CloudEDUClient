using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace CloudEDU.CourseStore
{
    public static class StorageFolderExtension
    {
        public static async Task<bool> CheckFileExisted(this StorageFolder folder, string fileName)
        {
            return folder != null &&
                   (await (
                              folder.CreateFileQueryWithOptions(
                                  new QueryOptions
                                  {
                                      FolderDepth = FolderDepth.Shallow,
                                      UserSearchFilter = "System.FileName:\"" + fileName + "\""
                                  })).GetFilesAsync()).Count > 0
                       ? true
                       : false;
        }
    }
}
