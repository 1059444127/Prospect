using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    public partial class Search : DALModel<Search>
    {
        public Search()
            : base("")
        {

        }

        public ObservableCollection<Search> Select(string condition, int maxnum)
        {
            ObservableCollection<Search> col = new ObservableCollection<Search>();
            try
            {
                SqlParameter[] cmdParams = new SqlParameter[2];
                cmdParams[0] = new SqlParameter("@Condition", SqlDbType.NVarChar, 2000);
                cmdParams[0].Value = condition;
                cmdParams[1] = new SqlParameter("@MaxNumber", SqlDbType.Int);
                if (maxnum == 0)
                    cmdParams[1].Value = DBNull.Value;
                else
                    cmdParams[1].Value = maxnum;
                DataSet ds = database.ExecuteDataSet("USP_Common_Application_Select",
                    CommandType.StoredProcedure, cmdParams);
                if (ds.Tables.Count > 0)
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        Search app = new Search();
                        PropertyInfo[] properties = this.GetType().GetProperties();

                        for (int i = 0; i < properties.Count(); i++)
                        {
                            if (ds.Tables[0].Columns.Contains(properties[i].Name))
                            {
                                object result = DatabaseUtil.GetDotNetTypeValue(properties[i].PropertyType,
                                    ds.Tables[0].Rows[j][properties[i].Name]);
                                properties[i].SetValue(app, result, null);
                            }
                        }
                        col.Add(app);
                    }
                return col;
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);

            }
            return col;
        }

        public ObservableCollection<Search> SearchPatientsFromServer()
        {
            ObservableCollection<Search> col = new ObservableCollection<Search>();
            try
            {
                SqlParameter[] cmdParams = new SqlParameter[1];
                DataSet ds = remoteDB.ExecuteDataSet("SELECT * FROM A_Patient WITH(NOLOCK) WHERE RecordDate >= '" + DateTime.Now.ToString("yyyyMMdd") + "'", CommandType.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Search search = new Search();
                        search.Patient_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["WorkNo"]);
                        search.Ultrasound_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["WorkNo"]);
                        search.Name = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["Name"]);
                        search.Case_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["CaseNumber"]);
                        search.Insurance_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["InsuranceNumber"]);
                        search.Sex = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["Sex"]);
                        search.Phone_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["PhoneNumber"]);
                        search.Id_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["IDCardNumber"]);
                        search.Age = DatabaseUtil.ConvertToDotNetTypeValue<int?>(ds.Tables[0].Rows[i]["Age"]);
                        search.AgeType = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["AgeType"]);
                        search.Contact_Address = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["ContactAddress"]);
                        search.Patient_Type = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[i]["PatientType"]);
                        search.Examine_Time = DatabaseUtil.ConvertToDotNetTypeValue<DateTime?>(ds.Tables[0].Rows[i]["ExamineTime"]);
                        col.Add(search);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
                ErrorHandle.HandlerWarning(e.Message + "\r\n" + e.StackTrace);
            }
            return col;
        }

        public ObservableCollection<Search> SelectFromServer(string condition, int maxnum)
        {
            Patient patient = new Patient();
            
            return null;
        }
    }
}
