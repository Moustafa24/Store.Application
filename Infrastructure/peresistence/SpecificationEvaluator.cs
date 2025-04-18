using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace peresistence
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
            IQueryable<TEntity> inputQuery
            ,ISpecifications<TEntity, TKey> Spec
            )
            where TEntity : BaseEntity<TKey>

        {
           var query = inputQuery;

            if (Spec.Criteria is not null)
            {
                query = query.Where(Spec.Criteria);
            }

            if (Spec.OrderBy is not null)
            { 
                query = query.OrderBy(Spec.OrderBy);
            }else if (Spec.OrderByDescending is not null)
                { query = query.OrderByDescending(Spec.OrderByDescending); }


            if(Spec.IsPagination)
            {

                query= query.Skip(Spec.Skip).Take(Spec.Take);
            }

            query=  Spec.IncludeExpression.Aggregate(query, (currentQuary, includeExpression) => currentQuary.Include(includeExpression));
            

            
            return query;


        }
    }
}
