using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TwixelAPI.Tests
{
    public class Twixelv3Tests
    {
        Twixel twixel;

        public Twixelv3Tests()
        {
            twixel = new Twixel(Secrets.ClientId, Secrets.ClientSecret,
                "http://golf1052.com", Twixel.APIVersion.v3);
        }

        [Fact]
        public async void RetrieveChannelTest()
        {
            Channel golf1052 = await twixel.RetrieveChannel("golf1052");
            Assert.Equal(22747608, golf1052.id);
            Assert.Equal("golf1052", golf1052.name);
        }

        [Fact]
        public async void RetrieveTeamsTest()
        {
            List<Team> teams = await twixel.RetrieveTeams("TSM_TheOddOne");
            Team tsm = teams.FirstOrDefault((team) => team.name == "solomid");
            Assert.NotNull(tsm);
        }

        [Fact]
        public async void RetrieveChatTest()
        {
            Dictionary<string, Uri> chatLinks = await twixel.RetrieveChat("golf1052");
            Assert.Equal(3, chatLinks.Count);
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
            List<Badge> badges = await twixel.RetrieveBadges("golf1052");
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
            Assert.Equal(1, searchedChannels.total);
            Assert.NotNull(channel);
        }

        [Fact]
        public async void SearchStreamsTest()
        {
            Total<List<Stream>> searchedStreams = await twixel.SearchStreams("league");
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
            stream = await twixel.RetrieveStream("esl_lol");
            if (stream != null)
            {
                Assert.Equal("League of Legends", stream.game);
            }
            else
            {
                Assert.True(streamOffline);
            }

            TwixelException ex = await Assert.ThrowsAsync<TwixelException>(async () => await twixel.RetrieveStream("dfsfd"));
            Assert.IsType(typeof(TwitchException), ex.InnerException);
            Assert.Equal(422, ((TwitchException)ex.InnerException).Status);

            ex = await Assert.ThrowsAsync<TwixelException>(async () => await twixel.RetrieveStream("ETdfdsfjldsjdfs"));
            Assert.IsType(typeof(TwitchException), ex.InnerException);
            Assert.Equal(404, ((TwitchException)ex.InnerException).Status);

            ex = await Assert.ThrowsAsync<TwixelException>(async () => await twixel.RetrieveStream("golf1052"));
            Assert.Equal("golf1052 is offline", ex.Message);
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
        public async void FeaturedStreamTest()
        {
            List<FeaturedStream> featuredStreams = await twixel.TestFeatured();
            Assert.NotNull(featuredStreams);
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
            User golf1052 = await twixel.RetrieveUser("golf1052");
            Assert.Equal("golf1052", golf1052.displayName);
        }

        [Fact]
        public async void RetrieveVideoTest()
        {
            Video video = await twixel.RetrieveVideo("c2543719");
            Assert.Equal(20, video.length);
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
            Total<List<Video>> videos = await twixel.RetrieveVideos("golf1052");
            Assert.Equal(20, videos.wrapped[0].length);
            Assert.Equal("League of Legends", videos.wrapped[0].game);
        }
    }
}
