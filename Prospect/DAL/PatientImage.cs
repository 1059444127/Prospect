using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace com.cloud.prospect
{
    public partial class PatientImage : DALModel<PatientImage>
    {
        public PatientImage()
            : base("A_Image")
        {

        }

        public void DeleteByKey()
        {
            if (String.IsNullOrEmpty(Patient_Number)) return;
            string condtion = " Patient_Number = '" + Patient_Number + "'"
                + " AND Image_Number = " + Image_Number;
            CommonDelete(this, condtion);
        }

        public void CreateNewImage(string baseDir)
        {
            if (!String.IsNullOrEmpty(Patient_Number))
            {
                SqlParameter[] cmdParams = new SqlParameter[4];
                cmdParams[0] = new SqlParameter("@Patient_Number", SqlDbType.NVarChar, 30);
                cmdParams[0].Value = Patient_Number;
                cmdParams[1] = new SqlParameter("@BaseDir", SqlDbType.NVarChar, 255);
                cmdParams[1].Value = baseDir;
                cmdParams[2] = new SqlParameter("@Image_Format", SqlDbType.NVarChar, 18);
                cmdParams[2].Value = Image_Format;
                cmdParams[3] = new SqlParameter("@Image_Type", SqlDbType.NVarChar, 18);
                cmdParams[3].Value = Image_Type;
                DataSet ds = ExecuteDataSet("USP_Common_Create_Image", CommandType.StoredProcedure, cmdParams);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.Image_Path = DatabaseUtil.ConvertToDotNetTypeValue<string>(ds.Tables[0].Rows[0][0]);
                    this.Image_Number = DatabaseUtil.ConvertToDotNetTypeValue<int?>(ds.Tables[0].Rows[0][1]);
                    FileInfo file = new FileInfo(this.Image_Path);
                    DirectoryInfo dir = file.Directory;
                    if (!dir.Exists)
                        dir.Create();
                }
                else
                {
                    ErrorHandle.HandlerWarning("保存图片错误:返回图片位置错误。", "PatientImage.CreateNewImage()");
                }
            }
            else
            {
                ErrorHandle.HandlerWarning("保存图片错误：病人编号为空。", "PatientImage.CreateNewImage()");
            }
        }

        public void DeletePatientImages()
        {
            CommonDelete(this, "Patient_Number = '" + Patient_Number + "'");
        }

        public List<PatientImage> GetPatientImages()
        {
            return CommonSelect(this, "Patient_Number = '" + Patient_Number + "' ORDER BY Image_Number", null);
        }


    }
}
