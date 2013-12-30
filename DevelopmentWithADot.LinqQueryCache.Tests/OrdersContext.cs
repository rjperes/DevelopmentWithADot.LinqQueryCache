using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DevelopmentWithADot.LinqQueryCache.Tests
{
	public class OrdersContext : DbContext
	{
		static OrdersContext()
		{
			Database.SetInitializer<OrdersContext>(null);
		}

		public OrdersContext() : base("Name=Orders")
		{

		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Product> Products
		{
			get;
			set;
		}

		public DbSet<Order> Orders
		{
			get;
			set;
		}

		public DbSet<Customer> Customers
		{
			get;
			set;
		}
	}
}
