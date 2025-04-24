using AutoMapper;
using Domain.Contracts;
using Services.Abstractions;

namespace Services
{
    public class ServicesManager(IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBasketRepository basketRepository,
        ICacheRepository cacheRepository 
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);

        public IBasketSrvice BasketSrvice { get; } = new BasketService(basketRepository, mapper);

        public ICacheService CacheService { get; } = new ChacheService(cacheRepository);
    }
}
