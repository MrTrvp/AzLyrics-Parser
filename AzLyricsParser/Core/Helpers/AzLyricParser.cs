using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;     
using AngleSharp.Parser.Html;
using AzLyricParser.Core.Models;

namespace AzLyricParser.Core.Helpers
{
    public static class AzLyricParser
    {
        private static readonly HtmlParser Parser;

        private static readonly string ByDelimiter;
        private static readonly string UsageComment;
        static AzLyricParser()
        {
            Parser = new HtmlParser();
            ByDelimiter = "  by ";
            UsageComment = "Usage of azlyrics.com content";
        }

        public static string ParseLyrics(string source)
        {
            using (var document = Parser.Parse(source))
            {                   
                var lyrics = document.QuerySelectorAll(".container.main-page > .row > .text-center > div")
                    .First(element => element.InnerHtml.Contains(UsageComment)).TextContent;

                return lyrics.Trim();
            }
        }

        public static SearchResult ParseSearch(string source, HttpClient client)
        {   
            using (var document = Parser.Parse(source))
            {
                var headerElement = document.QuerySelectorAll(".panel-heading")
                    .First(element => element.FirstChild.TextContent == "Song results:");

                var tableElement = headerElement.ParentElement.QuerySelector("tbody");   
                if (tableElement == null)
                    return new SearchResult(null);

                var entries = new List<Song>();
                foreach (var songElement in tableElement.QuerySelectorAll("tr > td.visitedlyr"))
                {
                    var element = songElement.QuerySelector("a > b");
                    var name = element.TextContent;
                    var nextElement = element.ParentElement;
                    var link = nextElement.Attributes["href"].Value;
                                                   
                    var nextSibling = nextElement.NextSibling;            
                    var featuring = string.Empty;  
                    if (nextSibling.TextContent != ByDelimiter)
                    {                                
                        featuring = nextSibling.TextContent;
                        featuring = featuring.Substring(1, featuring.Length - ByDelimiter.Length);
                    }
                    nextElement = nextElement.NextElementSibling;
                    var artist = nextElement.TextContent;
                    entries.Add(new Song(artist, name, featuring, link, client));
                }
                return new SearchResult(entries.ToArray());
            }
        }
    }
}         
