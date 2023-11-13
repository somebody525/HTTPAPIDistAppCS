using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HTTPAPIDistAppServer
{
    class NewsParserGMA : NewsParser {

        public List<News> ExtractNews(String content) {
            List<News> list = new List<News>();
 
            int start = content.IndexOf("<item>");
            content = content.Substring(start);
            Regex titlePat = new Regex("<title><!\\[CDATA\\[(.+?)]]></title>.*?<link>(.+?)</link>.*?<pubDate>(.+?)</pubDate>", RegexOptions.Singleline);
            MatchCollection matches = titlePat.Matches(content);
            foreach (Match match in matches) {
                News news = new News();
                news.company = "GMANews";
                news.title = match.Groups[1].Value;
                news.link = match.Groups[2].Value;
                try {
                    news.date = DateTime.ParseExact(match.Groups[3].Value, "ddd, dd MMM yyyy HH:mm:ss zzz", null);
                } catch (Exception e) {
                    Console.Out.WriteLine(e.StackTrace);
                }
                list.Add(news);
            }
            return list;
        }
    }
}
