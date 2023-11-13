using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HTTPAPIDistAppServer
{
    class NewsParserCNN : NewsParser {
        public List<News> ExtractNews(string content) {
            List<News> list = new List<News>();

            int start = content.IndexOf("Latest Articles");
            int end = content.IndexOf("</body>", start);
            content = content.Substring(start, end - start);
            Regex titlePat = new Regex("<a href=\"(/news/(\\d{4}/\\d+/\\d+)/.+?)\">(.+?)</a>", RegexOptions.Singleline);
            Regex otherPat = new Regex("<span>(.+?)</span>", RegexOptions.Singleline);
            MatchCollection matches = titlePat.Matches(content);
            foreach (Match match in matches) {
                News news = new News();
                news.company = "CNNph";
                news.title = match.Groups[3].Value;
                if (news.title.Contains("<span>")) {
                    Match titleMatch = otherPat.Match(news.title);
                    if (titleMatch.Success) {
                        news.title = titleMatch.Groups[1].Value.Trim();
                    }
                } else if (news.title.Contains("<picture>")) {
                    continue;
                }
                news.link = "https://cnnphilippines.com" + match.Groups[1].Value;
                try {
                    news.date = DateTime.ParseExact(match.Groups[2].Value, "yyyy/M/d", null);
                } catch (Exception e) {
                    Console.Out.WriteLine(e.StackTrace);
                }
                list.Add(news);
            }
            return list;
        }
    }
}
