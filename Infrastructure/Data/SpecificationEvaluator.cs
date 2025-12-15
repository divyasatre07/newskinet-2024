using Core.Entities;
using Core.interfaces;
using System;
using System.Linq;

namespace Infrastructure.Data
{
	internal class SpecificationEvaluator<T> where T : BaseEntity
	{
		public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
		{
			// WHERE
			if (spec.Criteria != null)
				query = query.Where(spec.Criteria);

			// ORDER BY
			//if (spec.OrderBy != null)
			//	query = query.OrderBy(spec.OrderBy);

			if (spec.OrderByDescending != null)
				query = query.OrderByDescending(spec.OrderByDescending);

			// DISTINCT
			if (spec.IsDistinct)
				query = query.Distinct();

			if(spec.IsPagingEnabled)
			{ 
				query=query.Skip(spec.Skip).Take(spec.Take);
			}
			return query;
		}

		public static IQueryable<TResult> GetQuery<TSpec, TResult>(
			IQueryable<T> query,
			ISpecification<T, TResult> spec)
		{
			// WHERE
			if (spec.Criteria != null)
				query = query.Where(spec.Criteria);

			// ORDER BY
			if (spec.OrderBy != null)
				query = query.OrderBy(spec.OrderBy);

			if (spec.OrderByDescending != null)
				query = query.OrderByDescending(spec.OrderByDescending);

			// PROJECTION (THIS IS THE MOST IMPORTANT PART)
			if (spec.Select == null)
				throw new InvalidOperationException(
					"Select expression must not be null when using ISpecification<T, TResult>."
				);

			var projectedQuery = query.Select(spec.Select);

			// DISTINCT should be applied after projection
			if (spec.IsDistinct)
				projectedQuery = projectedQuery.Distinct();
			if (spec.IsPagingEnabled)
			{
				projectedQuery = projectedQuery?.Skip(spec.Skip).Take(spec.Take);
			}

			return projectedQuery ?? query.Cast <TResult>();
		}
	}
}
