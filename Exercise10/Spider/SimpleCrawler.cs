using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Spider
{
    public class SimpleCrawler
    {
        private readonly ObservableCollection<Data> data;

        private readonly List<string> extFilter = new()
        {
            "htm",
            "html",
            "jsp",
            "aspx",
            "asp"
        };

        private readonly object uiLck;

        private readonly Hashtable urls = Hashtable.Synchronized(new Hashtable());
        private Uri baseUri;
        private int urlCount, threadCount;

        public SimpleCrawler(ObservableCollection<Data> data, object uiLck)
        {
            this.data = data;
            this.uiLck = uiLck;
            ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);
        }

        public void Begin(string entry)
        {
            lock (uiLck)
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
                lock (urls.SyncRoot)
                {
                    foreach (string url in urls.Keys)
                    {
                        if ((bool) urls[url]) continue;
                        current = url;
                    }
                }

                if (current == null && threadCount != 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (current == null)
                    break;

                urls[current] = true;
                Interlocked.Add(ref threadCount, 1);
                ThreadPool.QueueUserWorkItem(Crawl, current);
            }
        }

        private void Crawl(object url)
        {
            var current = (string) url;
            var count = Interlocked.Add(ref urlCount, 1);
            var info = DownLoad(current, count);
            lock (uiLck)
            {
                data.Add(new Data {FileName = count + ".html", Url = current, Error = info.Error});
            }

            Parse(info.Html, current);
            Interlocked.Decrement(ref threadCount);
        }

        private Data DownLoad(string url, int count)
        {
            var info = new Data {Url = url, Error = "", FileName = "", Html = ""};
            try
            {
                var webClient = new WebClient {Encoding = Encoding.UTF8};
                var html = webClient.DownloadString(url);
                var fileName = "./html/" + count + ".html";
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