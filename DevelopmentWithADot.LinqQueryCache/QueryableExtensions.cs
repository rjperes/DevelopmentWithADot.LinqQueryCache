using System;
using System.Linq;
using System.Runtime.Caching;

namespace DevelopmentWithADot.LinqQueryCache
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> AsCacheable<T>(this IQueryable<T> queryable, TimeSpan duration)
		{
			return (AsCacheable(queryable, (Int32) duration.TotalSeconds));
		}

		public static IQueryable<T> AsCacheable<T>(this IQueryable<T> queryable, Int32 durationSeconds)
		{
			ObjectCache cache = null;

			if (ObjectCache.Host != null)
			{
				cache = ObjectCache.Host.GetService(typeof(ObjectCache)) as ObjectCache;
			}

			cache = cache ?? MemoryCache.Default;

			IQueryable<T> cachedQuery = new QueryableWrapper<T>(cache, queryable, durationSeconds);

			return (cachedQuery);
		}

		public static IOrderedQueryable<T> AsCacheable<T>(this IOrderedQueryable<T> queryable, TimeSpan duration)
		{
			return (AsCacheable(queryable as IQueryable<T>, duration) as IOrderedQueryable<T>);
		}

		public static IOrderedQueryable<T> AsCacheable<T>(this IOrderedQueryable<T> queryable, Int32 durationSeconds)
		{
			return (AsCacheable(queryable as IQueryable<T>, durationSeconds) as IOrderedQueryable<T>);
		}
	}
}
