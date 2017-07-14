namespace QianTang_2
{
    partial class UserScoreSearch
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.numEndMonth = new System.Windows.Forms.NumericUpDown();
            this.txtCusName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numStartMonth = new System.Windows.Forms.NumericUpDown();
            this.chkIsEnable = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numEndYear = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.completeStart = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.numStartYear = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.completeEnd = new System.Windows.Forms.DateTimePicker();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.resultGrid = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inputdata = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cablenumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contractType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saler = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.complete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.money = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paytype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.numEndMonth);
            this.splitContainer1.Panel1.Controls.Add(this.txtCusName);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.numStartMonth);
            this.splitContainer1.Panel1.Controls.Add(this.chkIsEnable);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.btnSearch);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.numEndYear);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.completeStart);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.numStartYear);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.completeEnd);
            this.splitContainer1.Panel1.Controls.Add(this.cmbStatus);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.resultGrid);
            this.splitContainer1.Size = new System.Drawing.Size(1232, 577);
            this.splitContainer1.SplitterDistance = 52;
            this.splitContainer1.TabIndex = 0;
            // 
            // numEndMonth
            // 
            this.numEndMonth.Location = new System.Drawing.Point(585, 16);
            this.numEndMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numEndMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEndMonth.Name = "numEndMonth";
            this.numEndMonth.Size = new System.Drawing.Size(33, 21);
            this.numEndMonth.TabIndex = 55;
            this.numEndMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtCusName
            // 
            this.txtCusName.Location = new System.Drawing.Point(721, 16);
            this.txtCusName.Name = "txtCusName";
            this.txtCusName.Size = new System.Drawing.Size(151, 21);
            this.txtCusName.TabIndex = 62;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(566, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 53;
            this.label9.Text = "年";
            // 
            // numStartMonth
            // 
            this.numStartMonth.Location = new System.Drawing.Point(444, 16);
            this.numStartMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numStartMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartMonth.Name = "numStartMonth";
            this.numStartMonth.Size = new System.Drawing.Size(33, 21);
            this.numStartMonth.TabIndex = 58;
            this.numStartMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkIsEnable
            // 
            this.chkIsEnable.AutoSize = true;
            this.chkIsEnable.Location = new System.Drawing.Point(10, 19);
            this.chkIsEnable.Name = "chkIsEnable";
            this.chkIsEnable.Size = new System.Drawing.Size(72, 16);
            this.chkIsEnable.TabIndex = 48;
            this.chkIsEnable.Text = "完工日期";
            this.chkIsEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkIsEnable.UseVisualStyleBackColor = true;
            this.chkIsEnable.CheckedChanged += new System.EventHandler(this.chkIsEnable_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(624, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 54;
            this.label8.Text = "月";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1071, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 65;
            this.btnSearch.Text = "查  询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(310, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 61;
            this.label4.Text = "导入月度:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(656, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 60;
            this.label3.Text = "客户名称:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(421, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "年";
            // 
            // numEndYear
            // 
            this.numEndYear.Location = new System.Drawing.Point(520, 16);
            this.numEndYear.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numEndYear.Minimum = new decimal(new int[] {
            1111,
            0,
            0,
            0});
            this.numEndYear.Name = "numEndYear";
            this.numEndYear.Size = new System.Drawing.Size(44, 21);
            this.numEndYear.TabIndex = 56;
            this.numEndYear.Value = new decimal(new int[] {
            1111,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(480, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 52;
            this.label5.Text = "月 至";
            // 
            // completeStart
            // 
            this.completeStart.CustomFormat = "yyyy-MM-dd";
            this.completeStart.Enabled = false;
            this.completeStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.completeStart.Location = new System.Drawing.Point(86, 16);
            this.completeStart.Name = "completeStart";
            this.completeStart.Size = new System.Drawing.Size(91, 21);
            this.completeStart.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(878, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 64;
            this.label6.Text = "结算状态:";
            // 
            // numStartYear
            // 
            this.numStartYear.Location = new System.Drawing.Point(375, 16);
            this.numStartYear.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numStartYear.Minimum = new decimal(new int[] {
            1111,
            0,
            0,
            0});
            this.numStartYear.Name = "numStartYear";
            this.numStartYear.Size = new System.Drawing.Size(44, 21);
            this.numStartYear.TabIndex = 57;
            this.numStartYear.Value = new decimal(new int[] {
            1111,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 51;
            this.label2.Text = "至";
            // 
            // completeEnd
            // 
            this.completeEnd.CustomFormat = "yyyy-MM-dd";
            this.completeEnd.Enabled = false;
            this.completeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.completeEnd.Location = new System.Drawing.Point(206, 16);
            this.completeEnd.Name = "completeEnd";
            this.completeEnd.Size = new System.Drawing.Size(91, 21);
            this.completeEnd.TabIndex = 59;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "正常结算",
            "预结算",
            "补结算",
            "全部"});
            this.cmbStatus.Location = new System.Drawing.Point(942, 17);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(119, 20);
            this.cmbStatus.TabIndex = 63;
            // 
            // resultGrid
            // 
            this.resultGrid.AllowUserToAddRows = false;
            this.resultGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(228)))), ((int)(((byte)(246)))));
            this.resultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.inputdata,
            this.cablenumber,
            this.contractType,
            this.customer,
            this.saler,
            this.complete,
            this.remove,
            this.startDate,
            this.endDate,
            this.money,
            this.paytype,
            this.receivable});
            this.resultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultGrid.Location = new System.Drawing.Point(0, 0);
            this.resultGrid.Name = "resultGrid";
            this.resultGrid.RowHeadersVisible = false;
            this.resultGrid.RowTemplate.Height = 23;
            this.resultGrid.Size = new System.Drawing.Size(1232, 521);
            this.resultGrid.TabIndex = 29;
            // 
            // No
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.No.DefaultCellStyle = dataGridViewCellStyle1;
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 30;
            // 
            // inputdata
            // 
            this.inputdata.HeaderText = "导入月度";
            this.inputdata.Name = "inputdata";
            this.inputdata.ReadOnly = true;
            this.inputdata.Width = 90;
            // 
            // cablenumber
            // 
            this.cablenumber.HeaderText = "电路代码";
            this.cablenumber.Name = "cablenumber";
            this.cablenumber.ReadOnly = true;
            // 
            // contractType
            // 
            this.contractType.HeaderText = "电路类型";
            this.contractType.Name = "contractType";
            this.contractType.ReadOnly = true;
            // 
            // customer
            // 
            this.customer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.customer.HeaderText = "客户名称";
            this.customer.Name = "customer";
            this.customer.ReadOnly = true;
            // 
            // saler
            // 
            this.saler.HeaderText = "主销售渠道";
            this.saler.Name = "saler";
            this.saler.ReadOnly = true;
            // 
            // complete
            // 
            dataGridViewCellStyle2.Format = "d";
            dataGridViewCellStyle2.NullValue = null;
            this.complete.DefaultCellStyle = dataGridViewCellStyle2;
            this.complete.HeaderText = "完工日期";
            this.complete.Name = "complete";
            this.complete.ReadOnly = true;
            // 
            // remove
            // 
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            this.remove.DefaultCellStyle = dataGridViewCellStyle3;
            this.remove.HeaderText = "拆机日期";
            this.remove.Name = "remove";
            this.remove.ReadOnly = true;
            // 
            // startDate
            // 
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.startDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.startDate.HeaderText = "结算起始日期";
            this.startDate.Name = "startDate";
            this.startDate.ReadOnly = true;
            // 
            // endDate
            // 
            this.endDate.HeaderText = "结算截止日期";
            this.endDate.Name = "endDate";
            this.endDate.ReadOnly = true;
            // 
            // money
            // 
            this.money.HeaderText = "合同金额";
            this.money.Name = "money";
            this.money.ReadOnly = true;
            // 
            // paytype
            // 
            this.paytype.HeaderText = "付款方式";
            this.paytype.Name = "paytype";
            this.paytype.ReadOnly = true;
            // 
            // receivable
            // 
            this.receivable.HeaderText = "销账金额";
            this.receivable.Name = "receivable";
            this.receivable.ReadOnly = true;
            // 
            // UserScoreSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UserScoreSearch";
            this.Size = new System.Drawing.Size(1232, 577);
            this.Load += new System.EventHandler(this.UserScoreSearch_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numEndMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NumericUpDown numEndMonth;
        private System.Windows.Forms.TextBox txtCusName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numStartMonth;
        private System.Windows.Forms.CheckBox chkIsEnable;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numEndYear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker completeStart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numStartYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker completeEnd;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.DataGridView resultGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn inputdata;
        private System.Windows.Forms.DataGridViewTextBoxColumn cablenumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn contractType;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn saler;
        private System.Windows.Forms.DataGridViewTextBoxColumn complete;
        private System.Windows.Forms.DataGridViewTextBoxColumn remove;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn money;
        private System.Windows.Forms.DataGridViewTextBoxColumn paytype;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivable;
    }
}
