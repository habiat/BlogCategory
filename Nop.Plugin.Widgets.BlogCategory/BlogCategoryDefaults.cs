using Nop.Core.Caching;

namespace Nop.Plugin.Widgets.BlogCategory
{
    public class BlogCategoryDefaults
    {
        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Widgets.BlogCategory";


        #region api-settings

        public const int DefaultAccessTokenExpirationInDays = 365; // 1 year
        public const string ApiRoleSystemName = "Administrators";
        public static double AllowedClockSkewInMinutes { get; set; } = 5;
        public static string SecurityKey { get; set; } = "NowIsTheTimeForAllGoodMenToComeToTheAideOfTheirCountry";

        #endregion
    }
}
