using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwixelAPI;
using TwixelAPI.Constants;
using Xunit;

namespace TwixelAPI.Tests
{
    // Test class for user based functions (mainly from User.cs)
    // You will need to fill in this class with relevent data
    public class UserTests
    {
        Twixel twixel;

        // Your client ID and client secret;
        string clientId = ApiKey.clientId;
        string clientSecret = ApiKey.clientSecret;

        // Account access token
        string accessToken = "";

        public UserTests()
        {
            twixel = new Twixel(clientId,
                clientSecret,
                /*Whatever your redirect URL is*/ "");
            twixel.TwixelErrorEvent += twixel_TwixelErrorEvent;
        }

        void twixel_TwixelErrorEvent(object source, TwixelErrorEventArgs e)
        {
            Debug.WriteLine(e.ErrorString);
        }

        // This method returns a URL that you will need to navigate to and then log into
        // to get an access token, the access token will be in the URL. After you get the
        // access token then paste it into the access token field.
        [Fact]
        public void LoginTest()
        {
            List<TwitchConstants.Scope> scopes = new List<TwitchConstants.Scope>();
            scopes.Add(TwitchConstants.Scope.UserRead);
            scopes.Add(TwitchConstants.Scope.ChannelCheckSubscription);
            scopes.Add(TwitchConstants.Scope.ChannelCommercial);
            scopes.Add(TwitchConstants.Scope.ChannelEditor);
            scopes.Add(TwitchConstants.Scope.ChannelRead);
            scopes.Add(TwitchConstants.Scope.ChannelStream);
            scopes.Add(TwitchConstants.Scope.ChannelSubscriptions);
            scopes.Add(TwitchConstants.Scope.ChatLogin);
            scopes.Add(TwitchConstants.Scope.UserBlocksEdit);
            scopes.Add(TwitchConstants.Scope.UserBlocksRead);
            scopes.Add(TwitchConstants.Scope.UserFollowsEdit);
            scopes.Add(TwitchConstants.Scope.UserSubcriptions);

            Uri loginUri = twixel.Login(scopes);
            Debug.WriteLine(loginUri.ToString());
            Assert.True(true);
        }

        [Fact]
        public async void RetrieveUserTest()
        {
            // Enter a username to retrieve
            User user = await twixel.RetrieveUser("");
            Assert.Equal("", user.displayName);
        }

        [Fact]
        public async void RetrieveUserWithAccessToken()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Assert.True(twixelTest.authorizedScopes.Contains(TwitchConstants.Scope.UserRead));
        }

