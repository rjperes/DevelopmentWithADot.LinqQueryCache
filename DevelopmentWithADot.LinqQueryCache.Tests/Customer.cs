using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevelopmentWithADot.LinqQueryCache.Tests
{
	public class Customer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int32 CustomerId
		{
			get;
			private set;
		}

		[Required]
		[StringLength(50)]
		public String Name
		{
			get;
			set;
		}

		public virtual Address Address
		{
			get;
			set;
		}

		public virtual ICollection<Order> Orders
		{
			get;
			set;
		}
	}
}
