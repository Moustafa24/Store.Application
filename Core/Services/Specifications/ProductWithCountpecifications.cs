using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithCountpecifications :BaseSpecification<Product, int>
    {
        public ProductWithCountpecifications(ProductSpecificationsParamters Specifications) :base
            (
                P=>
                (string.IsNullOrEmpty(Specifications.Search) || P.Name.ToLower().Contains(Specifications.Search.ToLower())) &&
                (!Specifications.BrandId.HasValue || P.BrandId == Specifications.BrandId)
                 &&
                (!Specifications.TypeId.HasValue || P.TypeId == Specifications.TypeId)


            )
        {
            
        }
    }
}
