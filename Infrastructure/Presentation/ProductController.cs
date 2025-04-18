using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IServiceManager serviceManager) : ControllerBase
    {

        // sort : nameasc [Default]
        // sort : namedesc
        // sort : PriceDec
        // sort : PriceAsc


        [HttpGet]
        public async Task<IActionResult> GetAllProduct(int? brandId , int? typeId , string? sort , int pageIndex= 1 ,int pageSize = 5)
        {
            var result =await serviceManager.ProductService.GetAllProductsAsync(brandId,typeId , sort , pageIndex , pageSize);
            if (result is null) return BadRequest();
            return Ok(result); //200

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) return NotFound(); // 404  
            return Ok(result);
        }

        // Get All Brands :  Get All Brands 
        [HttpGet ("brands")]  //Get : api/products/Brands
         public async Task<IActionResult> GetAllBrands()
         {
            var result  = await serviceManager.ProductService.GetAllBrandsAsync();

            if (result is null) return BadRequest(); 

            return Ok(result);
         }


        //Get All Types

        [HttpGet("types")]  //Get : api/products/types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();

            if (result is null) return BadRequest();

            return Ok(result);
        }



    }
}
