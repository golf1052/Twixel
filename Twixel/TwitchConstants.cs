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

        public enum Period
        {
            None,
            Week,
            Month,
            All
        }

        public enum Direction
        {
            None,
            Ascending,
            Decending
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
                return "channel_check_subscription";
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
            else if (scope == "channel_check_subscription")
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

        public static string PeriodToString(Period period)
        {
            if (period == Period.Week)
            {
                return "week";
            }
            else if (period == Period.Month)
            {
                return "month";
            }
            else if (period == Period.All)
            {
                return "all";
            }
            else
            {
                return "none";
            }
        }

        public static Period StringToPeriod(string period)
        {
            if (period == "week")
            {
                return Period.Week;
            }
            else if (period == "month")
            {
                return Period.Month;
            }
            else if (period == "all")
            {
                return Period.All;
            }
            else
            {
                return Period.None;
            }
        }

        public static string DirectionToString(Direction direction)
        {
            if (direction == Direction.Ascending)
            {
                return "asc";
            }
            else if (direction == Direction.Decending)
            {
                return "desc";
            }
            else
            {
                return "none";
            }
        }

        public static Direction StringToDirection(string direction)
        {
            if (direction == "asc")
            {
                return Direction.Ascending;
            }
            else if (direction == "desc")
            {
                return Direction.Decending;
            }
            else
            {
                return Direction.None;
            }
        }
    }
}
