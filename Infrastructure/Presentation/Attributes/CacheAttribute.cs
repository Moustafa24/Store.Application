using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;


namespace Presentation.Attributes
{
    public class CacheAttribute(int durationInSecond) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

          var cacheService =   context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            var chacheKey = GenerateChacheKey(context.HttpContext.Request); 
           var result = await cacheService.GetCacheValueAsync(chacheKey);
           
            if (!string.IsNullOrEmpty(result))
            {
                context.Result = new ContentResult()
                {
                    ContentType= "application/json",
                    StatusCode= StatusCodes.Status200OK,
                    Content = result
                };
                return;
            }
            //Ececute The End Point 

            var contextResult =  await next.Invoke();
            if(contextResult.Result is OkObjectResult okObject)
            {
               await cacheService.SetCacheValueAsync(chacheKey, JsonSerializer.Serialize(okObject.Value), TimeSpan.FromSeconds(durationInSecond));
            }
        }

        private string GenerateChacheKey(HttpRequest request)
        {
            var Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}");
            }

            return Key.ToString();
        }
    }

}
