using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevelopmentWithADot.LinqQueryCache.Tests
{
	public class OrderDetail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int32 OrderDetailId
		{
			get;
			private set;
		}

		public Int32 Count
		{
			get;
			set;
		}

		[Required]
		public Order Order
		{
			get;
			set;
		}

		[Required]
		public Product Product
		{
			get;
			set;
		}
	}
}
