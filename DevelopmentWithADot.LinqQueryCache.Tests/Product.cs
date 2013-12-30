using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevelopmentWithADot.LinqQueryCache.Tests
{
	public class Product
	{
		[Required]
		[StringLength(50)]
		public String Name
		{
			get;
			set;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int32 ProductId
		{
			get;
			private set;
		}

		public Decimal Price
		{
			get;
			set;
		}

		public virtual ICollection<OrderDetail> Details
		{
			get;
			set;
		}
	}
}
