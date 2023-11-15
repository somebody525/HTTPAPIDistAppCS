using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HTTPAPIDistAppServer
{
    class NewsAgent {
        private string name;
        private string url;

      //  private Url httpUrl;

        private NewsParser newsParser;
        private List<News> newsList = new List<News>();

        public NewsAgent(string name, string url) {
            this.name = name;
            this.url = url;
            if (url.StartsWith("https://data.gmanetwork.com/"))
                newsParser = new NewsParserGMA();
            else if (url.StartsWith("https://cnnphilippines.com/"))
                newsParser = new NewsParserCNN();
        }

        internal async System.Threading.Tasks.Task Download() {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, x-gzip, deflate");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            string content = "";
            try
            {
                content = await client.GetStringAsync(url);
            }
            catch (Exception ex){
                Console.WriteLine("Download error: " + url + ": " + ex.Message);

            }
            //if (parser != null)
            //    newsList = parser.extractNews(content);
            //else
            //    newsList = new ArrayList<>();
            List<News> newsList = null;
            if (newsParser != null) {
                newsList = newsParser.ExtractNews(content);
                foreach(News news in newsList) {
                    Console.WriteLine(news.ToString());
                }
            } else {
                Console.WriteLine("TODO for " + url);
            }
        }

        internal async Task<IEnumerable<News>> DownloadAsync() {
            using var client = new HttpClient();

            /*http://zetcode.com/csharp/readwebpage/
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            var content = await client.GetStringAsync(url);*/

            //https://stackoverflow.com/questions/49828449/the-character-set-provided-in-contenttype-is-invalid
            client.DefaultRequestHeaders.Clear();
            string s = null;

            var result = await client.GetAsync(url);
            using (var sr = new StreamReader(await result.Content.ReadAsStreamAsync(), Encoding.GetEncoding("iso-8859-1"))) {
                s = sr.ReadToEnd();
            }

            try {
                newsList = new List<News>();

                if (newsParser != null) {
                    Console.WriteLine("Reaches here");
                    newsList = newsParser.ExtractNews(s.ToString());
                } else {
                    newsList = new List<News>();
                    //Console.WriteLine("NewsAgent:  NO PARSER" + url  );
                }


            } catch (Exception e) {
                Console.WriteLine("NewsAgent: " + e.Message);
            }
            return newsList;
        }
    }
}
