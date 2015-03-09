using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TwixelAPI.Tests
{
    public class Twixelv2Tests
    {
        Twixel twixel;

        public Twixelv2Tests()
        {
            twixel = new Twixel(ApiKey.clientId, ApiKey.clientSecret, "http://golf1052.com", Twixel.APIVersion.v2);
        }

        [Fact]
        public async void RetrieveStreamTest()
        {
            bool streamOffline = false;
            Stream stream = null;
            // This stream may be offline so it is suggested you edit
            // this line to get a stream that is online.
            stream = await twixel.RetrieveStream("Froggen");
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
            List<Stream> topStreams = await twixel.RetrieveStreams();
            Assert.Equal(25, topStreams.Count);

            List<Stream> leagueStreams = await twixel.RetrieveStreams("League of Legends");
            Assert.Equal("League of Legends", leagueStreams[0].game);
        }

        [Fact]
        public async void RetrieveAllStreamsTest()
        {
            List<Stream> allStreams = await twixel.RetrieveAllStreams();
            List<Stream> first25 = allStreams.GetRange(0, 25);
            Stream leagueStream = first25.First((s) => s.game == "League of Legends");
            Assert.NotNull(leagueStream);
        }

        [Fact]
        public async void RetrieveFeaturedStreamsTest()
        {
            List<FeaturedStream> featuredStreams = await twixel.RetrieveFeaturedStreams();
            Assert.True(featuredStreams.Count >= 5);
        }

        [Fact]
        public async void RetrieveTeamsTest()
        {
            List<Team> teams = await twixel.RetrieveTeams();
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
            List<Video> topVideos = await twixel.RetrieveTopVideos();
            Assert.True(topVideos.Count > 0);

            List<Video> topVideosAllTime = await twixel.RetrieveTopVideos(null, Constants.TwitchConstants.Period.All, 100, 0);
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
    }
}
