using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TwixelAPI;
using TwixelAPI.Constants;
using Xunit;

namespace TwixelAPI.Tests
{
    public class TwixelTests
    {
        Twixel twixel;

        public TwixelTests()
        {
            twixel = new Twixel(ApiKey.clientId, ApiKey.clientSecret, "http://golf1052.com");
            twixel.TwixelErrorEvent += twixel_TwixelErrorEvent;
        }

        void twixel_TwixelErrorEvent(object source, TwixelErrorEventArgs e)
        {
            Debug.WriteLine(e.ErrorString);
        }

        [Fact]
        public async void RetrieveTopGamesTest()
        {
            List<Game> topGames = await twixel.RetrieveTopGames(false);
            Game league = null;
            foreach (Game game in topGames)
            {
                if (game.name == "League of Legends")
                {
                    league = game;
                    break;
                }
            }

            Assert.NotNull(league);

            List<Game> nextGames = await twixel.RetrieveTopGames(true);
            league = null;
            foreach (Game game in nextGames)
            {
                if (game.name == "League of Legends")
                {
                    league = game;
                    break;
                }
            }

            Assert.Null(league);

            bool eventWasFired = false;
            twixel.TwixelErrorEvent += (o, e) => eventWasFired = true;
            List<Game> errorGames = await twixel.RetrieveTopGames(101, false);
            Assert.True(eventWasFired);
        }

        [Fact]
        public async void RetrieveStreamTest()
        {
            bool streamOffline = false;
            twixel.TwixelErrorEvent += (o, e) => streamOffline = true;
            Stream stream = null;
            // This stream may be offline so it is suggested you edit
            // this line to get a stream that is online.
            stream = await twixel.RetrieveStream("imaqtpie");
            if (stream != null)
            {
                Assert.Equal("League of Legends", stream.game);
            }
            else
            {
                Assert.True(streamOffline);
            }

            bool myStreamOffline = false; // will probably always be false
            twixel.TwixelErrorEvent += (o, e) => myStreamOffline = true;
            stream = await twixel.RetrieveStream("golf1052");
            Assert.True(myStreamOffline);
        }

        [Fact]
        public async void RetrieveStreamsTest()
        {
            List<Stream> topStreams = await twixel.RetrieveStreams(false);
            Assert.Equal(25, topStreams.Count);

            List<Stream> leagueStreams = await twixel.RetrieveStreams("League of Legends");
            Assert.Equal(leagueStreams[0].game, "League of Legends");
        }

        [Fact]
        public async void RetrieveFeaturedStreamsTest()
        {
            List<FeaturedStream> featuredStreams = await twixel.RetrieveFeaturedStreams();
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
        public async void RetrieveChatTest()
        {
            List<Uri> chatLinks = await twixel.RetrieveChat("golf1052");
            Assert.Equal(3, chatLinks.Count);
        }

        [Fact]
        public async void RetrieveEmoticonsTest()
        {
            List<Emoticon> emotes = await twixel.RetrieveEmoticons();
            Emoticon rotations = null;
            foreach (Emoticon emote in emotes)
            {
                if (emote.regex == "ognTSM")
                {
                    rotations = emote;
                    break;
                }
            }

            Assert.NotNull(rotations);
        }

        [Fact]
        public async void SearchGamesTest()
        {
            List<SearchedGame> searchedGames = await twixel.SearchGames("League", true);
            SearchedGame league = null;
            foreach (SearchedGame game in searchedGames)
            {
                if (game.name == "League of Legends")
                {
                    league = game;
                    break;
                }
            }

            Assert.NotNull(league);
        }

        [Fact]
        public async void RetrieveTeamsTest()
        {
            List<Team> teams = await twixel.RetrieveTeams(false);
            Team staff = null;
            foreach (Team team in teams)
            {
                if (team.name == "staff")
                {
                    staff = team;
                    break;
                }
            }

            Assert.NotNull(staff);
        }

        [Fact]
        public async void RetrieveTeamTest()
        {
            Team staff = await twixel.RetrieveTeam("staff");
            Assert.Equal("staff", staff.name);
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
            List<Video> topVideos = await twixel.RetrieveTopVideos(false);
            Assert.True(topVideos.Count > 0);

            List<Video> topVideosAllTime = await twixel.RetrieveTopVideos(25, "", TwitchConstants.Period.All);
            Video theOddOne = null;
            foreach (Video video in topVideosAllTime)
            {
                if (video.title == "Hotshot gets baited")
                {
                    theOddOne = video;
                    break;
                }
            }
            Assert.NotNull(theOddOne);
        }

        [Fact]
        public async void RetrieveVideosTest()
        {
            List<Video> videos = await twixel.RetrieveVideos("golf1052", false);
            Assert.Equal(20, videos[0].length);
            Assert.Equal("League of Legends", videos[0].game);
        }

        [Fact]
        public async void RetrieveFollowersTest()
        {
            List<User> followers = await twixel.RetrieveFollowers("golf1052", false);
            User zeroAurora = null;
            foreach (User follower in followers)
            {
                if (follower.name == "zero_aurora")
                {
                    zeroAurora = follower;
                    break;
                }
            }
            Assert.NotNull(zeroAurora);
        }

        [Fact]
        public async void RetrieveChannelTest()
        {
            Channel golf1052 = await twixel.RetrieveChannel("golf1052");

            Assert.Equal(22747608, golf1052.id);
            Assert.Equal("golf1052", golf1052.name);
        }

        [Fact]
        public async void RetrieveIngestsTest()
        {
            List<Ingest> ingests = await twixel.RetrieveIngests();
            Ingest newYork = null;
            foreach (Ingest ingest in ingests)
            {
                if (ingest.name == "US East: New York, NY")
                {
                    newYork = ingest;
                    break;
                }
            }
            Assert.NotNull(newYork);
        }
    }
}
