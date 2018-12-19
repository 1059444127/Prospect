using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Windows;

namespace com.cloud.prospect
{
    public class DALModel<T> : DALBase
        where T : DALModel<T>, new()
    {
        public readonly string TableName;

        protected DALModel(string _tableName)
        {
            TableName = _tableName;
        }

        protected bool CommonSelectSingleRow(T obj, string condtion, string[] colunms)
        {
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                SqlParameter[] cmdParams = new SqlParameter[3];
                cmdParams[0] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
                cmdParams[0].Value = obj.TableName;
                cmdParams[1] = new SqlParameter("@Condition", SqlDbType.NVarChar, 500);
                cmdParams[1].Value = condtion;
                cmdParams[2] = new SqlParameter("@Colnums", SqlDbType.NVarChar, 500);
                if (colunms != null && colunms.Count() > 0)
                {
                    StringBuilder colunmsStr = new StringBuilder();
                    foreach (string colunm in colunms)
                    {
                        colunmsStr.Append(colunm);
                        colunmsStr.Append(",");
                    }
                    colunmsStr = colunmsStr.Remove(colunmsStr.Length - 1, 1);
                }
                else
                {
                    cmdParams[2].Value = DBNull.Value;
                }
                string cmd = "USP_Common_Select";
                DataSet ds = database.ExecuteDataSet(cmd, CommandType.StoredProcedure, cmdParams);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    for (int i = 0; i < properties.Count(); i++)
                    {
                        if (ds.Tables[0].Columns.Contains(properties[i].Name))
                        {
                            object result = DatabaseUtil.GetDotNetTypeValue(properties[i].PropertyType,
                                ds.Tables[0].Rows[0][properties[i].Name]);
                            properties[i].SetValue(obj, result, null);
                        }
                    }
                else
                    return false;
                
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e,false);
            }
            return true;
        }

        protected List<T> CommonSelect(T obj, string condtion, string[] colunms)
        {
            List<T> lstObj = null;
            try
            {
                lstObj = new List<T>();
                PropertyInfo[] properties = typeof(T).GetProperties();
                SqlParameter[] cmdParams = new SqlParameter[3];
                cmdParams[0] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
                cmdParams[0].Value = obj.TableName;
                cmdParams[1] = new SqlParameter("@Condition", SqlDbType.NVarChar, 500);
                cmdParams[1].Value = condtion;
                cmdParams[2] = new SqlParameter("@Colnums", SqlDbType.NVarChar, 500);
                if (colunms != null && colunms.Count() > 0)
                {
                    StringBuilder colunmsStr = new StringBuilder();
                    foreach (string colunm in colunms)
                    {
                        colunmsStr.Append(colunm);
                        colunmsStr.Append(",");
                    }
                    colunmsStr = colunmsStr.Remove(colunmsStr.Length - 1, 1);
                }
                else
                {
                    cmdParams[2].Value = DBNull.Value;
                }
                string cmd = "USP_Common_Select";
                DataSet ds = database.ExecuteDataSet(cmd, CommandType.StoredProcedure, cmdParams);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        ConstructorInfo constructor = typeof(T).GetConstructor(System.Type.EmptyTypes);
                        T tmpObj = (T)constructor.Invoke(null);
                        for (int i = 0; i < properties.Count(); i++)
                        {
                            if (ds.Tables[0].Columns.Contains(properties[i].Name))
                            {
                                object result = DatabaseUtil.GetDotNetTypeValue(properties[i].PropertyType,
                                    ds.Tables[0].Rows[j][properties[i].Name]);
                                properties[i].SetValue(tmpObj, result, null);
                            }
                        }
                        lstObj.Add(tmpObj);
                    }
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e,false);
            }
            return lstObj;
        }

        protected void CommonUpdate(T obj, string condition, string[] colunms)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update ");
                sb.Append(obj.TableName);
                sb.Append(" with(rowlock) \r\n");
                sb.Append("set ");
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Count(); i++)
                {
                    object value = properties[i].GetValue(obj, null);
                    if (default(T) != value && (colunms == null || colunms.Contains(properties[i].Name)))
                    {
                        sb.Append(properties[i].Name);
                        sb.Append(" = ");
                        sb.Append(DatabaseUtil.GetSqlTypeValue(properties[i].PropertyType, value));
                        sb.Append(",");
                    }
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("\r\n");
                sb.Append("where ");
                sb.Append(condition);
                database.ExecuteNonQuery(sb.ToString(), CommandType.Text);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e,false);
            }
        }

        protected void CommonInsert(T obj, string[] colunms)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                sb.Append("Insert into ");
                sb.Append(obj.TableName);
                sb.Append("(");
                sb2.Append("Select ");
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Count(); i++)
                {
                    object value = properties[i].GetValue(obj, null);
                    if (default(T) != value && (colunms == null || colunms.Contains(properties[i].Name)))
                    {
                        sb.Append(properties[i].Name);
                        sb.Append(",");
                        sb2.Append(DatabaseUtil.GetSqlTypeValue(properties[i].PropertyType, value));
                        sb2.Append(",");
                    }
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb2 = sb2.Remove(sb2.Length - 1, 1);
                sb.Append(")\r\n");
                sb.Append(sb2.ToString());
                database.ExecuteNonQuery(sb.ToString(), CommandType.Text);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e,false);
            }
        }

        protected void CommonDelete(T obj, string condition)
        {
            try
            {
                string sql = "DELETE FROM " + obj.TableName + " WHERE " + condition;
                database.ExecuteNonQuery(sql, CommandType.Text);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e,false);
            }
        }

    }
}
