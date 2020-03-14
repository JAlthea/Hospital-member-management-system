namespace WindowsFormsApplication1
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.btnSQL = new MetroFramework.Controls.MetroButton();
            this.lblSQL = new MetroFramework.Controls.MetroLabel();
            this.btnAdd = new MetroFramework.Controls.MetroButton();
            this.btnDel = new MetroFramework.Controls.MetroButton();
            this.btnFix = new MetroFramework.Controls.MetroButton();
            this.btnReset = new MetroFramework.Controls.MetroButton();
            this.btnSQLReset = new MetroFramework.Controls.MetroButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cb1 = new MetroFramework.Controls.MetroComboBox();
            this.btnCreate = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.LightSkyBlue;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(23, 127);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(669, 403);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_Click);
            // 
            // txtSQL
            // 
            this.txtSQL.Location = new System.Drawing.Point(23, 567);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(669, 27);
            this.txtSQL.TabIndex = 6;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(21, 72);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(37, 19);
            this.metroLabel1.TabIndex = 12;
            this.metroLabel1.Text = "열람";
            // 
            // btnSQL
            // 
            this.btnSQL.Location = new System.Drawing.Point(709, 567);
            this.btnSQL.Name = "btnSQL";
            this.btnSQL.Size = new System.Drawing.Size(77, 27);
            this.btnSQL.TabIndex = 13;
            this.btnSQL.Text = "SQL 전송";
            this.btnSQL.Click += new System.EventHandler(this.btSQL_Click);
            // 
            // lblSQL
            // 
            this.lblSQL.AutoSize = true;
            this.lblSQL.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblSQL.Location = new System.Drawing.Point(21, 545);
            this.lblSQL.Name = "lblSQL";
            this.lblSQL.Size = new System.Drawing.Size(35, 19);
            this.lblSQL.TabIndex = 14;
            this.lblSQL.Text = "SQL";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(709, 468);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(77, 27);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "추가하기";
            this.btnAdd.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(792, 468);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(77, 27);
            this.btnDel.TabIndex = 16;
            this.btnDel.Text = "삭제하기";
            this.btnDel.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnFix
            // 
            this.btnFix.Location = new System.Drawing.Point(792, 501);
            this.btnFix.Name = "btnFix";
            this.btnFix.Size = new System.Drawing.Size(77, 27);
            this.btnFix.TabIndex = 17;
            this.btnFix.Text = "수정하기";
            this.btnFix.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(709, 501);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(77, 27);
            this.btnReset.TabIndex = 18;
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnSQLReset
            // 
            this.btnSQLReset.Location = new System.Drawing.Point(792, 567);
            this.btnSQLReset.Name = "btnSQLReset";
            this.btnSQLReset.Size = new System.Drawing.Size(77, 27);
            this.btnSQLReset.TabIndex = 19;
            this.btnSQLReset.Text = "SQL Reset";
            this.btnSQLReset.Click += new System.EventHandler(this.btnSQLReset_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(415, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(105, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // cb1
            // 
            this.cb1.FontWeight = MetroFramework.MetroLinkWeight.Bold;
            this.cb1.FormattingEnabled = true;
            this.cb1.ItemHeight = 23;
            this.cb1.Location = new System.Drawing.Point(23, 92);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(121, 29);
            this.cb1.Style = MetroFramework.MetroColorStyle.Black;
            this.cb1.TabIndex = 21;
            this.cb1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.cb1.SelectedValueChanged += new System.EventHandler(this.cb1_SelectedValueChanged);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(150, 92);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(81, 29);
            this.btnCreate.TabIndex = 22;
            this.btnCreate.Text = "계정관리";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(965, 617);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.cb1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnSQLReset);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnFix);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblSQL);
            this.Controls.Add(this.btnSQL);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.txtSQL);
            this.Controls.Add(this.dataGridView1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroForm.MetroFormShadowType.SystemShadow;
            this.Style = MetroFramework.MetroColorStyle.Blue;
            this.Text = "HMS";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtSQL;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton btnSQL;
        private MetroFramework.Controls.MetroLabel lblSQL;
        private MetroFramework.Controls.MetroButton btnAdd;
        private MetroFramework.Controls.MetroButton btnDel;
        private MetroFramework.Controls.MetroButton btnFix;
        private MetroFramework.Controls.MetroButton btnReset;
        private MetroFramework.Controls.MetroButton btnSQLReset;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroComboBox cb1;
        private MetroFramework.Controls.MetroButton btnCreate;
    }
}