        [Fact]
        public async void RetrieveOnlineFollowedStreamsTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<Stream> onlineStreams = await twixelTest.RetrieveOnlineFollowedStreams();
            Assert.NotNull(onlineStreams);
        }

        [Fact]
        public async void RetrieveBlockedUsersTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> blockedUsers = await twixelTest.RetrieveBlockedUsers();
            Assert.NotNull(blockedUsers);
        }

        [Fact]
        public async void BlockUserTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> blockedUsers = await twixelTest.BlockUser("nightblue3");
            User nightBlue3 = null;
            foreach (User user in blockedUsers)
            {
                if (user.name == "nightblue3")
                {
                    nightBlue3 = user;
                    break;
                }
            }
            Assert.NotNull(nightBlue3);
        }

        [Fact]
        public async void UnblockUserTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> blockedUsers = await twixelTest.RetrieveBlockedUsers();
            foreach (User user in blockedUsers)
            {
                if (user.name == "nightblue3")
                {
                    blockedUsers = await twixelTest.UnblockUser("nightblue3");
                    break;
                }
            }

            User nightblue3 = null;
            foreach (User user in blockedUsers)
            {
                if (user.name == "nightblue3")
                {
                    nightblue3 = user;
                }
            }
            Assert.Null(nightblue3);
        }

        [Fact]
        public async void RetrieveSubscribersTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<Subscription> subscribers = null;
            subscribers = await twixelTest.RetriveSubscribers(false);
            Assert.Null(subscribers);
        }

        [Fact]
        public async void RetrieveSubscriberTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Subscription golf1052 = null;
            golf1052 = await twixelTest.RetrieveSubsciber("golf1052");
            Assert.Null(golf1052);
        }

        [Fact]
        public async void RetrieveSubscriptionTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Subscription doublelift = null;

            // This will return null if you are not subscribed to clgdoublelift's channel
            // If you are subscribed you should probably use another channel or change
            // the test.
            doublelift = await twixelTest.RetrieveSubscription("clgdoublelift");
            Assert.Null(doublelift);
        }

        [Fact]
        public async void RetrieveFollowingTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<Channel> following = await twixelTest.RetrieveFollowing(false);

            // This test will only work if you are following riotgames
            // and you are not following nightblue3
            Channel riotGames = null;
            foreach (Channel channel in following)
            {
                if (channel.name == "riotgames")
                {
                    riotGames = channel;
                    break;
                }
            }
            Assert.NotNull(riotGames);

            riotGames = null;
            Channel nightblue3 = null;
            riotGames = await twixelTest.RetrieveFollowing("riotgames");
            nightblue3 = await twixelTest.RetrieveFollowing("nightblue3");
            Assert.NotNull(riotGames);
            Assert.Null(nightblue3);
        }

        [Fact]
        public async void FollowChannelTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Channel qtpie = null;
            qtpie = await twixelTest.FollowChannel("imaqtpie");
            Assert.NotNull(qtpie);
        }

        [Fact]
        public async void UnfollowChannelTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            bool? unfollowedQtpie = null;
            List<Channel> following = await twixelTest.RetrieveFollowing(false);
            Channel qtpie = null;
            foreach (Channel channel in following)
            {
                if (channel.name == "imaqtpie")
                {
                    qtpie = channel;
                    break;
                }
            }
            unfollowedQtpie = await twixelTest.UnfollowChannel("imaqtpie");
            if (qtpie == null)
            {
                Assert.False((bool)unfollowedQtpie);
            }
            else
            {
                Assert.True((bool)unfollowedQtpie);
            }
        }

        [Fact]
        public async void RetrieveChannelTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Channel channel = null;
            channel = await twixelTest.RetrieveChannel();

            // You should enter your account id here
            Assert.Equal(0, channel.id);
        }

        [Fact]
        public async void RetrieveChannelEditorsTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> editors = await twixelTest.RetrieveChannelEditors();
            User user = null;
            foreach (User u in editors)
            {
                // Enter editor name here
                if (u.name == "")
                {
                    user = u;
                    break;
                }
            }
            Assert.NotNull(user);
        }

        [Fact]
        public async void UpdateChannelTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Channel channel = await twixelTest.RetrieveChannel();
            string oldChannelStatus = channel.status;
            if (oldChannelStatus == "Test 1")
            {
                channel = await twixelTest.UpdateChannel("Test 2", "");
                Assert.Equal("Test 2", channel.status);
            }
            else if (oldChannelStatus == "Test 2")
            {
                channel = await twixelTest.UpdateChannel("Test 1", "");
                Assert.Equal("Test 1", channel.status);
            }
            else
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async void ResetStreamKeyTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Channel channel = await twixelTest.RetrieveChannel();
            string oldStreamKey = twixelTest.streamKey;
            string newStreamKey = null;
            newStreamKey = await twixelTest.ResetStreamKey();
            Assert.NotNull(newStreamKey);
            Assert.True(oldStreamKey != newStreamKey);
        }

        [Fact]
        public async void StartCommercialTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            bool? startedCommercial = null;
            startedCommercial = await twixelTest.StartCommercial(TwitchConstants.Length.Sec30);
            Assert.False((bool)startedCommercial);
        }
    }
}
