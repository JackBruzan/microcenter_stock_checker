using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MicrocenterStockChecker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var client = new MongoClient("mongodb+srv://jack:Dinos%40ur66@cluster0.lqdcf.mongodb.net/test");
                var urls = client.GetDatabase("stocktracker").GetCollection<BsonDocument>("microcenter_urls").Find(new BsonDocument()).FirstOrDefault().AsBsonDocument;
                List<URLInfo> shitsToDownload = new List<URLInfo>();
                URLInfo urlInfo = new URLInfo(urls["Products"].AsString, urls["Condition"].AsString, urls["URL"].AsString);
                shitsToDownload.Add(urlInfo);

                using HttpClient httpClient = new HttpClient();
                foreach(var shit in shitsToDownload) {
                    var resultsFromWebsite = await httpClient.GetAsync(shit.URL);
                    HtmlDocument fuck = new HtmlDocument();
                    fuck.LoadHtml(resultsFromWebsite.Content.ReadAsStringAsync().Result.ToString());
                    var nodes = fuck.DocumentNode.SelectNodes("//li[@class='product_wrapper']//div//a[contains(@id,'hypProduct')]");
                    foreach (var item in nodes) {
                        Console.WriteLine(item.Attributes["data-name"].Value);
                        Console.WriteLine(item.Attributes["data-price"].Value);
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
