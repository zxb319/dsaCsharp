using System.Collections.Generic;
using System.Net;

namespace csprj.dsa {
    public class Requests {
        public static HttpWebResponse post(string url, Dictionary<string, object> parameters = null, Dictionary<string, object> data = null, Dictionary<string, object> json = null) {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            return new HttpWebResponse();
        }
    }
}