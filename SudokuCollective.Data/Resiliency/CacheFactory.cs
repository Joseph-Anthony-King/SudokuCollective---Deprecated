using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Resiliency
{
    internal static class CacheFactory
    {
        internal static async Task<IRepositoryResponse> AddWithCacheAsync<T>(
            IRepository<T> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            T entity) where T : IEntityBase
        {
            var response = await repo.Add(entity);

            if (response.Success && response.Object.Id > 0)
            {
                var serializedItem = JsonConvert
                    .SerializeObject(response.Object);
                var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                await cache.SetAsync(
                    string.Format(cacheKey, response.Object.Id),
                    encodedItem,
                    options);

                List<string> cacheKeys;

                if (response.Object is User)
                {
                    cacheKeys = new List<string> {
                        string.Format(CacheKeys.GetAppUsersCacheKey, 1),
                        string.Format(CacheKeys.GetNonAppUsersCacheKey, 1),
                        CacheKeys.GetUsersCacheKey
                    };
                }
                else if (response.Object is App app)
                {
                    // Remove any app list cache items which may exist
                    cacheKeys = new List<string> {
                                string.Format(CacheKeys.GetMyAppsCacheKey, app.OwnerId)
                            };
                }
                else if (response.Object is Difficulty)
                {
                    // Remove any difficutly list cache items which may exist
                    cacheKeys = new List<string> {
                                CacheKeys.GetDifficulties
                            };
                }
                else
                {
                    // Remove any role list cache items which may exist
                    cacheKeys = new List<string> {
                                CacheKeys.GetRoles
                            };
                }

                if (cacheKeys != null)
                {
                    await RemoveKeysAsync(cache, cacheKeys);
                }
            }

            return response;
        }

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetWithCacheAsync<T>(
            IRepository<T> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int id,
            IBaseResult result = null) where T : IEntityBase
        {
            IRepositoryResponse response;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);

                response = new RepositoryResponse
                {
                    Success = true,
                    Object = JsonConvert
                    .DeserializeObject<T>(serializedItem)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.Get(id);

                if (response.Success && response.Object != null)
                {
                    var serializedItem = JsonConvert
                        .SerializeObject(response.Object);
                    var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItem,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetAllWithCacheAsync<T>(
            IRepository<T> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            IBaseResult result = null) where T : IEntityBase
        {
            IRepositoryResponse response;

            var cachedItems = await cache.GetAsync(cacheKey);

            if (cachedItems != null)
            {
                var serializedItems = Encoding.UTF8.GetString(cachedItems);

                response = new RepositoryResponse
                {
                    Success = true,
                    Objects = JsonConvert
                    .DeserializeObject<List<T>>(serializedItems)
                    .ConvertAll(x => (IEntityBase)x)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetAll();

                if (response.Success && response.Objects.Count > 0)
                {
                    var serializedItems = JsonConvert
                        .SerializeObject(response.Objects.ConvertAll(x => (T)x));
                    var encodedItems = Encoding.UTF8.GetBytes(serializedItems);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItems,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<IRepositoryResponse> UpdateWithCacheAsync<T>(
            IRepository<T> repo,
            IDistributedCache cache,
            T entity) where T : IEntityBase
        {
            var response = await repo.Update(entity);

            if (response.Success && response.Object.Id > 0)
            {
                List<string> cacheKeys;

                if (response.Object is User user)
                {
                    // Remove any user cache items which may exist
                    cacheKeys = new List<string> {
                                string.Format(CacheKeys.GetUserCacheKey, user.Id),
                                string.Format(CacheKeys.GetUserByUsernameCacheKey, user.UserName),
                                string.Format(CacheKeys.GetUserByEmailCacheKey, user.Email)
                            };
                }
                else if (response.Object is App app)
                {
                    // Remove any app cache items which may exist
                    cacheKeys = new List<string> {
                                string.Format(CacheKeys.GetAppCacheKey, app.Id),
                                string.Format(CacheKeys.GetAppByLicenseCacheKey, app.License),
                                string.Format(CacheKeys.GetMyAppsCacheKey, app.OwnerId)
                            };
                }
                else if (response.Object is Difficulty difficulty)
                {
                    // Remove any difficutly cache items which may exist
                    cacheKeys = new List<string> {
                                string.Format(CacheKeys.GetDifficulty, difficulty.Id),
                                CacheKeys.GetDifficulties
                            };
                }
                else
                {
                    var role = response.Object as Role;

                    // Remove any role cache items which may exist
                    cacheKeys = new List<string> {
                                string.Format(CacheKeys.GetRole, role.Id),
                                CacheKeys.GetRoles
                            };
                }

                await RemoveKeysAsync(cache, cacheKeys);
            }

            return response;
        }

        internal static async Task<IRepositoryResponse> DeleteWithCacheAsync<T>(
            IRepository<T> repo,
            IDistributedCache cache,
            T entity) where T : IEntityBase
        {
            /* If deleting a user this list will be need to get the associated
             * user apps in order to clear the cache */
            List<App> apps = null;

            if (entity is User)
            {
                /* Since we're deleting a user we get the associated apps
                 * to clear the cache */
                apps = new List<App>();

                var userRepo = (IUsersRepository<User>)repo;
                var appsResponse = await userRepo.GetMyApps(entity.Id);

                if (appsResponse.Success && appsResponse.Objects.Count > 0)
                {
                    apps = appsResponse
                        .Objects
                        .ConvertAll(a => (App)a)
                        .ToList();
                }

                // Finally, attach the license to each app...
                foreach (var app in apps)
                {
                    app.License = await userRepo.GetAppLicense(app.Id);
                }
            }

            var response = await repo.Delete(entity);

            if (response.Success)
            {
                List<string> cacheKeys;

                if (entity is User user)
                {
                    // Remove any user cache items which may exist
                    cacheKeys = new List<string> {
                            string.Format(CacheKeys.GetUserCacheKey, user.Id),
                            string.Format(CacheKeys.GetUserByUsernameCacheKey, user.UserName),
                            string.Format(CacheKeys.GetUserByEmailCacheKey, user.Email),
                            string.Format(CacheKeys.GetMyAppsCacheKey, user.Id),
                            string.Format(CacheKeys.GetMyRegisteredCacheKey, user.Id)
                        };

                    if (user.Apps.Count > 0 && apps != null)
                    {
                        foreach (var userApp in user.Apps)
                        {
                            var app = apps.Find(a => a.Id == userApp.AppId);

                            if (app != null)
                            {
                                cacheKeys.Add(string.Format(CacheKeys.GetAppCacheKey, userApp.AppId));
                                cacheKeys.Add(string.Format(CacheKeys.HasAppCacheKey, userApp.AppId));
                                cacheKeys.Add(string.Format(CacheKeys.GetAppLicenseCacheKey, userApp.AppId));
                                cacheKeys.Add(string.Format(CacheKeys.GetAppByLicenseCacheKey, app.License));
                                cacheKeys.Add(string.Format(CacheKeys.IsAppLicenseValidCacheKey, app.License));
                                cacheKeys.Add(string.Format(CacheKeys.GetAppUsersCacheKey, userApp.AppId));
                                cacheKeys.Add(string.Format(CacheKeys.GetNonAppUsersCacheKey, userApp.AppId));
                                cacheKeys.Add(CacheKeys.GetAppsCacheKey);
                            }
                        }
                    }
                }
                else if (entity is App app)
                {
                    // Remove any user cache items which may exist
                    cacheKeys = new List<string> {
                        string.Format(CacheKeys.GetAppCacheKey, app.Id),
                        string.Format(CacheKeys.HasAppCacheKey, app.Id),
                        string.Format(CacheKeys.GetAppLicenseCacheKey, app.Id),
                        string.Format(CacheKeys.GetAppByLicenseCacheKey, app.License),
                        string.Format(CacheKeys.IsAppLicenseValidCacheKey, app.License),
                        string.Format(CacheKeys.GetMyAppsCacheKey, app.OwnerId),
                        string.Format(CacheKeys.HasAppCacheKey, app.Id),
                        CacheKeys.GetAppsCacheKey                            
                    };
                }
                else if (entity is Difficulty difficulty)
                {
                    // Remove any difficulty cache items which may exist
                    cacheKeys = new List<string> {
                            string.Format(CacheKeys.GetDifficulty, difficulty.Id),
                            CacheKeys.GetDifficulties
                        };
                }
                else
                {
                    var role = entity as Role;

                    // Remove any role cache items which may exist
                    cacheKeys = new List<string> {
                            string.Format(CacheKeys.GetRole, role.Id),
                            CacheKeys.GetRoles
                        };
                }

                await RemoveKeysAsync(cache, cacheKeys);
            }

            return response;
        }

        internal static async Task<bool> HasEntityWithCacheAsync<T>(
            IRepository<T> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int id) where T : IEntityBase
        {
            bool result;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);
                result = JsonConvert.DeserializeObject<bool>(serializedItem);
            }
            else
            {
                var response = await repo.HasEntity(id);

                var serializedItem = JsonConvert.SerializeObject(response);
                var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                await cache.SetAsync(
                    cacheKey,
                    encodedItem,
                    options);

                result = response;
            }

            return result;
        }

        internal static async Task RemoveKeysAsync(
            IDistributedCache cache,
            List<string> keys)
        {
            foreach (var key in keys)
            {
                if (await cache.GetAsync(key) != null)
                {
                    await cache.RemoveAsync(string.Format(key));
                }
            }
        }

        #region App Repository Cache Methods
        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetAppByLicenseWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            string license,
            IBaseResult result = null)
        {
            IRepositoryResponse response;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);

                response = new RepositoryResponse
                {
                    Success = true,
                    Object = JsonConvert
                    .DeserializeObject<App>(serializedItem)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetByLicense(license);

                if (response.Success && response.Object != null)
                {
                    var serializedItem = JsonConvert
                        .SerializeObject(response.Object);
                    var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItem,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetAppUsersWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int id,
            IBaseResult result = null)
        {
            IRepositoryResponse response;

            var cachedItems = await cache.GetAsync(cacheKey);

            if (cachedItems != null)
            {
                var serializedItems = Encoding.UTF8.GetString(cachedItems);

                response = new RepositoryResponse
                {
                    Success = true,
                    Objects = JsonConvert
                    .DeserializeObject<List<User>>(serializedItems)
                    .ConvertAll(u => (IEntityBase)u)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetAppUsers(id);

                if (response.Success && response.Objects.Count > 0)
                {
                    var serializedItems = JsonConvert
                        .SerializeObject(response.Objects.ConvertAll(u => (IUser)u));
                    var encodedItems = Encoding.UTF8.GetBytes(serializedItems);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItems,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetNonAppUsersWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int id,
            IBaseResult result = null)
        {
            IRepositoryResponse response;

            var cachedItems = await cache.GetAsync(cacheKey);

            if (cachedItems != null)
            {
                var serializedItems = Encoding.UTF8.GetString(cachedItems);

                response = new RepositoryResponse
                {
                    Success = true,
                    Objects = JsonConvert
                    .DeserializeObject<List<User>>(serializedItems)
                    .ConvertAll(s => (IEntityBase)s)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetNonAppUsers(id);

                if (response.Success && response.Objects.Count > 0)
                {
                    var serializedItems = JsonConvert
                        .SerializeObject(response.Objects.ConvertAll(u => (IUser)u));
                    var encodedItems = Encoding.UTF8.GetBytes(serializedItems);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItems,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetMyAppsWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int ownerId,
            IBaseResult result = null)
        {
            IRepositoryResponse response;

            var cachedItems = await cache.GetAsync(cacheKey);

            if (cachedItems != null)
            {
                var serializedItems = Encoding.UTF8.GetString(cachedItems);

                response = new RepositoryResponse
                {
                    Success = true,
                    Objects = JsonConvert
                    .DeserializeObject<List<App>>(serializedItems)
                    .ConvertAll(a => (IEntityBase)a)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetMyApps(ownerId);

                if (response.Success && response.Objects.Count > 0)
                {
                    var serializedItems = JsonConvert
                        .SerializeObject(response.Objects.ConvertAll(a => (IApp)a));
                    var encodedItems = Encoding.UTF8.GetBytes(serializedItems);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItems,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetMyRegisteredAppsWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int userId,
            IBaseResult result = null)
        {
            IRepositoryResponse response;

            var cachedItems = await cache.GetAsync(cacheKey);

            if (cachedItems != null)
            {
                var serializedItems = Encoding.UTF8.GetString(cachedItems);

                response = new RepositoryResponse
                {
                    Success = true,
                    Objects = JsonConvert
                    .DeserializeObject<List<App>>(serializedItems)
                    .ConvertAll(a => (IEntityBase)a)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetMyRegisteredApps(userId);

                if (response.Success && response.Objects.Count > 0)
                {
                    var serializedItems = JsonConvert
                        .SerializeObject(response.Objects.ConvertAll(a => (IApp)a));
                    var encodedItems = Encoding.UTF8.GetBytes(serializedItems);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItems,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<Tuple<string, IBaseResult>> GetLicenseWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int id,
            IBaseResult result = null)
        {
            string license;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);

                license = JsonConvert.DeserializeObject<string>(serializedItem);

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                license = await repo.GetLicense(id);

                if (!string.IsNullOrEmpty(license))
                {
                    var serializedItem = JsonConvert.SerializeObject(license);
                    var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItem,
                        options);
                }
            }

            return new Tuple<string, IBaseResult>(license, result);
        }

        internal static async Task<IRepositoryResponse> ResetWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            App app)
        {
            var response = await repo.Reset(app);

            if (response.Success)
            {
                List<string> cacheKeys;

                // Remove any user cache items which may exist
                cacheKeys = new List<string> {
                            string.Format(CacheKeys.GetAppCacheKey, app.Id),
                            string.Format(CacheKeys.GetAppByLicenseCacheKey, app.License)
                        };

                await RemoveKeysAsync(cache, cacheKeys);
            }

            return response;
        }

        internal static async Task<IRepositoryResponse> ActivatetWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            int id)
        {
            var app = (App)(await repo.Get(id)).Object;
            var response = await repo.Activate(app.Id);

            if (response.Success)
            {
                List<string> cacheKeys;

                // Remove any user cache items which may exist
                cacheKeys = new List<string> {
                            string.Format(CacheKeys.GetAppCacheKey, app.Id),
                            string.Format(CacheKeys.GetAppByLicenseCacheKey, app.License)
                        };

                await RemoveKeysAsync(cache, cacheKeys);
            }

            return response;
        }

        internal static async Task<IRepositoryResponse> DeactivatetWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            int id)
        {
            var app = (App)(await repo.Get(id)).Object;
            var response = await repo.Deactivate(app.Id);

            if (response.Success)
            {
                List<string> cacheKeys;

                // Remove any user cache items which may exist
                cacheKeys = new List<string> {
                            string.Format(CacheKeys.GetAppCacheKey, app.Id),
                            string.Format(CacheKeys.GetAppByLicenseCacheKey, app.License)
                        };

                await RemoveKeysAsync(cache, cacheKeys);
            }

            return response;
        }

        internal static async Task<bool> IsAppLicenseValidWithCacheAsync(
            IAppsRepository<App> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            string license)
        {
            bool result;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);
                result = JsonConvert.DeserializeObject<bool>(serializedItem);
            }
            else
            {
                var response = await repo.IsAppLicenseValid(license);

                var serializedItem = JsonConvert.SerializeObject(response);
                var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                await cache.SetAsync(
                    cacheKey,
                    encodedItem,
                    options);

                result = response;
            }

            return result;
        }
        #endregion

        #region User Repository Cache Methods
        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetByUserNameWithCacheAsync(
            IUsersRepository<User> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            string username,
            IBaseResult result = null)
        {
            IRepositoryResponse response;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);

                response = new RepositoryResponse
                {
                    Success = true,
                    Object = JsonConvert
                    .DeserializeObject<User>(serializedItem)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetByUserName(username);

                if (response.Success && response.Object != null)
                {
                    var serializedItem = JsonConvert
                        .SerializeObject(response.Object);
                    var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    // Add user cache key by username
                    await cache.SetAsync(
                        cacheKey,
                        encodedItem,
                        options);

                    // Add user cache key by id
                    await cache.SetAsync(
                        string.Format(CacheKeys.GetUserCacheKey, response.Object.Id),
                        encodedItem,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetByEmailWithCacheAsync(
            IUsersRepository<User> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            string email,
            IBaseResult result = null)
        {
            IRepositoryResponse response;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);

                response = new RepositoryResponse
                {
                    Success = true,
                    Object = JsonConvert
                    .DeserializeObject<User>(serializedItem)
                };

                if (result != null)
                {
                    result.FromCache = true;
                }
            }
            else
            {
                response = await repo.GetByEmail(email);

                if (response.Success && response.Object != null)
                {
                    var serializedItem = JsonConvert
                        .SerializeObject(response.Object);
                    var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration);

                    await cache.SetAsync(
                        cacheKey,
                        encodedItem,
                        options);
                }
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }

        internal static async Task<IRepositoryResponse> ConfirmEmailWithCacheAsync(
            IUsersRepository<User> repo,
            IDistributedCache cache,
            EmailConfirmation email)
        {
            var response = await repo.ConfirmEmail(email);

            if (response.Success)
            {
                var user = (User)response.Object;
                List<string> cacheKeys;

                // Remove any user cache items which may exist
                cacheKeys = new List<string> {
                        string.Format(CacheKeys.GetUserCacheKey, user.Id),
                        string.Format(CacheKeys.GetUserByUsernameCacheKey, user.UserName),
                        string.Format(CacheKeys.GetUserByEmailCacheKey, user.Email)
                    };

                await RemoveKeysAsync(cache, cacheKeys);
            }

            return response;
        }

        internal static async Task<IRepositoryResponse> UpdateEmailWithCacheAsync(
            IUsersRepository<User> repo,
            IDistributedCache cache,
            EmailConfirmation email)
        {
            var response = await repo.UpdateEmail(email);

            if (response.Success)
            {
                var user = (User)response.Object;
                List<string> cacheKeys;

                // Remove any user cache items which may exist
                cacheKeys = new List<string> {
                        string.Format(CacheKeys.GetUserCacheKey, user.Id),
                        string.Format(CacheKeys.GetUserByUsernameCacheKey, user.UserName),
                        string.Format(CacheKeys.GetUserByEmailCacheKey, user.Email)
                    };

                await RemoveKeysAsync(cache, cacheKeys);
            }

            return response;
        }

        internal static async Task<bool> IsUserRegisteredWithCacheAsync(
            IUsersRepository<User> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            int id)
        {
            bool result;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);
                result = JsonConvert.DeserializeObject<bool>(serializedItem);
            }
            else
            {
                var response = await repo.IsUserRegistered(id);

                var serializedItem = JsonConvert.SerializeObject(response);
                var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                await cache.SetAsync(
                    cacheKey,
                    encodedItem,
                    options);

                result = response;
            }

            return result;
        }
        #endregion

        #region Difficulty Repository Cache Methods
        internal static async Task<bool> HasDifficultyLevelWithCacheAsync(
            IDifficultiesRepository<Difficulty> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            DifficultyLevel difficultyLevel)
        {
            bool result;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);
                result = JsonConvert.DeserializeObject<bool>(serializedItem);
            }
            else
            {
                var response = await repo.HasDifficultyLevel(difficultyLevel);

                var serializedItem = JsonConvert.SerializeObject(response);
                var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                await cache.SetAsync(
                    cacheKey,
                    encodedItem,
                    options);

                result = response;
            }

            return result;
        }
        #endregion

        #region Roles Repository Cache Methods
        internal static async Task<bool> HasRoleLevelWithCacheAsync(
            IRolesRepository<Role> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            RoleLevel roleLevel)
        {
            bool result;

            var cachedItem = await cache.GetAsync(cacheKey);

            if (cachedItem != null)
            {
                var serializedItem = Encoding.UTF8.GetString(cachedItem);
                result = JsonConvert.DeserializeObject<bool>(serializedItem);
            }
            else
            {
                var response = await repo.HasRoleLevel(roleLevel);

                var serializedItem = JsonConvert.SerializeObject(response);
                var encodedItem = Encoding.UTF8.GetBytes(serializedItem);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                await cache.SetAsync(
                    cacheKey,
                    encodedItem,
                    options);

                result = response;
            }

            return result;
        }
        #endregion
    }
}
