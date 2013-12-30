using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DevelopmentWithADot.LinqQueryCache.Tests
{
	public class Order
	{
		public DateTime Date
		{
			get;
			set;
		}

		[Key]
		[Column("ORDER_ID")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int32 OrderId
		{
			get;
			private set;
		}

		public virtual ICollection<OrderDetail> Details
		{
			get;
			set;
		}

		[Required]
		public virtual Customer Customer
		{
			get;
			set;
		}

		public Decimal TotalCost
		{
			get
			{
				return (this.Details.Select(d => d.Product.Price * d.Count).Sum());
			}
		}
	}
}
