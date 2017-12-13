using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    /// <summary>
    /// PropertyHelper class.
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Expression<T> GetGroupKey<T, TSource>(this object obj, string property)
        {
            var parameter = Expression.Parameter(typeof(TSource));
            var body = Expression.Property(parameter, property);
            return Expression.Lambda<T>(body, parameter);
        }


        /// <summary>
        /// Fourni une expression utilisable dans la méthode Where de LINQ
        /// </summary>
        /// <typeparam name="T">Classe cible</typeparam>
        /// <param name="nomPropriete">Nom de la propriété de la classe T</param>
        /// <param name="valueToCompare">Valeur à comparer</param>
        /// <returns>Expression</returns>
        public static Expression<Func<T, bool>> GetFilterEquality<T>(string nomPropriete, string valueToCompare)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            MemberExpression member = Expression.Property(param, nomPropriete);
            ConstantExpression cst = Expression.Constant(valueToCompare);
            BinaryExpression body = Expression.Equal(member, cst);
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        public static Expression<Func<ValueObject, string>> GetSelectDimension(string nomDimension)
        {
            ParameterExpression param = Expression.Parameter(typeof(ValueObject), "x");
            MemberExpression member = Expression.Property(param, nomDimension);
            return Expression.Lambda<Func<ValueObject, string>>(member, param);
        }


        /// <summary>
        /// This function permits to add a value in an array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] Add(this string[] array, string value)
        {
            List<string> list = array.ToList();
            list.Add(value);
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<T>(this T obj, string property) where T : class
        {
            return obj.GetType().GetProperty(property);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(this T obj, string property) where T : class
        {
            return obj.GetProperty<T>(property).GetValue(obj);
        }
    }
}
