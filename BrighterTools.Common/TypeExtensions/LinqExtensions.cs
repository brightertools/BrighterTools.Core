using System.Linq.Expressions;
using System.Reflection;

namespace App.TypeExtensions;

// more paging and sorting generic extensions
// https://stackoverflow.com/questions/30872715/entity-generic-pagination

public static class LinqExtensions
{
    /// <summary>
    /// OrderBy property name as string
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="orderByProperty"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
    {
        if (string.IsNullOrWhiteSpace(orderByProperty))
        {
            throw new ArgumentException("Order by property name cannot be null or empty.", nameof(orderByProperty));
        }

        var type = typeof(TEntity);
        var property = type.GetProperty(orderByProperty, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (property == null)
        {
            // Fallback to Id property if the specified property is not found
            property = type.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property == null)
            {
                throw new ArgumentException($"No property '{orderByProperty}' found on type '{type.Name}' and 'Id' property not found for fallback.", nameof(orderByProperty));
            }
        }

        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var command = desc ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<TEntity>(resultExpression);
    }

    public static IEnumerable<T> GroupDistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
    {
        return items.GroupBy(property).Select(x => x.First());
    }


    //    public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
    //    {
    //        if (list == null)
    //        {
    //            return;
    //        }

    //        Contract.Requires(list != null);
    //        Contract.Requires(firstIndex >= 0 && firstIndex < list!.Count);
    //        Contract.Requires(secondIndex >= 0 && secondIndex < list!.Count);
    //        if (firstIndex == secondIndex)
    //        {
    //            return;
    //        }
    //        T temp = list![firstIndex];
    //        list[firstIndex] = list[secondIndex];
    //        list[secondIndex] = temp;
    //    }

    //    public static IEnumerable<T> GroupDistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
    //    {
    //        return items.GroupBy(property).Select(x => x.First());
    //    }


    //    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string fieldName) where T : class
    //    {
    //        MethodCallExpression resultExp = GenerateMethodCall<T>(source, "OrderByDescending", fieldName);
    //        return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
    //    }

    //    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string fieldName) where T : class
    //    {
    //        MethodCallExpression resultExp = GenerateMethodCall<T>(source, "ThenBy", fieldName);
    //        return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
    //    }

    //    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string fieldName) where T : class
    //    {
    //        MethodCallExpression resultExp = GenerateMethodCall<T>(source, "ThenByDescending", fieldName);
    //        return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
    //    }

    //    #region Private expression helpers

    //    private static LambdaExpression GenerateSelector<T>(String propertyName, out Type resultType) where T : class
    //    {
    //        // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
    //        var parameter = Expression.Parameter(typeof(T), "Entity");
    //        //  create the selector part, but support child properties
    //        PropertyInfo property;
    //        Expression propertyAccess;
    //        if (propertyName.Contains('.'))
    //        {
    //            // support to be sorted on child fields.
    //            String[] childProperties = propertyName.Split('.');
    //            property = typeof(T).GetProperty(childProperties[0], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
    //            propertyAccess = Expression.MakeMemberAccess(parameter, property);
    //            for (int i = 1; i < childProperties.Length; i++)
    //            {
    //                property = property.PropertyType.GetProperty(childProperties[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
    //                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
    //            }
    //        }
    //        else
    //        {
    //            property = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
    //            propertyAccess = Expression.MakeMemberAccess(parameter, property);
    //        }
    //        resultType = property.PropertyType;
    //        // Create the order by expression.
    //        return Expression.Lambda(propertyAccess, parameter);
    //    }

    //    private static MethodCallExpression GenerateMethodCall<T>(IQueryable<T> source, string methodName, String fieldName) where T : class
    //    {
    //        Type type = typeof(T);
    //        Type selectorResultType;
    //        LambdaExpression selector = GenerateSelector<T>(fieldName, out selectorResultType);
    //        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
    //            new Type[] {type, selectorResultType},
    //            source.Expression, Expression.Quote(selector));
    //        return resultExp;
    //    }

    //    #endregion
}