using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace Atlas_of_innovation
{
    class CheckINN
    {
        public string inn { set; get; }
        public CheckINN(string inn)
        {
            this.inn = inn;
        }

        private string url = "https://www.list-org.com/";
        public JObject Check()
        {
            string result = "";
            JObject jObject = new JObject();
            using (var request = new HttpRequest())
            {
                
                    HttpResponse response = request.Get(url + "search?type=inn&val=" + inn);
                    var a = response.ToString();
                    var document = new HtmlDocument();
                    document.LoadHtml(a);
                try
                {
                    document.LoadHtml(document.DocumentNode.SelectNodes(@".//div[@class='org_list']")[0].InnerHtml.ToString());
                    result = document.DocumentNode.SelectNodes(@".//a[@href]")[0].OuterHtml.ToString();
                    result = result.Substring(result.IndexOf('<'), result.IndexOf('>') - 1).Replace("<a href='/", "");
                }
                catch { }

            }
            string site = "";
            string map = "";
            using (var request = new HttpRequest()) {
                
                var response = request.Get(url + result);
                var document = new HtmlDocument();
                try
                {
                    document.LoadHtml(response.ToString());
                
                    site = document.DocumentNode.SelectNodes(@".//div[@class='sites']")[0].InnerText.ToString().Replace("Сайт:", "").Replace(" ", "").Split('\n')[0];
                
                var _map = document.DocumentNode.SelectNodes(@".//a[@class='nwra']")[0].OuterHtml.ToString().Replace("<a class='nwra' href='/companies_on_map/","").Replace("'><i class='fa fa-globe'></i> Посмотреть все на карте</a>","");
                map = _map.ToString().Split('_')[0]+", "+ _map.ToString().Split('_')[1];
                }
                catch { }
            }


            double pro = 0;
            double yandex =0;
            double count_otv = 0;
            jObject["map"] = map;
            jObject["site"] = site;
            jObject["pro"] = pro;
            jObject["count_otv"] = count_otv;

            using (var request = new HttpRequest())
            {
                var response = request.Get("https://yandex.ru/ugcpub/iznanka?text=yandex.ru");
                var json = JObject.Parse(response.ToString())["iznanka"][0]["content"];
                double positive_reviews_count = json["positive_reviews_count"].Value<double>();
                double negative_reviews_count = json["negative_reviews_count"].Value<double>();
                yandex = positive_reviews_count + negative_reviews_count;
            }
            jObject["yandex"] = yandex;
            if (site == "") return jObject;
            site = site.Replace("https://", "").Replace("http://", "");
            using (var request = new HttpRequest())
            {
                var response = request.Get("https://yandex.ru/ugcpub/iznanka?text="+site);
                var json = JObject.Parse(response.ToString())["iznanka"][0]["content"] ;
                double positive_reviews_count = 0;
                double negative_reviews_count = 0;
                try
                {
                    positive_reviews_count = json["positive_reviews_count"].Value<double>();
                    negative_reviews_count = json["negative_reviews_count"].Value<double>();
                }
                catch { }
                pro = positive_reviews_count / (positive_reviews_count+ negative_reviews_count);
                pro =  Math.Round(pro,2);
                count_otv = positive_reviews_count + negative_reviews_count;
            }
            
            jObject["pro"] = pro;
            jObject["count_otv"] = count_otv;
            return jObject;

        }
    }
}
