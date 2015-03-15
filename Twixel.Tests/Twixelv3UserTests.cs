using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TwixelAPI.Constants;
using Xunit;

namespace TwixelAPI.Tests
{
    public class Twixelv3UserTests
    {
        Twixel twixel;
        string accessToken = Secrets.AccessToken;

        public Twixelv3UserTests()
        {
            twixel = new Twixel(Secrets.ClientId, Secrets.ClientSecret, Secrets.RedirectUrl, Twixel.APIVersion.v3);
        }

        [Fact]
        public async void LoginTest()
        {
            // Collect scopes
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

            Uri uri = twixel.Login(scopes);
            Debug.WriteLine(uri.ToString());
            Assert.True(true);
        }

        [Fact]
        public async void RetrieveUserWithAccessToken()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            Assert.True(user.authorizedScopes.Contains(TwitchConstants.Scope.UserRead));
        }

        [Fact]
        public async void RetrieveBlockedUsersTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> blockedUsers = await user.RetrieveBlockedUsers();
            Assert.NotNull(blockedUsers);
        }

        [Fact]
        public async void BlockUserTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> blockedUsers = await user.BlockUser("nightblue3");
            User nightBlue3 = blockedUsers.FirstOrDefault((u) => u.name == "nightblue3");
            Assert.NotNull(nightBlue3);
        }

        [Fact]
        public async void UnblockUserTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> blockedUsers = await user.RetrieveBlockedUsers();
            foreach (User u in blockedUsers)
            {
                if (user.name == "nightblue3")
                {
                    blockedUsers = await user.UnblockUser("nightblue3");
                    break;
                }
            }

            TwixelException ex = await Assert.ThrowsAsync<TwixelException>(async () => await user.UnblockUser("nightblue3"));
        }

        [Fact]
        public async void RetrieveChannelTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            Channel channel = await user.RetrieveChannel();
            Assert.Equal(22747608, channel.id);
        }

        [Fact]
        public async void RetrieveChannelEditorsTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(Secrets.SecondAccessToken);
            List<User> editors = await twixelTest.RetrieveChannelEditors();
            // I probably won't be an editor to your channel, might want to edit this.
            User golf1052 = null;
            foreach (User user in editors)
            {
                if (user.name == "golf1052")
                {
                    golf1052 = user;
                    break;
                }
            }
            Assert.NotNull(golf1052);
        }

        [Fact]
        public async void UpdateChannelTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            Channel channel = await user.UpdateChannel("Test 1", "", 0);
            string oldChannelStatus = channel.status;
            if (oldChannelStatus == "Test 1")
            {
                channel = await user.UpdateChannel("Test 2", "", 0);
                Assert.Equal("Test 2", channel.status);
            }
            else if (oldChannelStatus == "Test 2")
            {
                channel = await user.UpdateChannel("Test 1", "");
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
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            Channel channel = await user.RetrieveChannel();
            string oldStreamKey = user.streamKey;
            string newStreamKey = null;
            newStreamKey = await user.ResetStreamKey();
            Assert.NotNull(newStreamKey);
            Assert.True(oldStreamKey != newStreamKey);
        }

        [Fact]
        public async void StartCommercialTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            await Assert.ThrowsAsync<TwixelException>(async () => await user.StartCommercial(TwitchConstants.Length.Sec30));
        }

        [Fact]
        public async void RetrieveFollowingTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Total<List<Follow<Channel>>> following = await twixelTest.RetrieveFollowing();

            // This test will only work if you are following riotgames
            // and you are not following nightblue3
            Channel riotGames = null;
            //foreach (Channel channel in following.wrapped)
            //{
            //    if (channel.name == "riotgames")
            //    {
            //        riotGames = channel;
            //        break;
            //    }
            //}
            Assert.NotNull(riotGames);

            //riotGames = null;
            //Channel nightblue3 = null;
            //riotGames = await twixelTest.RetrieveFollowing("riotgames");
            //nightblue3 = await twixelTest.RetrieveFollowing("nightblue3");
            //Assert.NotNull(riotGames);
            //Assert.Null(nightblue3);
        }

        //[Fact]
        //public async void RetrieveOnlineFollowedStreamsTest()
        //{
        //    User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
        //    List<Stream> onlineStreams = await twixelTest.RetrieveOnlineFollowedStreams();
        //    Assert.NotNull(onlineStreams);
        //}

        

        //[Fact]
        //public async void RetrieveSubscribersTest()
        //{
        //    User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
        //    List<Subscription> subscribers = null;
        //    subscribers = await twixelTest.RetriveSubscribers(false);
        //    Assert.Null(subscribers);
        //}

        //[Fact]
        //public async void RetrieveSubscriberTest()
        //{
        //    User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
        //    Subscription golf1052 = null;
        //    golf1052 = await twixelTest.RetrieveSubsciber("golf1052");
        //    Assert.Null(golf1052);
        //}

        //[Fact]
        //public async void RetrieveSubscriptionTest()
        //{
        //    User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
        //    Subscription doublelift = null;

        //    // This will return null if you are not subscribed to clgdoublelift's channel
        //    // If you are subscribed you should probably use another channel or change
        //    // the test.
        //    doublelift = await twixelTest.RetrieveSubscription("clgdoublelift");
        //    Assert.Null(doublelift);
        //}

        //[Fact]
        //public async void FollowChannelTest()
        //{
        //    User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
        //    Channel qtpie = null;
        //    qtpie = await twixelTest.FollowChannel("imaqtpie");
        //    Assert.NotNull(qtpie);
        //}

        //[Fact]
        //public async void UnfollowChannelTest()
        //{
        //    User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
        //    bool? unfollowedQtpie = null;
        //    List<Channel> following = await twixelTest.RetrieveFollowing(false);
        //    Channel qtpie = null;
        //    foreach (Channel channel in following)
        //    {
        //        if (channel.name == "imaqtpie")
        //        {
        //            qtpie = channel;
        //            break;
        //        }
        //    }
        //    unfollowedQtpie = await twixelTest.UnfollowChannel("imaqtpie");
        //    if (qtpie == null)
        //    {
        //        Assert.False((bool)unfollowedQtpie);
        //    }
        //    else
        //    {
        //        Assert.True((bool)unfollowedQtpie);
        //    }
        //}
    }
}
