using AngleSharp.Html.Parser;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GW2Tradz.Networking
{
    class GW2BLTC
    {
        private HttpClient httpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });


        public List<Item> ScrapeItems()
        {
            var result = new List<Item> { };
            var page = 1;
            var parser = new HtmlParser();
            while (true)
            {
                var request = httpClient.GetAsync($"https://www.gw2bltc.com/en/tp/search?sold-day-min=1&ipg=200&sort=demand&page={page++}");
                var content = request.Result.Content.ReadAsStringAsync().Result;
                var document = parser.ParseDocument(content);
                var rows = document.QuerySelectorAll("table.table-result>tbody>tr");
                if(!rows.Any())
                {
                    break;
                }
                foreach (var row in rows)
                {
                    try
                    {
                        var link = row.Children[1].Children[0].Attributes["href"].Value;
                        var id = int.Parse(Regex.Match(link, "/([0-9]+)-").Groups[1].Value);
                        var sell = int.Parse(row.Children[2].TextContent.Replace(",", ""));
                        var buy = int.Parse(row.Children[3].TextContent.Replace(",", ""));
                        var sold = int.Parse(row.Children[8].TextContent.Replace(",", ""));
                        var bought = float.Parse(row.Children[10].TextContent);
                        result.Add(new Item
                        {
                            Id = id,
                            WeekBuyVelocity = bought * 7,
                            WeekSellVelocity = sold * 7,
                            BuyPrice = buy,
                            SellPrice = sell 

                        }
                            );
                    }
                    catch
                    {
                        Debugger.Break();
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            return result;

        }
    }
}
