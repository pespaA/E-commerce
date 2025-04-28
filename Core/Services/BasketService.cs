using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Basket;
using Domain.Exceptions;
using Services.Abstractions;
using Shared;

namespace Services
{
    public class BasketService(IBasketRepository basketRepository,IMapper Mapper)
        : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
        => await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDto?> GetBasketAsync(string id)
        {
            var basket =await basketRepository.GetBasketAsync(id);
            return basket is null? throw new BasketNotFoundException(id):
                Mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto)
        {
            var customerBasket = Mapper.Map<CustomerBasket>(basketDto);
            var updateBasket = await basketRepository.UpdateBasketAsync(customerBasket);
            return updateBasket is null? throw new Exception("We Can Not Update Basket"):
                Mapper.Map<BasketDto>(updateBasket);
        }
    }
}
