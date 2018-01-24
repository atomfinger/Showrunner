using Newtonsoft.Json;
using Showrunner.Data.Classes;
using Showrunner.Data.Interfaces;
using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Showrunner.Data.Utils
{
    public class TvmazeApi : ShowApi
    {
        private Uri baseUri = new Uri(@"http://api.tvmaze.com/");


        public async override Task<Show> SearchAsync(string showName)
        {
            return await DoShowQueryAsync(new Uri(baseUri, $@"singlesearch/shows?q={showName}"));
        }

        public async override Task<Show> FindShow(int id)
        {
            return await DoShowQueryAsync(new Uri(baseUri, $@"lookup/shows?tvrage={id}"));
        }

        public async override Task<IEnumerable<Episode>> GetEpisodes(int showId)
        {
            var result = await DoRequestAsync(new Uri(baseUri, $@"/shows/{showId}/episodes?specials=1"));
            if (string.IsNullOrWhiteSpace(result))
                return null;

            dynamic episodes = JsonConvert.DeserializeObject(result);
            if (episodes == null)
                return null;

            var converted = new List<Episode>();
            foreach (dynamic episode in episodes)
                converted.Add(ConvertToEpisode(episode));

            return converted;
        }


        #region convertions

        private Episode ConvertToEpisode(dynamic episode)
        {
            var newEpisode = new Episode()
            {
                ApiId = episode.id,
                Season = episode.season,
                Title = episode.name,
            };

            if (episode.airdate > SqlDateTime.MinValue)
                newEpisode.AirDate = episode.airdate;

            return newEpisode;
        }

        private Show ConvertToShow(dynamic show)
        {
            var newShow = new Show()
            {
                Title = show.name,
                ApiId = show.id,
            };

            if (show.premiered > SqlDateTime.MinValue)
                newShow.Premiered = show.premiered;

            return newShow;
        }

        #endregion

        #region Requests

        private async Task<Show> DoShowQueryAsync(Uri uri)
        {
            var result = await DoRequestAsync(uri);
            if (string.IsNullOrWhiteSpace(result))
                return null;

            dynamic show = JsonConvert.DeserializeObject(result);
            if (show == null)
                return null;

            return ConvertToShow(show);
        }

        private async Task<string> DoRequestAsync(Uri requestUri)
        {
            int tries = 1;
            while (tries <= 10)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                try
                {
                    WebResponse response = await request.GetResponseAsync();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        if (ex.Response is HttpWebResponse webResponse)
                        {
                            switch (webResponse.StatusCode)
                            {
                                case HttpStatusCode.NotFound:
                                    return null;
                            }
                        }
                    }

                    string response = string.Empty;
                    var responseStream = ex.Response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                        response = sr.ReadToEnd();

                    if (response.Contains("429"))
                    {
                        Thread.Sleep(10000 * tries);
                        tries++;
                        continue;
                    }

                    if (response.Contains("404"))
                        return null;

                    throw;
                }
            }

            throw new TimeoutException("Tried 10 times");
        }

        #endregion

    }
}
