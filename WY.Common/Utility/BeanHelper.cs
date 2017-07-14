using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using WY.Common.Framework;

namespace WY.Common.Utility
{
    public class BeanHelper
    {
        private static string[] excludeList = new string[]{"Id"
                                                            ,"Password"
                                                            , "Createuser"
                                                            , "Createtime"
                                                            , "Updateuser"
                                                            , "Updatetime"
                                                        };

        //public static Object GetValueIngoreCase(BaseModel dao, string propName)
        //{
        //    Type t = dao.GetType();

        //    PropertyInfo[] props = t.GetProperties();
        //    foreach (PropertyInfo p in props)
        //    {

        //        string fName = p.Name.ToUpper();
        //        if (fName.Equals(propName, StringComparison.OrdinalIgnoreCase))
        //        {
        //            Object val = p.GetGetMethod().Invoke(dao, new Object[] { });
        //            return val;
        //        }
        //    }
        //    throw new Exception("This Field does not exist");
        //}

        public static void DataRowToModel(DataRow row, object o)
        {
            Type t = o.GetType();

            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo p in props)
            {

                string fName = p.Name.ToUpper();
                Object val = row[p.Name.ToUpper()];
                if (val == DBNull.Value)
                {
                    val = null;
                }
                p.GetSetMethod().Invoke(o, new Object[] { val });
                //GetColumnValueType(o, row[p.Name.ToUpper()], p);
            }
        }

        public static void ModelToDataRow(object o, DataRow row)
        {
            Type t = o.GetType();

            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo p in props)
            {

                string fName = p.Name.ToUpper();
                Object val = p.GetGetMethod().Invoke(o, new Object[] { });

                if (val != null)
                {
                    row[p.Name.ToUpper()] = val;
                }
                else
                {
                    row[p.Name.ToUpper()] = DBNull.Value;
                }
                //GetColumnValueType(o, row[p.Name.ToUpper()], p);
            }
        }

        /// <summary>
        /// 复制对象（复制属性名相同的数据）
        /// </summary>
        /// <param name="objfrom"></param>
        /// <param name="objto"></param>
        public static void ObjectCopy(object objfrom, object objto)
        {
            //if (!objfrom.GetType().Equals(objto.GetType()))
            //{
            //    throw new Exception("类型不一致，导致Clone失败！");
            //}

            Type fromt = objfrom.GetType();
            Type tot = objto.GetType();

            PropertyInfo[] propsfrom = fromt.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | BindingFlags.DeclaredOnly);
            PropertyInfo[] propsto = tot.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo fromp in propsfrom)
            {
                string fName = fromp.Name.ToUpper();

                foreach (PropertyInfo top in propsto)
                {
                    if (fName.Equals(top.Name.ToUpper()))
                    {
                        if (top.CanWrite)
                        {
                            try
                            {
                                if (typeof(List<string>).Equals(fromp.PropertyType))
                                {
                                    List<string> newlist = new List<string>();
                                    foreach (string str in (List<string>)fromp.GetValue(objfrom, null))
                                    {
                                        newlist.Add(str);
                                    }
                                    top.SetValue(objto, newlist, null);
                                }
                                else
                                {
                                    top.SetValue(objto, fromp.GetValue(objfrom, null), null);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                //Log.Error(ex);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 复制对象（要求类型一致）
        /// </summary>
        /// <param name="objfrom"></param>
        /// <param name="objto"></param>
        public static void ObjectClone(object objfrom, object objto)
        {
            if (!objfrom.GetType().Equals(objto.GetType()))
            {
                throw new Exception("类型不一致，导致Clone失败！");
            }

            Type fromt = objfrom.GetType();
            Type tot = objto.GetType();

            PropertyInfo[] propsfrom = fromt.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo fromp in propsfrom)
            {
                string fName = fromp.Name.ToUpper();

                if (fromp.CanWrite)
                {
                    try
                    {
                        if (typeof(List<string>).Equals(fromp.PropertyType))
                        {
                            List<string> newlist = new List<string>();
                            foreach (string str in (List<string>)fromp.GetValue(objfrom, null))
                            {
                                newlist.Add(str);
                            }
                            fromp.SetValue(objto, newlist, null);
                        }
                        else
                        {
                            fromp.SetValue(objto, fromp.GetValue(objfrom, null), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //Log.Error(ex);
                    }
                }
            }
        }


        public static object getPropertyValueByName(Object o, string pName)
        {
            PropertyInfo pf = getPropertyByName(o, pName);
            return pf.GetValue(o, null);
        }
        public static PropertyInfo getPropertyByName(Object o, string pName)
        {
            Type oType = o.GetType();
            PropertyInfo[] propsFromt = oType.GetProperties();
            foreach (PropertyInfo fromp in propsFromt)
            {
                string fName = fromp.Name.ToUpper();
                if (fName.Equals(pName, StringComparison.OrdinalIgnoreCase))
                {
                    return fromp;
                }
            }
            return null;
        }
        public static string[] getPropertyNameList(object o)
        {
            List<string> list = new List<string>();
            Type oType = o.GetType();
            PropertyInfo[] propsFromt = oType.GetProperties();
            foreach (PropertyInfo fromp in propsFromt)
            {
                list.Add(fromp.Name);
            }

            return list.ToArray();
        }

        public static bool Equal(Object o1, Object o2)
        {
            if (o1 == null)
            {
                if (o2 == null)
                {
                    return true;
                }
                else
                {
                    if (typeof(string).IsInstanceOfType(o2))
                    {
                        if (string.IsNullOrEmpty((string)o2))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            else
            {
                if (o2 == null)
                {
                    if (typeof(string).IsInstanceOfType(o1))
                    {
                        if (string.IsNullOrEmpty((string)o1))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return o1.Equals(o2);
                }
            }
        }
        public static bool DataEqual(object obj1, object obj2)
        {
            if (obj1 == null)
            {
                return false;
            }
            if (!obj1.GetType().Equals(obj2.GetType()))
            {
                return false;
            }
            string[] keys = BeanHelper.getPropertyNameList(obj1);
            return DataEqual(obj1, obj2, keys, true);
        }
        public static bool DataEqual(object obj1, object obj2, string[] keys, params bool[] isExeclude)
        {
            return DataEqual(obj1, obj2, keys, keys, isExeclude);
        }
        public static bool DataEqual(object obj1, object obj2, string[] fromKeys, string[] toKeys, params bool[] isExeclude)
        {
            for (int i = 0; i < fromKeys.Length; i++)
            {
                if (isExeclude.Length > 0 && isExeclude[0])
                {
                    if (IsExclude(fromKeys[i]))
                    {
                        continue;
                    }
                }
                object currValue = BeanHelper.getPropertyValueByName(obj1, fromKeys[i]);
                object comValue = BeanHelper.getPropertyValueByName(obj2, toKeys[i]);
                if (!BeanHelper.Equal(currValue, comValue))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsExclude(string name)
        {
            foreach (string eName in excludeList)
            {
                if (eName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
