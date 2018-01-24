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

        public async override Task<IEnumerable<Show>> SearchShow(string search)
        {
            return await DoShowQuery(new Uri(baseUri, $@"search/shows?q={search}"));
        }

        public async override Task<Show> SingleShowSearch(string showName)
        {
            return await DoSingleShowQuery(new Uri(baseUri, $@"singlesearch/shows?q={showName}"));
        }

        public async override Task<Show> FindShow(int id)
        {
            return await DoSingleShowQuery(new Uri(baseUri, $@"lookup/shows?tvrage={id}"));
        }

        public async override Task<IEnumerable<Episode>> GetEpisodes(int showId)
        {
            var result = await Task.Run(() => DoRequestAsync(new Uri(baseUri, $@"/shows/{showId}/episodes?specials=1")));
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

        public async override Task<IEnumerable<Episode>> GetScheduledEpisodes(DateTime date, string countryCode)
        {
            var result = await Task.Run(() => DoRequestAsync(new Uri(baseUri, $@"schedule?country={countryCode}&date={date.ToString("yyyy-MM-dd")}")));
            dynamic episodes = JsonConvert.DeserializeObject(result);
            if (episodes == null)
                return null;

            var converted = new List<Episode>();
            foreach (dynamic episode in episodes)
            {
                Episode e = ConvertToEpisode(episode);
                e.Show = ConvertToShow(episode.show);
                converted.Add(e);
            }

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
                Number = episode.number,
            };

            if (episode.airdate > SqlDateTime.MinValue)
                newEpisode.AirDate = episode.airdate;

            newEpisode.Summary = episode.summary;

            return newEpisode;
        }

        private Show ConvertToShow(dynamic show)
        {
            var newShow = new Show()
            {
                Title = show.name,
                ApiId = show.id,
            };

            var externals = show.externals.ToObject <Dictionary<string, string>>();
            if (externals.ContainsKey("imdb"))
                newShow.ImbdId = externals["imdb"];

            if (show.premiered > SqlDateTime.MinValue)
                newShow.Premiered = show.premiered;

            if (show.network != null)
                newShow.Network = ConvertToNetwork(show.network);

            if (show.genres != null && show.genres.Count > 0)
            {
                foreach (var genre in show.genres.ToObject<List<string>>())
                    newShow.Genres.Add(ConvertToGenre(genre));
            }

            if (show.rating != null)
                newShow.Rating = show.rating.average;

            newShow.Summary = show.summary;

            return newShow;
        }

        private Network ConvertToNetwork(dynamic network)
        {
            var newNetwork = new Network()
            {
                ApiId = network.id,
                Name = network.name
            };

            return newNetwork;
        }

        private Genre ConvertToGenre(string genre)
        {
            return new Genre() { Description = genre };
        }

        #endregion

        #region Requests

        private async Task<IEnumerable<Show>> DoShowQuery(Uri uri)
        {
            var shows = new List<Show>();
            var result = await Task.Run(() => DoRequestAsync(uri));
            if (result == null)
                return shows;

            dynamic dynamicShows = JsonConvert.DeserializeObject(result);

            foreach (var item in dynamicShows.ToObject<List<dynamic>>())
                shows.Add(ConvertToShow(item.show));

            return shows;
        }

        private async Task<Show> DoSingleShowQuery(Uri uri)
        {
            var result = await Task.Run(() => DoRequestAsync(uri));
            if (string.IsNullOrWhiteSpace(result))
                return null;

            dynamic show = JsonConvert.DeserializeObject(result);
            if (show == null)
                return null;

            return ConvertToShow(show);
        }

        private string DoRequestAsync(Uri requestUri)
        {
            int tries = 1;
            while (tries <= 10)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Proxy = null;

                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                return reader.ReadToEnd();
                            }
                        }
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
                        Thread.Sleep(10000);
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
