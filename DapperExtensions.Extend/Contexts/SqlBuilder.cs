﻿using System.Collections.Generic;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using System;
using System.Linq;

namespace DapperExtensions.Extend
{
    internal class SqlBuilder : SqlGeneratorImpl
    {
        /// <summary>
        /// DapperExtensions配置
        /// </summary>
        IDapperExtensionsConfiguration _configuration;

        public SqlBuilder(IDapperExtensionsConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 生成update语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">where条件</param>
        /// <param name="where">where字段字典</param>
        /// <param name="set">set字段</param>
        /// <returns></returns>
        public string Update<T>(IPredicate predicate, IDictionary<string, object> where, IEnumerable<string> set) where T : class
        {
            IClassMapper classMap = _configuration.GetMap<T>();
            if (predicate==null || where == null || set == null)
            {
                throw new ArgumentNullException("predicate Or where Or set is null");
            }
            if (set.Count() == 0)
            {
                throw new ArgumentException("No columns.");
            }
            IEnumerable<string> setSql = set.Select(p => string.Format(
                        "{0} = {1}{2}", GetColumnName(classMap, p, false), Configuration.Dialect.ParameterPrefix, p));
            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                GetTableName(classMap),
                setSql.AppendStrings(),
                predicate.GetSql(this, where));
        }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="resultsPerPage"></param>
        /// <returns></returns>
        public string GetPagingWithMySql(Type entityType, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage)
        {
            var classMap = _configuration.GetMap(entityType);
            var parameters = new Dictionary<string, object>();
            var generator = SqlFactory.GetSqlGenerator(_configuration);
            return generator.SelectPaged(classMap, predicate, sort, page, resultsPerPage, parameters);
        }
    }
}