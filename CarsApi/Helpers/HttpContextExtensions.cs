using Microsoft.EntityFrameworkCore;

namespace CarsApi.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametersPagination<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int entitiesPerPage)
        {
            double entitiesCount = await queryable.CountAsync();
            double entitiesPages = Math.Ceiling(entitiesCount/entitiesPerPage);
            httpContext.Response.Headers.Add("entitiesPerPage", entitiesPages.ToString());
        }
    }
}
