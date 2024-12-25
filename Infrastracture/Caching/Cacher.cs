using Core.ResultModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Infrastracture.Caching
{
    public class Cacher : ICacher
    {
        private readonly IDistributedCache cache;
        private readonly IOptions<CacheOptions> options;
        private readonly DistributedCacheEntryOptions defaultOptions;
        private readonly TimeSpan defaultExpare;
        public Cacher(IDistributedCache cache, IOptions<CacheOptions> options)
        {
            this.cache = cache;
            this.options = options;
            defaultExpare = TimeSpan.FromMinutes(options.Value.ExpiresMinutes);
            defaultOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = defaultExpare
            };
        }
        public async Task<ResultModel> SetCode(string key, int value)
        {
            try
            {
                await cache.SetStringAsync(key, value.ToString(), defaultOptions);
                return ResultModel.Ok();
            }
            catch (Exception ex)
            {
                return ResultModel.Error("Error during executing : " + ex);
            }           
        }
        public async Task<int?> GetCode(string key)
        {
            var cacheResult = await cache.GetStringAsync(key);
            if (cacheResult == null)
            {
                return null;
            }
            var result = int.Parse(cacheResult);
            return result;
        }
    }
}
