using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TwixelAPI.Tests
{
    public class Twixelv2AllTests
    {
        Twixel twixel;

        public Twixelv2AllTests()
        {
            twixel = new Twixel(Secrets.ClientId, Secrets.ClientSecret, "http://golf1052.com", Twixel.APIVersion.v2);
        }

        [Fact]
        public async void RetrieveAllStreamsTest()
        {
            List<Stream> allStreams = await twixel.RetrieveAllStreams();
            List<Stream> first25 = allStreams.GetRange(0, 25);
            Stream leagueStream = first25.First((s) => s.game == "League of Legends");
            Assert.NotNull(leagueStream);
        }
    }
}
