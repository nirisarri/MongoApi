using System.Collections;
using System.Net;
using System.Runtime.Serialization;
using System.Web.Http;

namespace MongoApi.Controllers
{
    public static class RestUtil
    {
        public static T Response<T>(T item)
        {
            if (item == null || IsEmptyList(item))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        private static bool IsEmptyList(object item)
        {
            return item is IList && ((IList)item).Count == 0;
        }
    }
}