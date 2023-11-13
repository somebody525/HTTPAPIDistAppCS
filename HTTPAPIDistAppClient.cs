using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

//Combined from SimpleChat and this link that showed how to http post json  in c# http://zetcode.com/csharp/httpclient/
namespace HTTPAPIDistAppServer {
    class HttpAPIDistAppClient {
        const int PORT = 8088;

        public HttpAPIDistAppClient() {


        }

        public async void read() {
            readNews();
            //capitalize();
        }

        public async void capitalize() {
            String[] dataCaps = new String[] { "one", "two" };
            var json = JsonConvert.SerializeObject(dataCaps);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://localhost" + ":" + PORT + "/capitalize";
            using var client = new HttpClient();
            Console.WriteLine(url);

            var response = client.PostAsync(url, data).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.Created) {
                Console.WriteLine(response);
            }
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            Console.WriteLine(resp);

            List<String> items = JsonConvert.DeserializeObject<List<String>>(resp);
            items.ForEach(Console.WriteLine);
        }

        public async void readNews() {
            Console.WriteLine("reached");

            List<string> urls = new List<string>();
            using (StreamReader reader = new StreamReader("urls.txt")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (!line.StartsWith("//"))
                        urls.Add(line);
                }
            }
            urls.ForEach(Console.WriteLine);

            var json = JsonConvert.SerializeObject(urls);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://localhost" + ":" + PORT + "/news";
            using var client = new HttpClient();
            Console.WriteLine(url);

            var response = client.PostAsync(url, data).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.Created) {
                Console.WriteLine(response);
            }
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            Console.WriteLine(resp);

            List<News> news = JsonConvert.DeserializeObject<List<News>>(resp);
            news.ForEach(Console.WriteLine);
        }
    }
}