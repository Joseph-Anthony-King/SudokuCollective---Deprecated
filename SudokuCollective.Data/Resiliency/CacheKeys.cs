namespace SudokuCollective.Data.Resiliency
{
    internal static class CacheKeys
    {
        internal const string GetAppCacheKey = ":GetAppCacheKey:{0}";
        internal const string GetAppByLicenseCacheKey = ":GetAppByLicenseCacheKey:{0}";
        internal const string GetAppsCacheKey = ":GetAppsCacheKey";
        internal const string GetMyAppsCacheKey = ":GetMyAppsCacheKey:{0}";
        internal const string GetMyRegisteredCacheKey = ":GetMyRegisteredCacheKey:{0}";
        internal const string GetAppUsersCacheKey = ":GetAppUsersCacheKey:{0}";
        internal const string GetNonAppUsersCacheKey = ":GetNonAppUsersCacheKey:{0}";
        internal const string GetAppLicenseCacheKey = ":GetAppLicense:{0}";
        internal const string HasAppCacheKey = ":HasAppCacheKey:{0}";
        internal const string IsAppLicenseValidCacheKey = ":IsAppLicenseValidCacheKey:{0}";
        internal const string GetUserCacheKey = ":GetUserCacheKey:{0}";
        internal const string HasUserCacheKey = ":HasUserCacheKey:{0}";
        internal const string IsUserRegisteredCacheKey = ":IsUserRegistered:{0}";
        internal const string GetSolutionsCacheKey = ":GetSolutionsCacheKey";
    }
}
