using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<PginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParamters Specifications)
        {
            var Spec = new ProductWithBrandsAndTypeSpecifications(Specifications);
            
           

            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(Spec);
            var specCount = new ProductWithBrandsAndTypeSpecifications(Specifications);
            var count  = await unitOfWork.GetRepository<Product, int >().CountAsync(specCount);  

            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);
            return new PginationResponse<ProductResultDto>(Specifications.PageIndex, Specifications.PageSize, count, result);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)  
        {
            var Spec = new ProductWithBrandsAndTypeSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(Spec);
            if (product is null) return null;

            var result = mapper.Map<ProductResultDto>(product);
            return result;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }
    }
}
