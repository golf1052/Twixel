using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TwixelAPI.Constants;
using Xunit;

namespace TwixelAPI.Tests
{
    public class Twixelv2UserTests
    {
        Twixel twixel;
        string accessToken = Secrets.AccessToken;

        public Twixelv2UserTests()
        {
            twixel = new Twixel(Secrets.ClientId,
                Secrets.RedirectUrl, Twixel.APIVersion.v2);
        }

        [Fact]
        public void LoginTest()
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
            List<Block> blockedUsers = await user.RetrieveBlockedUsers();
            Assert.NotNull(blockedUsers);
        }

        [Fact]
        public async void BlockUserTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            Block blockedUser = await user.BlockUser("nightblue3");
            Assert.NotNull(blockedUser);
        }

        [Fact]
        public async void UnblockUserTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<Block> blockedUsers = await user.RetrieveBlockedUsers();
            bool? unblockedUser = null;
            foreach (Block u in blockedUsers)
            {
                if (u.user.name == "nightblue3")
                {
                    unblockedUser = await user.UnblockUser("nightblue3");
                    break;
                }
            }
            if (unblockedUser != null)
            {
                Assert.True(unblockedUser);
            }
            else
            {
                Assert.True(false);
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
            User golf1052 = editors.FirstOrDefault((editor) => editor.name == "golf1052");
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
            await Assert.ThrowsAsync<TwixelException>(async () => await user.StartCommercial(TwitchConstants.CommercialLength.Sec30));
        }

        [Fact]
        public async void RetrieveFollowersTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            Total<List<Follow<User>>> followers = await user.RetrieveFollowers();
            User zeroAurora = followers.wrapped.FirstOrDefault((follower) => follower.wrapped.name == "zero_aurora").wrapped;
            Assert.NotNull(zeroAurora);
        }

        [Fact]
        public async void RetrieveFollowingTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            Total<List<Follow<Channel>>> following = await user.RetrieveFollowing();

            // This test will only work if you are following chaoxlol
            // and you are not following nightblue3
            Follow<Channel> chaoxlol = following.wrapped.FirstOrDefault((follow) => follow.wrapped.name == "chaoxlol");
            Assert.NotNull(chaoxlol.wrapped);

            chaoxlol = null;
            chaoxlol = await user.RetrieveFollowing("chaoxlol");
            await Assert.ThrowsAsync<TwixelException>(async () => await user.RetrieveFollowing("nightblue3"));
            Assert.NotNull(chaoxlol.wrapped);
        }

        [Fact]
        public async void FollowChannelTest()
        {
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            Follow<Channel> qtpie = await twixelTest.FollowChannel("imaqtpie");
            Assert.NotNull(qtpie.wrapped);
        }

        [Fact]
        public async void UnfollowChannelTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            bool? unfollowedQtpie = null;
            Total<List<Follow<Channel>>> following = await user.RetrieveFollowing();
            Follow<Channel> qtpie = following.wrapped.FirstOrDefault((follow) => follow.wrapped.name == "imaqtpie");
            Assert.NotNull(qtpie.wrapped);
            unfollowedQtpie = await user.UnfollowChannel("imaqtpie");
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
        public async void RetrieveSubscribersTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            await Assert.ThrowsAsync<TwixelException>(async () => await user.RetriveSubscribers());
        }

        [Fact]
        public async void RetrieveSubscriberTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(Secrets.SecondAccessToken);
            await Assert.ThrowsAsync<TwixelException>(async () => await user.RetrieveSubsciber("golf1052"));
        }

        [Fact]
        public async void RetrieveSubscriptionTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            // This will throw an exception if you are not subscribed to clgdoublelift's channel
            // If you are subscribed you should probably use another channel or change
            // the test.
            Subscription<Channel> doublelift = await user.RetrieveSubscription("clgdoublelift");
            Assert.Null(doublelift);
        }

        [Fact]
        public async void RetrieveOnlineFollowedStreamsTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<Stream> onlineStreams = await user.RetrieveOnlineFollowedStreams();
            Assert.NotNull(onlineStreams);
        }

        [Fact]
        public async void RetrieveFollowedVideosTest()
        {
            User user = await twixel.RetrieveUserWithAccessToken(accessToken);
            await Assert.ThrowsAsync<TwixelException>(async () => await user.RetrieveFollowedVideos());
        }
    }
}
