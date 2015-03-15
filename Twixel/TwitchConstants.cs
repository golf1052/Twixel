using System.Collections.Generic;

namespace TwixelAPI.Constants
{
    public static class TwitchConstants
    {
        public const string baseUrl = "https://api.twitch.tv/kraken/";
        public const string twitchAPIErrorString = "There was a Twitch API error";
        public const string v2UnsupportedErrorString = "This function is not supported by Twitch API v2.";
        public const string unknownErrorString = "Unknown error";

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

        public enum Length
        {
            None,
            Sec30,
            Sec60,
            Sec90
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
                return "";
            }
        }

        public static string ListOfScopesToStringOfScopes(List<Scope> scopes)
        {
            string scopesString = "";
            foreach (Scope scope in scopes)
            {
                scopesString += ScopeToString(scope) + " ";
            }
            return scopesString.Substring(0, scopesString.Length - 1);
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

        public static List<Scope> StringOfScopesToListOfScopes(string scopes)
        {
            List<Scope> scopesList = new List<Scope>();
            foreach (string scope in scopes.Split(' '))
            {
                scopesList.Add(StringToScope(scope));
            }
            return scopesList;
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
                return "";
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
                return "";
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

        public static int LengthToInt(Length length)
        {
            if (length == Length.Sec30)
            {
                return 30;
            }
            else if (length == Length.Sec60)
            {
                return 60;
            }
            else if (length == Length.Sec90)
            {
                return 90;
            }
            else
            {
                return 0;
            }
        }

        public static Length IntToLength(int length)
        {
            if (length == 30)
            {
                return Length.Sec30;
            }
            else if (length == 60)
            {
                return Length.Sec60;
            }
            else if (length == 90)
            {
                return Length.Sec90;
            }
            else
            {
                return Length.None;
            }
        }
    }
}
