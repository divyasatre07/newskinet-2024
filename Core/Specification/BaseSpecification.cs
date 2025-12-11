using Core.interfaces;
using System;
using System.Linq.Expressions;

namespace Core.Specification
{
	public class BaseSpecification<T> : ISpecification<T>
	{
		protected BaseSpecification() : this(null) { }

		public BaseSpecification(Expression<Func<T, bool>>? criteria)
		{
			Criteria = criteria;
		}

		public Expression<Func<T, bool>>? Criteria { get; }
		public Expression<Func<T, object>>? OrderBy { get; private set; }
		public Expression<Func<T, object>>? OrderByDescending { get; private set; }
		public bool IsDistinct { get; private set; }

		protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
			=> OrderBy = orderByExpression;

		protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
			=> OrderByDescending = orderByDescExpression;

		protected void ApplyDistinct() => IsDistinct = true;
	}

	// SELECT version
	public class BaseSpecification<T, TResult>
	: BaseSpecification<T>, ISpecification<T, TResult>
	{
		protected BaseSpecification() : this(null!) { }

		public BaseSpecification(Expression<Func<T, bool>>? criteria)
			: base(criteria)
		{
		}

		public Expression<Func<T, TResult>>? Select { get; private set; }

		protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
			=> Select = selectExpression;
	}
}
