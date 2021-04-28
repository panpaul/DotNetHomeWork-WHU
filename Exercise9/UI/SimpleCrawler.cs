using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace UI
{
    public class SimpleCrawler
    {
        private readonly ObservableCollection<Data> data;

        private readonly object dataLock;

        private readonly List<string> extFilter = new()
        {
            "htm",
            "html",
            "jsp",
            "aspx",
            "asp"
        };

        private readonly Hashtable urls = new();
        private Uri baseUri;
        private int count;

        public SimpleCrawler(ObservableCollection<Data> data, object dataLock)
        {
            this.data = data;
            this.dataLock = dataLock;
        }

        public void Crawl(string entry)
        {
            lock (dataLock)
            {
                data.Clear();
            }

            // entry info
            var uri = new Uri(entry);
            baseUri = new Uri(uri.Scheme + "://" + uri.Host);
            urls.Add(entry, false);

            while (true)
            {
                string current = null;
                foreach (string url in urls.Keys)
                {
                    if ((bool) urls[url]) continue;
                    current = url;
                }

                if (current == null || count > 500) break;

                var info = DownLoad(current);
                urls[current] = true;
                lock (dataLock)
                {
                    data.Add(new Data {FileName = count + ".html", Url = current});
                }

                count++;
                Parse(info.Html, current);
                //Thread.Sleep(500);
            }
        }

        private Data DownLoad(string url)
        {
            var info = new Data {Url = url, Error = "", FileName = "", Html = ""};
            try
            {
                var webClient = new WebClient {Encoding = Encoding.UTF8};
                var html = webClient.DownloadString(url);
                var fileName = count + ".html";
                File.WriteAllText(fileName, html, Encoding.UTF8);

                info.FileName = fileName;
                info.Html = html;
                return info;
            }
            catch (Exception ex)
            {
                info.Error = ex.Message;
                return info;
            }
        }

        private void Parse(string html, string baseUrl)
        {
            var strRef = @"(href|HREF)[]*=[]*[""'][^""'#>]+[""']";
            var matches = new Regex(strRef).Matches(html);
            foreach (Match match in matches)
            {
                strRef = match.Value[(match.Value.IndexOf('=') + 1)..]
                    .Trim('"', '\"', '#', '>');

                if (strRef.Length == 0) continue;

                // handle relative
                Uri uri;
                if (!strRef.StartsWith("http://") && !strRef.StartsWith("https://"))
                    uri = new Uri(new Uri(baseUrl), strRef);
                else
                    uri = new Uri(strRef);
                // same site
                if (!baseUri.IsBaseOf(uri)) continue;
                // page type
                var ext = uri.AbsolutePath;
                if (ext[^1] != '/')
                {
                    ext = ext[(ext.IndexOf('.') + 1)..];
                    if (!extFilter.Contains(ext)) continue;
                }

                urls[uri.AbsoluteUri] ??= false;
            }
        }
    }
}