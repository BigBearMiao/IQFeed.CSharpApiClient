﻿using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace IQFeed.CSharpApiClient.Lookup.Symbol.MarketSymbols
{
    public class MarketSymbolDownloader
    {
        public string GetMarketSymbolsFile(string downloadPath = null, string marketSymbolsUrl = IQFeedDefault.MarketSymbolsArchiveUrl)
        {
            var zipPath = downloadPath ?? Path.GetTempFileName();
            var extractPath = Path.GetDirectoryName(zipPath);

            using (var myWebClient = new WebClient())
            {
                myWebClient.DownloadFile(marketSymbolsUrl, zipPath);
            }

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (!entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                        continue;

                    var fileName = Path.Combine(extractPath, entry.FullName);
                    entry.ExtractToFile(fileName, true);
                    return fileName;
                }
            }

            throw new Exception("Can't extract the market symbols CSV file.");
        }
    }
}