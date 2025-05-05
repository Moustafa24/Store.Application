using AutoMapper;
using Domain.Contracts;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared;

namespace Services
{
    public class ServicesManager(IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBasketRepository basketRepository,
        ICacheRepository cacheRepository ,
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> options
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);

        public IBasketSrvice BasketSrvice { get; } = new BasketService(basketRepository, mapper);

        public ICacheService CacheService { get; } = new ChacheService(cacheRepository);

        public IAuthService AuthService { get; } = new AuthService(userManager, options ,mapper);

        public IOrderService OrderService { get; } = new OrederService(mapper,basketRepository,unitOfWork); 
    }
}
