using System;
using System.Collections.Generic;
using System.Linq;
using Flurl;
using Newtonsoft.Json.Linq;
using TwixelAPI.Constants;
using Xunit;

namespace TwixelAPI.Tests
{
    public class Twixelv5Tests
    {
        Twixel twixel;

        public Twixelv5Tests()
        {
            twixel = new Twixel(Secrets.ClientId,
                "http://golf1052.com", Twixel.APIVersion.v5);
        }

        [Fact]
        public async void GetUserIdTest()
        {
            long id = await twixel.GetUserId("golf1052");
            Assert.Equal(22747608, id);
        }

        [Fact]
        public async void ClientIdTest()
        {
            // This test doesn't pass https://discuss.dev.twitch.tv/t/problem-with-client-id/6528/2
            // You know...cause Twitch sucks
            string responseString = await Twixel.GetWebData(new Uri(new Url(TwitchConstants.baseUrl)), Twixel.APIVersion.v3);
            JObject responseObject = JObject.Parse(responseString);
            bool identified = (bool)responseObject["identified"];
            Assert.True(identified);
        }

        [Fact]
        public async void RetrieveChannelTest()
        {
            Channel golf1052 = await twixel.RetrieveChannel("22747608");
            Assert.Equal(22747608, golf1052.id);
            Assert.Equal("golf1052", golf1052.name);
        }

        [Fact]
        public async void RetrieveTeamsTest()
        {
            List<Team> teams = await twixel.RetrieveTeams((await twixel.GetUserId("TSM_TheOddOne")).ToString());
            Team tsm = teams.FirstOrDefault((team) => team.name == "solomid");
            Assert.NotNull(tsm);
        }

        [Fact]
        public async void RetrieveEmoticonsTest()
        {
            List<Emoticon> emotes = await twixel.RetrieveEmoticons();
            Emoticon rotations = emotes.FirstOrDefault((emote) => emote.regex == "ognTSM");
            Assert.NotNull(rotations);
        }

        [Fact]
        public async void RetrieveBadgesTest()
        {
            List<Badge> badges = await twixel.RetrieveBadges("22747608");
            Assert.Equal(7, badges.Count);
        }

        [Fact]
        public async void RetrieveTopGamesTest()
        {
            Total<List<Game>> topGames = await twixel.RetrieveTopGames();
            Game league = topGames.wrapped.FirstOrDefault((game) => game.name == "League of Legends");
            Assert.NotNull(league);

            Total<List<Game>> nextGames = await twixel.RetrieveTopGames(25);
            league = nextGames.wrapped.FirstOrDefault((game) => game.name == "League of Legends");
            Assert.Null(league);
        }

        [Fact]
        public async void RetrieveIngestsTest()
        {
            List<Ingest> ingests = await twixel.RetrieveIngests();
            Ingest newYork = ingests.FirstOrDefault((ingest) => ingest.name == "US East: New York, NY");
            Assert.NotNull(newYork);
        }

        [Fact]
        public async void SearchChannelsTest()
        {
            Total<List<Channel>> searchedChannels = await twixel.SearchChannels("golf1052");
            Channel channel = searchedChannels.wrapped.FirstOrDefault((c) => c.name == "golf1052");
            Assert.NotNull(channel);
        }

        [Fact]
        public async void SearchStreamsTest()
        {
            Total<List<Stream>> searchedStreams = await twixel.SearchStreams("league", hls: true);
            Stream leagueStream = searchedStreams.wrapped.FirstOrDefault((stream) => stream.game == "League of Legends");
            Assert.NotNull(leagueStream);
        }

        [Fact]
        public async void SearchGamesTest()
        {
            List<SearchedGame> searchedGames = await twixel.SearchGames("League", true);
            SearchedGame league = searchedGames.FirstOrDefault((game) => game.name == "League of Legends");
            Assert.NotNull(league);
        }

        [Fact]
        public async void RetrieveStreamTest()
        {
            bool streamOffline = false;
            Stream stream = null;
            // This stream may be offline so it is suggested you edit
            // this line to get a stream that is online.
            stream = await twixel.RetrieveStream((await twixel.GetUserId("monstercat")).ToString());
            if (stream != null)
            {
                Assert.Equal("Music", stream.game);
            }
            else
            {
                Assert.True(streamOffline);
            }

            TwixelException ex = await Assert.ThrowsAsync<TwixelException>(async () => await twixel.RetrieveStream("ETdfdsfjldsjdfs"));
            Assert.IsType(typeof(TwixelException), ex);

            ex = await Assert.ThrowsAsync<TwixelException>(async () => await twixel.RetrieveStream("22747608"));
            Assert.Equal("22747608 is offline", ex.Message);
        }

        [Fact]
        public async void RetrieveStreamsTest()
        {
            Total<List<Stream>> topStreams = await twixel.RetrieveStreams();
            Assert.Equal(25, topStreams.wrapped.Count);

            Total<List<Stream>> leagueStreams = await twixel.RetrieveStreams("League of Legends");
            Assert.Equal("League of Legends", leagueStreams.wrapped[0].game);
        }

        [Fact]
        public async void RetrieveFeaturedStreamsTest()
        {
            List<FeaturedStream> featuredStreams = await twixel.RetrieveFeaturedStreams();
            foreach (FeaturedStream stream in featuredStreams)
            {
                stream.CleanTextString();
            }
            Assert.True(featuredStreams.Count >= 5);
        }

        [Fact]
        public async void RetrieveStreamsSummaryTest()
        {
            Dictionary<string, int> summary = await twixel.RetrieveStreamsSummary();
            Assert.True(summary["Viewers"] > 1);
            Assert.True(summary["Channels"] > 1);
        }

        [Fact]
        public async void RetrieveAllTeamsTest()
        {
            List<Team> teams = await twixel.RetrieveTeams();
            Team staff = teams.FirstOrDefault((team) => team.name == "staff");
            Assert.NotNull(staff);
        }

        [Fact]
        public async void RetrieveTeamTest()
        {
            Team staff = await twixel.RetrieveTeam("staff");
            Assert.Equal("staff", staff.name);
        }

        [Fact]
        public async void RetrieveUserTest()
        {
            User golf1052 = await twixel.RetrieveUser("22747608");
            Assert.Equal("golf1052", golf1052.displayName);
        }

        [Fact]
        public async void RetrieveVideoTest()
        {
            Video video = await twixel.RetrieveVideo("v50248541");
            Assert.Equal(27, video.length);
            Assert.Equal("League of Legends", video.game);
        }

        [Fact]
        public async void RetrieveTopVideosTest()
        {
            List<Video> topVideos = await twixel.RetrieveTopVideos();
            Assert.True(topVideos.Count > 0);

            List<Video> topVideosAllTime = await twixel.RetrieveTopVideos(null,
                Constants.TwitchConstants.Period.All, 0, 100);
            Video theOddOne = topVideosAllTime.FirstOrDefault((video) => video.title == "Hotshot gets baited");
            Assert.NotNull(theOddOne);
        }

        [Fact]
        public async void RetrieveVideosTest()
        {
            Total<List<Video>> videos = await twixel.RetrieveVideos("22747608");
            Assert.Equal(27, videos.wrapped[0].length);
            Assert.Equal("League of Legends", videos.wrapped[0].game);
        }
    }
}
