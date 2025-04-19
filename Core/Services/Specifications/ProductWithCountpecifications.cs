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
                (!Specifications.BrandId.HasValue || P.BrandId == Specifications.BrandId)
                 &&
                (!Specifications.TypeId.HasValue || P.TypeId == Specifications.TypeId)


            )
        {
            
        }
    }
}
