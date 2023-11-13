using System;
using System.Collections.Generic;

namespace HTTPAPIDistAppServer
{
    public interface NewsParser {
        public List<News> ExtractNews(String content);
    }
}
