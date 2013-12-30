using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;

namespace DevelopmentWithADot.LinqQueryCache
{
	sealed class QueryableWrapper<T> : IOrderedQueryable<T>
	{
		private static readonly ExpressionEqualityComparer comparer = new ExpressionEqualityComparer();

		sealed class EnumeratorWrapper : IEnumerator<T>
		{
			private readonly LinkedList<T> list = new LinkedList<T>();
			private QueryableWrapper<T> queryable;
			private IEnumerator<T> enumerator;
			private Boolean stored = false;
			internal Boolean consumed;

			public EnumeratorWrapper(QueryableWrapper<T> queryable, IEnumerator<T> enumerator)
			{
				this.enumerator = enumerator;
				this.queryable = queryable;
			}

			internal IEnumerator<T> FromCache()
			{
				return (this.list.GetEnumerator());
			}

			#region IEnumerator<T> Members

			public T Current
			{
				get
				{
					T current = this.enumerator.Current;

					if (this.stored == false)
					{
						this.list.AddLast(current);
						this.stored = true;
					}

					return (current);
				}
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				this.stored = false;
				this.consumed = true;
				this.enumerator.Dispose();
			}

			#endregion

			#region IEnumerator Members

			Object IEnumerator.Current
			{
				get
				{
					return (this.Current);
				}
			}

			public Boolean MoveNext()
			{
				Boolean result = this.enumerator.MoveNext();

				if (result == true)
				{
					this.stored = false;
				}

				return (result);
			}

			public void Reset()
			{
				this.stored = false;
				this.list.Clear();
				this.enumerator.Reset();
			}

			#endregion
		}

		#region Private readonly fields
		private readonly IQueryable<T> queryable;
		private readonly ObjectCache cache;
		private readonly Int32 durationSeconds;
		#endregion

		#region Internal constructor
		internal QueryableWrapper(ObjectCache cache, IQueryable<T> queryable, Int32 durationSeconds)
		{
			this.cache = cache;
			this.queryable = queryable;
			this.durationSeconds = durationSeconds;
		}
		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			IEnumerator<T> enumerator = null;
			String key = this.GetKey(this.queryable).ToString();

			if (this.cache.Contains(key) == true)
			{
				//hit
				enumerator = this.cache[key] as EnumeratorWrapper;
				if ((enumerator as EnumeratorWrapper).consumed == true)
				{
					return ((enumerator as EnumeratorWrapper).FromCache());
				}
			}
			else
			{
				//miss
				enumerator = new EnumeratorWrapper(this, this.queryable.GetEnumerator());
				this.cache.Add(key, enumerator, DateTimeOffset.Now.AddSeconds(this.durationSeconds));
			}

			return (enumerator);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (this.GetEnumerator());
		}

		#endregion

		#region IQueryable Members

		public Type ElementType
		{
			get
			{
				return (this.queryable.ElementType);
			}
		}

		public Expression Expression
		{
			get
			{
				return (this.queryable.Expression);
			}
		}

		public IQueryProvider Provider
		{
			get
			{
				return (this.queryable.Provider);
			}
		}

		#endregion

		#region Private methods
		private Int32 GetKey(IQueryable queryable)
		{
			return (comparer.GetHashCode(queryable.Expression));
		}
		#endregion
	}
}
