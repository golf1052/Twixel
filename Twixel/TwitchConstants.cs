using System.Collections.Generic;

namespace TwixelAPI.Constants
{
    /// <summary>
    /// Contains Twitch constants
    /// </summary>
    public static class TwitchConstants
    {
        /// <summary>
        /// Base URL
        /// </summary>
        public const string baseUrl = "https://api.twitch.tv/kraken/";

        /// <summary>
        /// General Twitch API error
        /// </summary>
        public const string twitchAPIErrorString = "There was a Twitch API error";

        /// <summary>
        /// Twitch API v2 unsupported method error
        /// </summary>
        public const string v2UnsupportedErrorString = "This method is not supported by Twitch API v2.";

        /// <summary>
        /// Unknown error
        /// </summary>
        public const string unknownErrorString = "Unknown error";

        /// <summary>
        /// Scopes
        /// </summary>
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

        /// <summary>
        /// Time periods
        /// </summary>
        public enum Period
        {
            None,
            Week,
            Month,
            All
        }

        /// <summary>
        /// Sort directions
        /// </summary>
        public enum Direction
        {
            None,
            Ascending,
            Decending
        }

        /// <summary>
        /// Sort types
        /// </summary>
        public enum SortBy
        {
            None,
            CreatedAt,
            LastBroadcast
        }

        /// <summary>
        /// Commercial lengths
        /// </summary>
        public enum CommercialLength
        {
            None,
            Sec30,
            Sec60,
            Sec90,
            Sec120,
            Sec150,
            Sec180
        }

        /// <summary>
        /// Converts a scope to a string representation of that scope
        /// </summary>
        /// <param name="scope">Scope</param>
        /// <returns>A string representation of the given scope</returns>
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
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts a list of scopes to a string of scopes each seperated by spaces
        /// </summary>
        /// <param name="scopes">List of scopes</param>
        /// <returns>A string of scopes</returns>
        public static string ListOfScopesToStringOfScopes(List<Scope> scopes)
        {
            string scopesString = "";
            foreach (Scope scope in scopes)
            {
                scopesString += ScopeToString(scope) + " ";
            }
            return scopesString.Substring(0, scopesString.Length - 1);
        }

        /// <summary>
        /// Converts a string representation of a scope to a scope
        /// </summary>
        /// <param name="scope">String representation of a scope</param>
        /// <returns>A scope</returns>
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

        /// <summary>
        /// Converts a string of scopes which are seperated by spaces to a list of scopes
        /// </summary>
        /// <param name="scopes">String of scopes seperated by spaces</param>
        /// <returns>A list of scopes</returns>
        public static List<Scope> StringOfScopesToListOfScopes(string scopes)
        {
            List<Scope> scopesList = new List<Scope>();
            foreach (string scope in scopes.Split(' '))
            {
                scopesList.Add(StringToScope(scope));
            }
            return scopesList;
        }

        /// <summary>
        /// Converts a period to a string representation of that period
        /// </summary>
        /// <param name="period">Period</param>
        /// <returns>A string representation of the given period</returns>
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
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts a string representation of a period to a period
        /// </summary>
        /// <param name="period">String representation of a period</param>
        /// <returns>A period</returns>
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

        /// <summary>
        /// Converts a direction to a string representation of a direction
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <returns>String representation of a direction</returns>
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
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts a string representation of a direction to a direction
        /// </summary>
        /// <param name="direction">String representation of a direction</param>
        /// <returns>A direction</returns>
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

        /// <summary>
        /// Converts a sort by to a string representation of a sort by
        /// </summary>
        /// <param name="sortBy">A sort by</param>
        /// <returns>String representation of a sort by</returns>
        public static string SortByToString(SortBy sortBy)
        {
            if (sortBy == SortBy.CreatedAt)
            {
                return "created_at";
            }
            else if (sortBy == SortBy.LastBroadcast)
            {
                return "last_broadcast";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts a string representation of a sort by to a sort by
        /// </summary>
        /// <param name="sortBy">String representation of a sort by</param>
        /// <returns>A sort by</returns>
        public static SortBy StringToSortBy(string sortBy)
        {
            if (sortBy == "created_at")
            {
                return SortBy.CreatedAt;
            }
            else if (sortBy == "last_broadcast")
            {
                return SortBy.LastBroadcast;
            }
            else
            {
                return SortBy.None;
            }
        }

        /// <summary>
        /// Converts a commercial length to a number
        /// </summary>
        /// <param name="length">Commercial length</param>
        /// <returns>A number</returns>
        public static int LengthToInt(CommercialLength length)
        {
            if (length == CommercialLength.Sec30)
            {
                return 30;
            }
            else if (length == CommercialLength.Sec60)
            {
                return 60;
            }
            else if (length == CommercialLength.Sec90)
            {
                return 90;
            }
            else if (length == CommercialLength.Sec120)
            {
                return 120;
            }
            else if (length == CommercialLength.Sec150)
            {
                return 150;
            }
            else if (length == CommercialLength.Sec180)
            {
                return 180;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Converts a number to a commercial length
        /// </summary>
        /// <param name="length">A number</param>
        /// <returns>A commercial length</returns>
        public static CommercialLength IntToLength(int length)
        {
            if (length == 30)
            {
                return CommercialLength.Sec30;
            }
            else if (length == 60)
            {
                return CommercialLength.Sec60;
            }
            else if (length == 90)
            {
                return CommercialLength.Sec90;
            }
            else if (length == 120)
            {
                return CommercialLength.Sec120;
            }
            else if (length == 150)
            {
                return CommercialLength.Sec150;
            }
            else if (length == 180)
            {
                return CommercialLength.Sec180;
            }
            else
            {
                return CommercialLength.None;
            }
        }
    }
}
