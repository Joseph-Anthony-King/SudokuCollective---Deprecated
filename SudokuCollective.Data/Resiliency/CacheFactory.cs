using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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
            IBaseResult result)
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

                    await cache.SetAsync(
                        cacheKey,
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

        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> ConfirmEmailWithCacheAsync(
            IUsersRepository<User> repo,
            IDistributedCache cache,
            string cacheKey,
            DateTime expiration,
            EmailConfirmation email,
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
                response = await repo.ConfirmEmail(email);

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
    }
}
