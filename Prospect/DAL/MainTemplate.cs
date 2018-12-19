using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace com.cloud.prospect
{
    public partial class MainTemplate : DALModel<MainTemplate>
    {
        public static ObservableCollection<MainTemplate> BodyPartCollection = null;
        public static ObservableCollection<MainTemplate> ExpressTemplate = null;
        public MainTemplate()
            : base("S_Template_Main")
        {
            Children = new ObservableCollection<MainTemplate>();
        }

        public void Insert()
        {
            SqlParameter[] cmdParams = new SqlParameter[5];
            cmdParams[0] = new SqlParameter("@Parent_Id", System.Data.SqlDbType.Int);
            cmdParams[0].Value = Parent_Id;
            cmdParams[1] = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 100);
            cmdParams[1].Value = Name;
            cmdParams[2] = new SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 3000);
            cmdParams[2].Value = Description;
            cmdParams[3] = new SqlParameter("@OrderID", System.Data.SqlDbType.Int);
            cmdParams[3].Value = OrderID;
            cmdParams[4] = new SqlParameter("@NodeLevel", System.Data.SqlDbType.Int);
            cmdParams[4].Value = NodeLevel;

            DataSet ds = database.ExecuteDataSet("USP_Common_Template_Main_Insert",
                System.Data.CommandType.StoredProcedure, cmdParams);
            Template_Id = (int)DatabaseUtil.GetDotNetTypeValue(typeof(int), ds.Tables[0].Rows[0][0]);
        }

        public void DeleteThisAndChildren()
        {
            SqlParameter[] cmdParams = new SqlParameter[1];
            cmdParams[0] = new SqlParameter("@Template_Id", System.Data.SqlDbType.Int);
            cmdParams[0].Value = Template_Id;
            database.ExecuteNonQuery("USP_Common_Template_Main_Delete", CommandType.StoredProcedure, cmdParams);
        }

        public void UpdateFinding(string finding)
        {
            SqlParameter[] cmdParams = new SqlParameter[2];
            cmdParams[0] = new SqlParameter("@Template_Id", System.Data.SqlDbType.Int);
            cmdParams[0].Value = Template_Id;
            cmdParams[1] = new SqlParameter("@Finding", System.Data.SqlDbType.NVarChar, 3000);
            cmdParams[1].Value = finding;
            database.ExecuteNonQuery("USP_Common_Template_Main_Finding_Update", CommandType.StoredProcedure, cmdParams);
        }

        public void UpdatePrompt(string prompt)
        {
            SqlParameter[] cmdParams = new SqlParameter[2];
            cmdParams[0] = new SqlParameter("@Template_Id", System.Data.SqlDbType.Int);
            cmdParams[0].Value = Template_Id;
            cmdParams[1] = new SqlParameter("@Prompt", System.Data.SqlDbType.NVarChar, 3000);
            cmdParams[1].Value = prompt;
            database.ExecuteNonQuery("USP_Common_Template_Main_Prompt_Update", CommandType.StoredProcedure, cmdParams);
        }

        public void UpdateName()
        {
            CommonUpdate(this, "Template_Id = '" + Template_Id + "'", new string[] { "Name" });
        }


        public ObservableCollection<MainTemplate> LoadBodyPart()
        {
            if (BodyPartCollection == null)
            {
                List<MainTemplate> lstTemplate = CommonSelect(this, "1=1", null);
                foreach (MainTemplate template in lstTemplate)
                {
                    if (template.NodeLevel == 1)
                        template.ImagePath = @"pack://application:,,,/Prospect;Component/img/blueround.png";
                    else if (template.NodeLevel == 2)
                        template.ImagePath = @"pack://application:,,,/Prospect;Component/img/yellowround.png";
                }
                BodyPartCollection = RecursionTemplate(lstTemplate, 0, 0, 2);
            }
            return BodyPartCollection;
        }

        public ObservableCollection<MainTemplate> LoadTemplateList()
        {
            return new ObservableCollection<MainTemplate>(
                CommonSelect(this, "Parent_Id = " + Template_Id + " ORDER BY OrderID", null));
        }

        public ObservableCollection<MainTemplate> LoadExpressTemplate()
        {
            if (ExpressTemplate == null)
            {
                List<MainTemplate> lstTemplate = CommonSelect(this, "1=1", null);
                foreach (MainTemplate template in lstTemplate)
                {
                    if (template.NodeLevel == 1)
                        template.ImagePath = @"pack://application:,,,/Prospect;Component/img/blueround.png";
                    else if (template.NodeLevel == 2)
                        template.ImagePath = @"pack://application:,,,/Prospect;Component/img/yellowround.png";
                    else if (template.NodeLevel == 3)
                        template.ImagePath = @"pack://application:,,,/Prospect;Component/img/redround.png";
                }
                ExpressTemplate = RecursionTemplate(lstTemplate, 0, 0, 3);
            }
            return ExpressTemplate;
        }

        static ObservableCollection<MainTemplate> RecursionTemplate(IEnumerable<MainTemplate> collection,
             int id, int level, int maxlvl)
        {
            level++;
            if (level <= maxlvl)
            {
                var children = from template in collection
                               where template.Parent_Id == id
                               orderby template.OrderID
                               select template;
                foreach (MainTemplate template in children)
                {
                    template.Children = RecursionTemplate(collection, template.Template_Id, level, maxlvl);
                }
                return new ObservableCollection<MainTemplate>(children);
            }
            return null;
        }


        public string GetUltrasoundFinding()
        {
            MainTemplate template = new MainTemplate();
            CommonSelectSingleRow(template, "Parent_id = " + Template_Id + " AND Name = '所见'", null);
            return template.Description;
        }

        public string GetUltrasoundPrompt()
        {
            MainTemplate template = new MainTemplate();
            CommonSelectSingleRow(template, "Parent_id = " + Template_Id + " AND Name = '提示'", null);
            return template.Description;
        }

        public void MoveNodeUp()
        {
            try
            {
                int nextId = 0;
                int nextTemplateId = 0;

                string sql = "SELECT TOP 1 Template_Id,OrderID FROM S_Template_Main WITH(NOLOCK) WHERE OrderId < " + this.OrderID + " AND NodeLevel = " + this.NodeLevel + " AND Parent_Id = " + this.Parent_Id + " ORDER BY OrderId DESC";
                DataSet ds = database.ExecuteDataSet(sql, CommandType.Text);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    nextTemplateId = DatabaseUtil.ConvertToDotNetTypeValue<int>(ds.Tables[0].Rows[0][0]);
                    nextId = DatabaseUtil.ConvertToDotNetTypeValue<int>(ds.Tables[0].Rows[0][1]);
                    MainTemplate next = new MainTemplate();
                    CommonSelectSingleRow(next, "Template_id = " + nextTemplateId, null);
                    next.OrderID = this.OrderID;
                    this.OrderID = nextId;
                    CommonUpdate(next, "Template_id = " + nextTemplateId, new string[] { "OrderID" });
                    CommonUpdate(this, "Template_id = " + this.Template_Id, new string[] { "OrderID" });
                }
            }
            catch (Exception)
            {

                throw;
            }



        }

        public void MoveNodeDown()
        {
            try
            {
                int nextId = 0;
                int nextTemplateId = 0;

                string sql = "SELECT TOP 1 Template_Id,OrderID FROM S_Template_Main WITH(NOLOCK) WHERE OrderId > " + this.OrderID + " AND NodeLevel = " + this.NodeLevel + " AND Parent_Id = " + this.Parent_Id + " ORDER BY OrderId";
                DataSet ds = database.ExecuteDataSet(sql, CommandType.Text);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    nextTemplateId = DatabaseUtil.ConvertToDotNetTypeValue<int>(ds.Tables[0].Rows[0][0]);
                    nextId = DatabaseUtil.ConvertToDotNetTypeValue<int>(ds.Tables[0].Rows[0][1]);
                    MainTemplate next = new MainTemplate();
                    CommonSelectSingleRow(next, "Template_id = " + nextTemplateId, null);
                    next.OrderID = this.OrderID;
                    this.OrderID = nextId;
                    CommonUpdate(next, "Template_id = " + nextTemplateId, new string[] { "OrderID" });
                    CommonUpdate(this, "Template_id = " + this.Template_Id, new string[] { "OrderID" });
                }
            }
            catch (Exception)
            {

                throw;
            }



        }
    }
}
