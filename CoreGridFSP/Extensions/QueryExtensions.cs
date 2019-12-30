using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreGridFSP.Extensions
{
    public static class QueryExtensions
    {
        /// <summary>
        /// Allows string column names to be used to sort a data set.
        /// </summary>
        /// <typeparam name="TEntity">The type of the data model to sort</typeparam>
        /// <param name="source">An IQueryable to sort</param>
        /// <param name="orderByProperty">The name of the property to sort on</param>
        /// <param name="desc">True if sort descending</param>
        /// <returns>A sorted IQueryable</returns>
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            if (string.IsNullOrWhiteSpace(orderByProperty))
                return source;

            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                return source;
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
