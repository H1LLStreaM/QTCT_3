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
        /// ��DataRow����ת��ΪModel����
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
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="pagesize">ÿҳ��ʾ��¼��</param>
        /// <param name="pageindex">��ǰҳ�루��1��ʼ��ref������</param>
        /// <param name="pagecount">��ҳ����out������</param>
        /// <param name="criteria">��ѯ����</param>
        /// <returns></returns>
        public static T[] FindAllByPage(int pagesize, ref int pageindex, out int rowscount, out int pagecount, params NHibernate.Expression.ICriterion[] criteria)
        {
            doPage(pagesize, ref pageindex, out rowscount, out pagecount, criteria);

            return ActiveRecordBase<T>.SlicedFindAll((pageindex - 1) * pagesize, pagesize, criteria);
        }

        /// <summary>
        /// ��ȡ��ҳ���ݣ��������ܣ�
        /// </summary>
        /// <param name="pagesize">ÿҳ��ʾ��¼��</param>
        /// <param name="pageindex">��ǰҳ�루��1��ʼ��ref������</param>
        /// <param name="pagecount">��ҳ����out������</param>
        /// <param name="orders">�������</param>
        /// <param name="criteria">��ѯ����</param>
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
        /// �������
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static int Count(params NHibernate.Expression.ICriterion[] criteria)
        {
            return  ActiveRecordBase<T>.Count(criteria);
        }
    }
}
