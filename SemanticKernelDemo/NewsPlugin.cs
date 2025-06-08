using Microsoft.SemanticKernel;
using SimpleFeedReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernelDemo
{
    public class NewsPlugin
    {
        [KernelFunction("get_news")]
        [Description("get new item for today news.")]
        [return:Description("A List of today's news.")]
        public async Task<IEnumerable<FeedItem>> GetNews(Kernel kernel, string catgeory) {
            var reader = new FeedReader();
           return await reader.RetrieveFeedAsync($"https://rss.nytimes.com/services/xml/rss/nyt/{catgeory}.xml");
        }
    }
}
