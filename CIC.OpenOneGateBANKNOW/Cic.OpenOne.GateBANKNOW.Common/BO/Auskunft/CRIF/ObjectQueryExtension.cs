namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CrifHelper
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq.Expressions;

    public static class ObjectQueryExtension
    {
        /// <summary>
        /// Strongly typed Include Method for Entity Framework 4.0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mainQuery"></param>
        /// <param name="subSelector"></param>
        /// <returns></returns>
        public static ObjectQuery<T> Include<T>(this ObjectQuery<T> mainQuery, Expression<Func<T, object>> subSelector)
        {
            return mainQuery.Include(((subSelector.Body as MemberExpression).Member as System.Reflection.PropertyInfo).Name);
        }
    }
}