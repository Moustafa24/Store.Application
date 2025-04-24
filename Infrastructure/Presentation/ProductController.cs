using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services.Abstractions;
using Shared;

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
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(PginationResponse<ProductResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError , Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest , Type = typeof(ErrorDetails))]
        [Cache(100)]
        public async Task<ActionResult<PginationResponse<ProductResultDto>>> GetAllProduct([FromQuery]ProductSpecificationsParamters Specifications)
        {
            var result =await serviceManager.ProductService.GetAllProductsAsync(Specifications);
            if (result is null) return BadRequest();
            return Ok(result); //200

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [Cache(100)]
        public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) return NotFound(); // 404  
            return Ok(result);
        }

        // Get All Brands :  Get All Brands 
        [HttpGet ("brands")]  //Get : api/products/Brands
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Cache(100)]
        public async Task<ActionResult<BrandResultDto>> GetAllBrands()
         {
            var result  = await serviceManager.ProductService.GetAllBrandsAsync();

            if (result is null) return BadRequest(); 

            return Ok(result);
         }


        //Get All Types
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PginationResponse<TypeResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [HttpGet("types")]  //Get : api/products/types
        [Cache(100)]
        public async Task<ActionResult<TypeResultDto>> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();

            if (result is null) return BadRequest();

            return Ok(result);
        }



    }
}
