
namespace com.cloud.prospect
{
    partial class ReportControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FastReport.Design.DesignerSettings designerSettings1 = new FastReport.Design.DesignerSettings();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportControl));
            FastReport.Design.DesignerRestrictions designerRestrictions1 = new FastReport.Design.DesignerRestrictions();
            FastReport.Export.Email.EmailSettings emailSettings1 = new FastReport.Export.Email.EmailSettings();
            FastReport.PreviewSettings previewSettings1 = new FastReport.PreviewSettings();
            FastReport.ReportSettings reportSettings1 = new FastReport.ReportSettings();
            this.previewControl = new FastReport.Preview.PreviewControl();
            this.evrSetting = new FastReport.EnvironmentSettings();
            this.SuspendLayout();
            // 
            // previewControl
            // 
            this.previewControl.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.previewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewControl.Font = new System.Drawing.Font("SimSun", 9F);
            this.previewControl.Location = new System.Drawing.Point(0, 0);
            this.previewControl.Name = "previewControl";
            this.previewControl.PageOffset = new System.Drawing.Point(10, 10);
            this.previewControl.Size = new System.Drawing.Size(951, 721);
            this.previewControl.TabIndex = 0;
            // 
            // evrSetting
            // 
            designerSettings1.ApplicationConnection = null;
            designerSettings1.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            //designerSettings1.Icon = ((System.Drawing.Icon)(resources.GetObject("designerSettings1.Icon")));
            designerSettings1.Restrictions = designerRestrictions1;
            designerSettings1.Text = "";
            this.evrSetting.DesignerSettings = designerSettings1;
            emailSettings1.Address = "";
            emailSettings1.Host = "";
            emailSettings1.MessageTemplate = "";
            emailSettings1.Name = "";
            emailSettings1.Password = "";
            emailSettings1.UserName = "";
            this.evrSetting.EmailSettings = emailSettings1;
            //previewSettings1.Icon = ((System.Drawing.Icon)(resources.GetObject("previewSettings1.Icon")));
            previewSettings1.Text = "";
            this.evrSetting.PreviewSettings = previewSettings1;
            this.evrSetting.ReportSettings = reportSettings1;
            this.evrSetting.UIStyle = FastReport.Utils.UIStyle.Office2007Blue;
            this.evrSetting.DatabaseLogin += new FastReport.DatabaseLoginEventHandler(this.evrSetting_DatabaseLogin);
            // 
            // ReportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewControl);
            this.Name = "ReportControl";
            this.Size = new System.Drawing.Size(951, 721);
            this.Load += new System.EventHandler(this.ReportControl_Load);
            this.Disposed += new System.EventHandler(this.ReportControl_Disposed);
            this.ResumeLayout(false);

        }

        private FastReport.Preview.PreviewControl previewControl;
        #endregion
        private FastReport.EnvironmentSettings evrSetting;
        
    }
}
