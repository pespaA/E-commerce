using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstractions
{
    public interface IBasketService
    {
        public Task<BasketDto?> GetBasketAsync(string id);
        public Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto);
        public Task<bool> DeleteBasketAsync(string id);
    }
}
