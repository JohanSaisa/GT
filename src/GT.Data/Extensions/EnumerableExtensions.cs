using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GT.Data.Extensions
{
	public static class EnumerableExtensions
	{
		public static bool FilteredAny<TSource>(
			this IEnumerable<TSource> source, 
			Expression<Func<TSource, bool>> predicateExpression, 
			out IEnumerable<TSource> result)
		{
			if(source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if(predicateExpression is null)
			{
				throw new ArgumentNullException(nameof(predicateExpression));
			}

			var predicate = predicateExpression.Compile();

			result = source.Where(predicate);

			return result.Any();
		}
	}
}
