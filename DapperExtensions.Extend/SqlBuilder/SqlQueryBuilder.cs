using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions.Extend
{
    public class SqlQueryBuilder
    {
        public string Join<T, TJoin>(Expression<Func<T, TJoin, bool>> conditions)
            where T : class
            where TJoin : class
        {
            //var c1 = _configuration.GetMap<T>();
            //var c2 = _configuration.GetMap<TJoin>();
            return OnSql(conditions);

        }

        public string Select<T>(Expression<Func<T, object>> select) where T : class
        {
            if (select.Body is NewExpression)
            {
                var body = select.Body as NewExpression;
                return string.Join(",", body.Members.Select(x => x.Name));
            }
            if (select.Body is MemberExpression)
            {
                var body = select.Body as MemberExpression;
                return body.Member.Name;
            }
            if (select.Body is UnaryExpression)
            {
                var body = select.Body as UnaryExpression;
                var member = body.Operand as MemberExpression;
                return member.Member.Name;
            }
            return string.Empty;
        }

        public string Where<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }


        private string OnSql<T, TJoin>(Expression<Func<T, TJoin, bool>> expr) where T : class
            where TJoin : class
        {
            if (expr is LambdaExpression)
            {
                IDictionary<string, string> dic = new Dictionary<string, string>();
                if (expr.Body is BinaryExpression)
                {
                    Visit(expr.Body as BinaryExpression, ref dic);
                }
                if (dic.Count > 0)
                {
                    return string.Join("", dic.Select(x => x.Key + "=" + x.Value));
                }
            }
            return null;
        }

        private void Visit(BinaryExpression expr, ref IDictionary<string, string> dic)
        {
            var left = expr.Left as MemberExpression;
            var right = expr.Right as MemberExpression;
            if (left != null && right != null)
            {
                if (!dic.ContainsKey(left.Member.Name))
                {
                    dic.Add(left.Member.Name, right.Member.Name);
                }
            }
            else
            {
                throw new NotImplementedException("没有实现多个条件");
                // Visit(expr.Left as BinaryExpression, ref dic);
                //  Visit(expr.Right as BinaryExpression, ref dic);
            }
        }



    }



}
