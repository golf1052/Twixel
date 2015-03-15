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
            twixel = new Twixel(Secrets.ClientId, Secrets.ClientSecret, Secrets.RedirectUrl, Twixel.APIVersion.v2);
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
            User twixelTest = await twixel.RetrieveUserWithAccessToken(accessToken);
            List<User> blockedUsers = await twixelTest.RetrieveBlockedUsers();
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
    }
}
