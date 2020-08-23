using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using xNet;
using HtmlAgilityPack;

namespace Atlas_of_innovation
{
    public class ParseSite
    {
        public string search { get; set; }
        public long link;
        public ParseSite(string search)
        {
            this.search = search;
        }

        public ParseSite(long link)
        {
            this.link = link;
            search = link.ToString();
        }

        public JObject GetRusprofile(string link)
        {
            var pageContent =   LoadPage("https://www.rusprofile.ru"+link.ToString());
            var document = new HtmlDocument();
            document.LoadHtml(pageContent);

            var result = document.DocumentNode.SelectNodes(@".//dl[@class='company-col']");

            JObject data = new JObject();
            double dd = 0;
            string dolg = "";
            result = document.DocumentNode.SelectNodes(@".//div[@class='connexion-col__num tosmall']");
                if (result==null){data["Dolg"] = 0; return data;
        }
        dolg = result[1].InnerText.Replace(" ", "").Replace("\n", "").Replace("руб.", "");
                
             
            if (dolg.Contains("млн"))
            {
                dolg = dolg.Replace("млн", "");
                dd = double.Parse(dolg);
                dd *= Math.Pow(10, 6);
            }
            else if (dolg.Contains("тыс"))
            {
                dolg = dolg.Replace("тыс", "");
                try
                {
                    dd = double.Parse(dolg);
                    dd *= Math.Pow(10, 3);
                }
                catch { dolg = dolg.Replace("менее", "").Replace(".",""); dd = double.Parse(dolg);
                    dd *= Math.Pow(10, 3);
                }
            }
            data["Dolg"] = dd;
                return data;
        }

        public JObject GetSearch()
        {
            using (var request = new HttpRequest())
            {
                try
                {
                    request.UserAgent = Http.ChromeUserAgent();
                    request.AddHeader(HttpHeader.Accept, "gzip, deflate, sdch, br");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");
                    HttpResponse response = request.Get("https://www.rusprofile.ru/ajax.php?&query=" + search + "&action=search");
                    string content = response.ToString();
                    return JObject.Parse(content);
                }
                catch { return new JObject(); }
               
            }
        }

        public string GetPatent(string name)
        {
            using (var request = new HttpRequest())
            {


                HttpResponse response = request.Get($"http://datahack.pythonanywhere.com/patent?name="+name);

                var json = JObject.Parse(response.ToString());

                return json["response"].ToString();
            }
        }

        private string LoadPage(string url)
        {
            using (var request = new HttpRequest())
            {
                request.UserAgent = Http.ChromeUserAgent();
                HttpResponse response = request.Get(url);
                string content = response.ToString();
                return content;
            }
            
        }


    }


}
