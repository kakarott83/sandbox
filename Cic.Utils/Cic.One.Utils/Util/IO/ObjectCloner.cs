using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Cic.One.Util.IO
{
    /// <summary>
    /// Deep-Cloning Objects
    /// </summary>
    public class ObjectCloner
    {
        /// <summary>
        /// Clones the given Object with its sub-Objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(T obj)
        {
            return ObjectExtCache<T>.Clone(obj);
        }

        static class ObjectExtCache<T>
        {
            private static readonly Func<T, T> cloner;
            static ObjectExtCache()
            {
                ParameterExpression param = Expression.Parameter(typeof(T), "in");

                var bindings = from prop in typeof(T).GetProperties()
                               where prop.CanRead && prop.CanWrite
                               //let column = Attribute.GetCustomAttribute(prop,typeof(ColumnAttribute)) as ColumnAttribute
                               // where column == null || !column.IsPrimaryKey
                               select (MemberBinding)Expression.Bind(prop,
                                   Expression.Property(param, prop));

                cloner = Expression.Lambda<Func<T, T>>(
                    Expression.MemberInit(
                        Expression.New(typeof(T)), bindings), param).Compile();
            }
            public static T Clone(T obj)
            {
                return cloner(obj);
            }
        }
    }
}