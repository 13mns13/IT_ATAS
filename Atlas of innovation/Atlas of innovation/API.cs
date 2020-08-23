using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Atlas_of_innovation
{
    public class API
    {

        private string url = "http://datahack.pythonanywhere.com/";


        public JObject Method(string method, JObject _params)
        {
            string response = "";
            string @params = "";
            foreach (var x in _params)
                @params += $"{x.Key}={x.Value.ToString().Replace(',','.')}&";

            HttpWebRequest requests = (HttpWebRequest)WebRequest.Create(url + method + "?" + @params);
            requests.Method = "GET";
            using (var requestStream = requests.GetResponse().GetResponseStream())
            using (var sr = new StreamReader(requestStream))
            {
                response = sr.ReadToEnd();
            }

            return JObject.Parse(response);
        }
    }
}
