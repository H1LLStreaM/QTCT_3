using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using NHibernate;
using System.Reflection;

namespace WY.Library.Dao
{
    public abstract class BaseDao<T> : ActiveRecordBase<T>
    {
        /// <summary>
        /// 将DataRow数据转换为Model对象
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T DataRowToModel(DataRow row)
        {
            object o = Activator.CreateInstance(typeof(T));

            PropertyInfo[] props = typeof(T).GetProperties();
            
            foreach (PropertyInfo p in props)
            {
                string fName = p.Name.ToUpper();
                if (row.Table.Columns.Contains(fName))
                {
                    Object val = row[p.Name.ToUpper()];
                    if (val == DBNull.Value)
                    {
                        val = null;
                    }
                    if (p.PropertyType == typeof(float))
                    {
                        p.GetSetMethod().Invoke(o, new Object[] { Convert.ToSingle(val) });
                    }
                    else if (p.PropertyType == typeof(double))
                    {
                        p.GetSetMethod().Invoke(o, new Object[] { Convert.ToDouble(val) });
                    }
                    else
                    {
                        p.GetSetMethod().Invoke(o, new Object[] { val });
                    }
                }
            }
            
            return (T)o;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagesize">每页显示记录数</param>
        /// <param name="pageindex">当前页码（从1开始，ref变量）</param>
        /// <param name="pagecount">总页数（out变量）</param>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public static T[] FindAllByPage(int pagesize, ref int pageindex, out int rowscount, out int pagecount, params NHibernate.Expression.ICriterion[] criteria)
        {
            doPage(pagesize, ref pageindex, out rowscount, out pagecount, criteria);

            return ActiveRecordBase<T>.SlicedFindAll((pageindex - 1) * pagesize, pagesize, criteria);
        }

        /// <summary>
        /// 获取分页数据（带排序功能）
        /// </summary>
        /// <param name="pagesize">每页显示记录数</param>
        /// <param name="pageindex">当前页码（从1开始，ref变量）</param>
        /// <param name="pagecount">总页数（out变量）</param>
        /// <param name="orders">排序规则</param>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public static T[] FindAllByPage(int pagesize, ref int pageindex, out int rowscount, out int pagecount, NHibernate.Expression.Order[] orders, params NHibernate.Expression.ICriterion[] criteria)
        {
            doPage(pagesize, ref pageindex, out rowscount, out pagecount, criteria);

            return ActiveRecordBase<T>.SlicedFindAll((pageindex - 1) * pagesize, pagesize, orders, criteria);
        }

        private static void doPage(int pagesize, ref int pageindex, out int rowscount, out int pagecount, params NHibernate.Expression.ICriterion[] criteria)
        {
            rowscount = ActiveRecordBase<T>.Count(criteria);

            if (pageindex < 1) pageindex = 1;
            pagecount = 0;

            pagecount = rowscount / pagesize;
            if ((rowscount % pagesize) > 0)
            {
                pagecount += 1;
            }

            if (pageindex > pagecount)
            {
                pageindex = pagecount;
            }
        }

        /// <summary>
        /// 获得条数
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static int Count(params NHibernate.Expression.ICriterion[] criteria)
        {
            return  ActiveRecordBase<T>.Count(criteria);
        }
    }
}
