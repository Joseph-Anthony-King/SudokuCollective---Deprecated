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
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Resiliency
{
    internal static class CacheFactory
    {
        internal static async Task<Tuple<IRepositoryResponse, IBaseResult>> GetAllWithCacheAsync<T>(
            IRepository<T> repo,
            IDistributedCache cache,
            IRepositoryResponse response,
            IBaseResult result,
            string cacheKey,
            DateTime expiration) where T : IEntityBase
        {
            var cachedItems = await cache.GetAsync(cacheKey);

            if (cachedItems != null)
            {
                var serializedItems = Encoding.UTF8.GetString(cachedItems);

                response = new RepositoryResponse
                {
                    Success = true,
                    Objects = JsonConvert
                    .DeserializeObject<List<T>>(serializedItems)
                    .ConvertAll(s => (IEntityBase)s)
                };

                result.FromCache = true;
            }
            else
            {
                response = await repo.GetAll();

                var serializedItems = JsonConvert
                    .SerializeObject(response.Objects.ConvertAll(s => (T)s));
                var encodedItems = Encoding.UTF8.GetBytes(serializedItems);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                await cache.SetAsync(
                    cacheKey,
                    encodedItems,
                    options);
            }

            return new Tuple<IRepositoryResponse, IBaseResult>(response, result);
        }
    }
}
