namespace EShop.Domain.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<TDestination> Apply<TSource, TDestination>(this IQueryable<TSource> sources,
            Func<IQueryable<TSource>, IQueryable<TDestination>> builder)
        {
            return builder(sources);
        }
    }
}
