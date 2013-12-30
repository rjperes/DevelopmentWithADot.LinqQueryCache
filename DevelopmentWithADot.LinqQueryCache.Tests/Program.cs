using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;

namespace DevelopmentWithADot.LinqQueryCache.Tests
{
	class Program
	{
		static void Main(String[] args)
		{
			using (OrdersContext ctx = new OrdersContext())
			{
				ExpressionEqualityComparer comparer = new ExpressionEqualityComparer();
				var e1 = ctx.Customers.Where(x => x.Orders.Any()).OrderBy(x => x.Name);
				var e2 = ctx.Customers.Where(y => y.Orders.Any()).OrderBy(y => y.Name);
				var e = comparer.Equals(e1.Expression, e2.Expression);

				//miss
				var c1 = ctx.Customers.Where(x => x.Orders.Any()).OrderBy(x => x.Name).AsCacheable(TimeSpan.FromSeconds(10)).ToList();

				//hit
				var c2 = ctx.Customers.Where(o => o.Orders.Any()).OrderBy(x => x.Name).AsCacheable(TimeSpan.FromSeconds(10)).ToList();

				//hit
				var c3 = (from c in ctx.Customers where c.Orders.Any() orderby c.Name select c).AsCacheable(TimeSpan.FromSeconds(10)).ToList();

				Thread.Sleep(10000);

				//miss
				var c4 = ctx.Customers.Where(x => x.Orders.Any()).OrderBy(x => x.Name).AsCacheable(TimeSpan.FromSeconds(10)).ToList();
			}
		}
	}
}
