using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;


namespace Services.Abstractions
{
    public interface IProductService
    {
        // Get All Product  
        //Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? brandId, int? typeId ,string? sort , int pageIndex ,int  PageSize);
        Task<PginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParamters Specifications);

        // Get Product By Id  
        Task<ProductResultDto?> GetProductByIdAsync(int id);

        // Get All Brands  
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        // Get All Type  
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
    }
}
