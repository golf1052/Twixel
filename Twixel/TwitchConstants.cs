using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwixelAPI.Constants
{
    public static class TwitchConstants
    {
        public enum Scope
        {
            None,
            UserRead,
            UserBlocksEdit,
            UserBlocksRead,
            UserFollowsEdit,
            ChannelRead,
            ChannelEditor,
            ChannelCommercial,
            ChannelStream,
            ChannelSubscriptions,
            UserSubcriptions,
            ChannelCheckSubscription,
            ChatLogin
        }

        public static string ScopeToString(Scope scope)
        {
            if (scope == Scope.UserRead)
            {
                return "user_read";
            }
            else if (scope == Scope.UserBlocksEdit)
            {
                return "user_blocks_edit";
            }
            else if (scope == Scope.UserBlocksRead)
            {
                return "user_blocks_read";
            }
            else if (scope == Scope.UserFollowsEdit)
            {
                return "user_follows_edit";
            }
            else if (scope == Scope.ChannelRead)
            {
                return "channel_read";
            }
            else if (scope == Scope.ChannelEditor)
            {
                return "channel_editor";
            }
            else if (scope == Scope.ChannelCommercial)
            {
                return "channel_commercial";
            }
            else if (scope == Scope.ChannelStream)
            {
                return "channel_stream";
            }
            else if (scope == Scope.ChannelSubscriptions)
            {
                return "channel_subscriptions";
            }
            else if (scope == Scope.UserSubcriptions)
            {
                return "user_subscriptions";
            }
            else if (scope == Scope.ChannelCheckSubscription)
            {
                return "channel_check_subscriptions";
            }
            else if (scope == Scope.ChatLogin)
            {
                return "chat_login";
            }
            else
            {
                return "none";
            }
        }

        public static Scope StringToScope(string scope)
        {
            if (scope == "user_read")
            {
                return Scope.UserRead;
            }
            else if (scope == "user_blocks_edit")
            {
                return Scope.UserBlocksEdit;
            }
            else if (scope == "user_blocks_read")
            {
                return Scope.UserBlocksRead;
            }
            else if (scope == "user_follows_edit")
            {
                return Scope.UserFollowsEdit;
            }
            else if (scope == "channel_read")
            {
                return Scope.ChannelRead;
            }
            else if (scope == "channel_editor")
            {
                return Scope.ChannelEditor;
            }
            else if (scope == "channel_commercial")
            {
                return Scope.ChannelCommercial;
            }
            else if (scope == "channel_stream")
            {
                return Scope.ChannelStream;
            }
            else if (scope == "channel_subscriptions")
            {
                return Scope.ChannelSubscriptions;
            }
            else if (scope == "user_subscriptions")
            {
                return Scope.UserSubcriptions;
            }
            else if (scope == "channel_check_subscriptions")
            {
                return Scope.ChannelCheckSubscription;
            }
            else if (scope == "chat_login")
            {
                return Scope.ChatLogin;
            }
            else
            {
                return Scope.None;
            }
        }
    }
}
