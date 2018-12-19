using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace com.cloud.prospect
{
    public partial class Patient : DALModel<Patient>
    {
        public Patient() : base("A_Patient") { }

        #region Stardand Database Operation
        public void SelectSingleRow(string condition)
        {
            if (!CommonSelectSingleRow(this, condition, null))
            {
                this.Patient_Number = null;
            }
        }

        public void DeleteByPK()
        {
            if (String.IsNullOrEmpty(Patient_Number)) return;
            string condtion = " Patient_Number = '" + Patient_Number + "'";
            CommonDelete(this, condtion);
        }

        public void SelectByPK()
        {
            if (String.IsNullOrEmpty(Patient_Number)) return;
            string condtion = " Patient_Number = '" + Patient_Number + "'";
            SelectSingleRow(condtion);
        }

        public void Update()
        {
            if (String.IsNullOrEmpty(Patient_Number)) return;
            string condition = "Patient_Number = '" + Patient_Number + "'";
            CommonUpdate(this, condition, null);
        }



        public void Insert()
        {
            CommonInsert(this, null);
        }
        #endregion

        #region Other Database Operation
        public void GetLatestRecord()
        {
            string condition = "Serial_Number = (SELECT MAX(Serial_Number) FROM A_Patient WITH(NOLOCK))";
            SelectSingleRow(condition);
        }

        public void GetFirstRecord()
        {
            string condition = "Serial_Number = (SELECT MIN(Serial_Number) FROM A_Patient WITH(NOLOCK))";
            SelectSingleRow(condition);
        }

        public void GetNextRecord()
        {
            SqlParameter[] cmdParams = new SqlParameter[1];
            cmdParams[0] = new SqlParameter("@Current_Patient_Number", SqlDbType.NVarChar, 30);
            cmdParams[0].Value = Patient_Number;
            DataSet ds = database.ExecuteDataSet("USP_Common_Next_Record_Select", CommandType.StoredProcedure, cmdParams);
            PropertyInfo[] properties = this.GetType().GetProperties();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                for (int i = 0; i < properties.Count(); i++)
                {
                    object result = DatabaseUtil.GetDotNetTypeValue(properties[i].PropertyType,
                        ds.Tables[0].Rows[0][properties[i].Name]);
                    properties[i].SetValue(this, result, null);
                }
        }

        public void GetLastRecord()
        {
            SqlParameter[] cmdParams = new SqlParameter[1];
            cmdParams[0] = new SqlParameter("@Current_Patient_Number", SqlDbType.NVarChar, 30);
            cmdParams[0].Value = Patient_Number;
            DataSet ds = database.ExecuteDataSet("USP_Common_Last_Record_Select", CommandType.StoredProcedure, cmdParams);
            PropertyInfo[] properties = this.GetType().GetProperties();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                for (int i = 0; i < properties.Count(); i++)
                {
                    object result = DatabaseUtil.GetDotNetTypeValue(properties[i].PropertyType,
                        ds.Tables[0].Rows[0][properties[i].Name]);
                    properties[i].SetValue(this, result, null);
                }
        }

        public void CreatePatientNumber()
        {
            DataSet ds = ExecuteDataSet("USP_Common_Create_Patient_Number", CommandType.StoredProcedure);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.Patient_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0][0]);
            }
        }

        public List<Patient> Select(string condition)
        {
            return CommonSelect(this, condition, null);
        }

        public void GetPatientInfoFromServer()
        {
            try
            {
                SqlParameter[] cmdParams = new SqlParameter[1];
                DataSet ds = remoteDB.ExecuteDataSet("SELECT * FROM A_Patient WITH(NOLOCK) WHERE WorkNo = '" + Patient_Number + "'", CommandType.Text); 
               // DataSet ds = remoteDB.ExecuteDataSet("SELECT * FROM RemoteServer WITH(NOLOCK) WHERE WorkNo = '" + Patient_Number + "'", CommandType.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.Patient_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["WorkNo"]);
                    this.Name = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["Name"]);
                    this.Case_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["CaseNumber"]);
                    this.Insurance_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["InsuranceNumber"]);
                    this.Sex = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["Sex"]);
                    this.Phone_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["PhoneNumber"]);
                    this.Id_Number = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["IDCardNumber"]);
                    this.Age = DatabaseUtil.ConvertToDotNetTypeValue<int?>(ds.Tables[0].Rows[0]["Age"]);
                    this.AgeType = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["AgeType"]);
                    this.Contact_Address = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["ContactAddress"]);
                    this.Patient_Type = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0]["PatientType"]);
                    this.Examine_Time = DatabaseUtil.ConvertToDotNetTypeValue<DateTime?>(ds.Tables[0].Rows[0]["ExamineTime"]);
                    return;
                }
                this.Patient_Number = null;
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }
        }

      

        public int CheckIsExistsOrError()
        {
            DataSet ds = database.ExecuteDataSet("SELECT TOP 1 1 FROM A_Patient NOLOCK WHERE Patient_Number = '" + Patient_Number + "'", CommandType.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return 1;
            }
            return 0;
        }

        public void ChangePK()
        {
            database.ExecuteNonQuery("Update A_Patient WITH(ROWLOCK) SET Patient_Number='" + Patient_Number + "A' WHERE Patient_Number='" + Patient_Number + "'", CommandType.Text);
        }
        #endregion


    }
}
