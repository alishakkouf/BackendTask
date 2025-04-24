using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Shared.Enums;

namespace BackendTask.Shared.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderByName, SortingDirection direction)
        {
            if (direction == SortingDirection.Asc)
                return source.OrderBy(ToLambda<T>(orderByName));
            return source.OrderByDescending(ToLambda<T>(orderByName));
        }


        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector, SortingDirection direction)
        {
            if (direction == SortingDirection.Asc)
                return source.OrderBy(keySelector);
            return source.OrderByDescending(keySelector);
        }

        public static IQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> keySelector, SortingDirection direction)
        {
            if (direction == SortingDirection.Asc)
                return source.ThenBy(keySelector);
            return source.ThenByDescending(keySelector);
        }

        public static IOrderedQueryable<T> ThenByOne<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> keySelector, SortingDirection direction)
        {
            if (direction == SortingDirection.Asc)
                return source.ThenBy(keySelector);
            return source.ThenByDescending(keySelector);
        }

        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
