using System;
using System.Linq;
using System.Linq.Expressions;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    /// <summary>
    /// ExpressionHelper class.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() where T : class { return f => true; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() where T : class { return f => false; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> DoAnd<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2) where T : class
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> DoLessThan<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2) where T : class
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.LessThan(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> DoGreaterThan<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2) where T : class
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.GreaterThan(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> DoNotEqual<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2) where T : class
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.NotEqual(expr1.Body, invokedExpr), expr1.Parameters);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> DoEqual<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2) where T : class
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Equal(expr1.Body, invokedExpr), expr1.Parameters);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> DoStartWith<T>(this Expression<Func<T, string>> expression, 
            string filter, Type type) where T : class
        {
            var vNotNullExpresion = Expression.NotEqual(
                                    expression.Body,
                                    Expression.Constant(null));

            var vMethodExpresion = Expression.Call(
                    expression.Body,
                    type.ToString(),
                    null,
                    Expression.Constant(filter));

            var vFilterExpresion = Expression.AndAlso(vNotNullExpresion, vMethodExpresion);

            return Expression.Lambda<Func<T, bool>>(
                vFilterExpresion,
                expression.Parameters);
        }
    }
}
