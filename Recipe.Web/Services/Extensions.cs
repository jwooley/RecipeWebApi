using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Recipe.Web.Services
{
    /// <summary>
    /// Extension methods to wrap an IQueryable value into a paged result.
    /// </summary>
    public static class PageResultWrapper
    {
        public static PageResult<T> ToPageResult<T>(this IQueryable<T> source, ODataQueryOptions<T> options, HttpRequestMessage request)
        {
            var settings = new ODataQuerySettings { PageSize = 10 };
            if (options.Top != null && options.Top.Value != 0)
            {
                settings.PageSize = options.Top.Value;
            }

            var items = options.ApplyTo(source, settings);
            PageResult<T> result = new PageResult<T>(items as IEnumerable<T>, request.ODataProperties().NextLink, request.ODataProperties().TotalCount);
            return result;
        }
    }
}