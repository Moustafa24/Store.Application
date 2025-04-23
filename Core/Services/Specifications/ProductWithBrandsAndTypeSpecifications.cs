using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypeSpecifications : BaseSpecification<Product, int>
    {
        public ProductWithBrandsAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyInclude();
        }

        public ProductWithBrandsAndTypeSpecifications(ProductSpecificationsParamters Specifications) : base
            (

            P =>
            (string.IsNullOrEmpty(Specifications.Search) || P.Name.ToLower().Contains(Specifications.Search.ToLower())) &&
            (!Specifications.BrandId.HasValue || P.BrandId == Specifications.BrandId)
            && (!Specifications.TypeId.HasValue || P.TypeId == Specifications.TypeId)
            )
        {
            ApplyInclude();
            ApplySorting(Specifications.Sort);
            ApplyPagination(Specifications.PageIndex, Specifications.PageSize);
        }


        private void ApplyInclude()
        {

            AddInclude(P => P.ProductBrand);

            AddInclude(P => P.ProductType);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;


                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

        }

        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            IsPagination = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

    }


}
