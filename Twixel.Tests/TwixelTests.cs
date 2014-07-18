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
            twixel = new Twixel(ApiKey.clientId, ApiKey.clientSecret);
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
            bool streamOnline = false;
            twixel.TwixelErrorEvent += (o, e) => streamOnline = true;
            Stream stream = null;
            stream = await twixel.RetrieveStream("imaqtpie");
            if (stream != null)
            {
                Assert.Equal("League of Legends", stream.game);
            }
            else
            {
                Assert.True(streamOnline);
            }

            bool myStreamOffline = false; // will probably always be false
            twixel.TwixelErrorEvent += (o, e) => myStreamOffline = true;
            stream = await twixel.RetrieveStream("golf1052");
            Assert.True(myStreamOffline);
        }
    }
}
