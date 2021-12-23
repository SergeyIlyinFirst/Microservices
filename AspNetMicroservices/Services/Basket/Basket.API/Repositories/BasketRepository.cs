using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly JsonSerializerOptions _serializerOptions = new ()
        {
            PropertyNameCaseInsensitive = true
        };

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            string basket = await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket, _serializerOptions);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket, _serializerOptions));

            return await GetBasketAsync(basket.UserName);
        }

        public async Task DeleteBasketAsync(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
