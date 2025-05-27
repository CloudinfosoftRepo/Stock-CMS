using Stock_CMS.Models;
using System.Linq.Expressions;

namespace Stock_CMS.Common
{
	public static class FilterHelper
	{
		public static Expression<Func<T, bool>> BuildFilterExpression<T>(List<FilterRule> filters)
		{
			if (filters == null || !filters.Any()) return null;

			var parameter = Expression.Parameter(typeof(T), "x");
			Expression combined = null;

			foreach (var filter in filters)
			{
				var property = typeof(T).GetProperty(filter.Field);
				if (property == null) continue;

				var left = Expression.Property(parameter, property);
				object convertedValue;

				try
				{
					convertedValue = Convert.ChangeType(filter.Value, property.PropertyType);
				}
				catch
				{
					continue;
				}

				var right = Expression.Constant(convertedValue);
				Expression expression = null;

				switch (filter.Operator?.ToLower())
				{
					case "eq": expression = Expression.Equal(left, right); break;
					case "neq": expression = Expression.NotEqual(left, right); break;
					case "gt": expression = Expression.GreaterThan(left, right); break;
					case "lt": expression = Expression.LessThan(left, right); break;
					case "contains":
						if (property.PropertyType != typeof(string)) continue;
						var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
						expression = Expression.Call(left, containsMethod, right);
						break;
					default: continue;
				}

				combined = combined == null ? expression : Expression.AndAlso(combined, expression);
			}

			return combined != null ? Expression.Lambda<Func<T, bool>>(combined, parameter) : null;
		}
	}


}
