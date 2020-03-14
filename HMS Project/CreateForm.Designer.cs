namespace WindowsFormsApplication1
{
    partial class CreateForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbID = new MetroFramework.Controls.MetroTextBox();
            this.tbPW = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.btnCreate = new MetroFramework.Controls.MetroButton();
            this.cbType = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(87, 102);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(95, 23);
            this.tbID.TabIndex = 0;
            // 
            // tbPW
            // 
            this.tbPW.Location = new System.Drawing.Point(87, 131);
            this.tbPW.Name = "tbPW";
            this.tbPW.Size = new System.Drawing.Size(95, 23);
            this.tbPW.TabIndex = 1;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(34, 102);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(21, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "ID";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(34, 131);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(30, 19);
            this.metroLabel2.TabIndex = 3;
            this.metroLabel2.Text = "PW";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(36, 177);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "생성하기";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // cbType
            // 
            this.cbType.FontWeight = MetroFramework.MetroLinkWeight.Bold;
            this.cbType.FormattingEnabled = true;
            this.cbType.ItemHeight = 23;
            this.cbType.Items.AddRange(new object[] {
            "의사",
            "간호사",
            "환자"});
            this.cbType.Location = new System.Drawing.Point(87, 67);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(95, 29);
            this.cbType.Style = MetroFramework.MetroColorStyle.Black;
            this.cbType.TabIndex = 6;
            this.cbType.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(34, 67);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(36, 19);
            this.metroLabel3.TabIndex = 7;
            this.metroLabel3.Text = "Type";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(128, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "취소하기";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 223);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.tbPW);
            this.Controls.Add(this.tbID);
            this.MaximizeBox = false;
            this.Name = "CreateForm";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "사용자 계정 관리";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox tbID;
        private MetroFramework.Controls.MetroTextBox tbPW;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton btnCreate;
        private MetroFramework.Controls.MetroComboBox cbType;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroButton btnCancel;
    }
}