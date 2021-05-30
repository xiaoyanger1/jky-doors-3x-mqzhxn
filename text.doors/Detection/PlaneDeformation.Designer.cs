namespace text.doors.Detection
{
    partial class PlaneDeformation
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
            this.components = new System.ComponentModel.Container();
            this.tc_RealTimeSurveillance = new System.Windows.Forms.TabControl();
            this.page_airtight = new System.Windows.Forms.TabPage();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_tgstart = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txt_desc = new System.Windows.Forms.TextBox();
            this.btn_datadispose = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_stop1 = new System.Windows.Forms.Button();
            this.btn_level5 = new System.Windows.Forms.Button();
            this.btn_level4 = new System.Windows.Forms.Button();
            this.btn_level3 = new System.Windows.Forms.Button();
            this.btn_level2 = new System.Windows.Forms.Button();
            this.btn_level1 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_hl = new System.Windows.Forms.Button();
            this.btn_qt = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgv_level = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_biaojilingdian = new System.Windows.Forms.TextBox();
            this.btn_zore = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.tChart_pm = new Steema.TeeChart.TChart();
            this.pm_Line = new Steema.TeeChart.Styles.FastLine();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_dqyl = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_reslevel = new System.Windows.Forms.ComboBox();
            this.tim_PainPic = new System.Windows.Forms.Timer(this.components);
            this.tim_WY = new System.Windows.Forms.Timer(this.components);
            this.tim_getType = new System.Windows.Forms.Timer(this.components);
            this.tc_RealTimeSurveillance.SuspendLayout();
            this.page_airtight.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_level)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc_RealTimeSurveillance
            // 
            this.tc_RealTimeSurveillance.Controls.Add(this.page_airtight);
            this.tc_RealTimeSurveillance.ItemSize = new System.Drawing.Size(120, 26);
            this.tc_RealTimeSurveillance.Location = new System.Drawing.Point(1, 2);
            this.tc_RealTimeSurveillance.Name = "tc_RealTimeSurveillance";
            this.tc_RealTimeSurveillance.SelectedIndex = 0;
            this.tc_RealTimeSurveillance.Size = new System.Drawing.Size(1096, 675);
            this.tc_RealTimeSurveillance.TabIndex = 1;
            // 
            // page_airtight
            // 
            this.page_airtight.BackColor = System.Drawing.Color.White;
            this.page_airtight.Controls.Add(this.btn_stop);
            this.page_airtight.Controls.Add(this.btn_tgstart);
            this.page_airtight.Controls.Add(this.groupBox7);
            this.page_airtight.Controls.Add(this.btn_datadispose);
            this.page_airtight.Controls.Add(this.groupBox6);
            this.page_airtight.Controls.Add(this.groupBox5);
            this.page_airtight.Controls.Add(this.groupBox4);
            this.page_airtight.Controls.Add(this.groupBox2);
            this.page_airtight.Controls.Add(this.groupBox9);
            this.page_airtight.Controls.Add(this.groupBox1);
            this.page_airtight.Controls.Add(this.groupBox3);
            this.page_airtight.Location = new System.Drawing.Point(4, 30);
            this.page_airtight.Name = "page_airtight";
            this.page_airtight.Padding = new System.Windows.Forms.Padding(3);
            this.page_airtight.Size = new System.Drawing.Size(1088, 641);
            this.page_airtight.TabIndex = 0;
            this.page_airtight.Text = "平面内变形性能检测";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(974, 10);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(100, 28);
            this.btn_stop.TabIndex = 25;
            this.btn_stop.Text = "推杆停止";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_tgstart
            // 
            this.btn_tgstart.Location = new System.Drawing.Point(867, 10);
            this.btn_tgstart.Name = "btn_tgstart";
            this.btn_tgstart.Size = new System.Drawing.Size(101, 28);
            this.btn_tgstart.TabIndex = 24;
            this.btn_tgstart.Text = "推杆启动";
            this.btn_tgstart.UseVisualStyleBackColor = true;
            this.btn_tgstart.Click += new System.EventHandler(this.btn_tgstart_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txt_desc);
            this.groupBox7.Location = new System.Drawing.Point(867, 501);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(211, 88);
            this.groupBox7.TabIndex = 23;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "检测结果";
            // 
            // txt_desc
            // 
            this.txt_desc.Location = new System.Drawing.Point(7, 20);
            this.txt_desc.Multiline = true;
            this.txt_desc.Name = "txt_desc";
            this.txt_desc.Size = new System.Drawing.Size(195, 62);
            this.txt_desc.TabIndex = 2;
            // 
            // btn_datadispose
            // 
            this.btn_datadispose.Location = new System.Drawing.Point(867, 593);
            this.btn_datadispose.Name = "btn_datadispose";
            this.btn_datadispose.Size = new System.Drawing.Size(211, 28);
            this.btn_datadispose.TabIndex = 14;
            this.btn_datadispose.Text = "数据处理";
            this.btn_datadispose.UseVisualStyleBackColor = true;
            this.btn_datadispose.Click += new System.EventHandler(this.btn_datadispose_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_stop1);
            this.groupBox6.Controls.Add(this.btn_level5);
            this.groupBox6.Controls.Add(this.btn_level4);
            this.groupBox6.Controls.Add(this.btn_level3);
            this.groupBox6.Controls.Add(this.btn_level2);
            this.groupBox6.Controls.Add(this.btn_level1);
            this.groupBox6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox6.Location = new System.Drawing.Point(866, 326);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(212, 114);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "自动检测";
            // 
            // btn_stop1
            // 
            this.btn_stop1.Location = new System.Drawing.Point(108, 80);
            this.btn_stop1.Name = "btn_stop1";
            this.btn_stop1.Size = new System.Drawing.Size(96, 28);
            this.btn_stop1.TabIndex = 9;
            this.btn_stop1.Text = "停止";
            this.btn_stop1.UseVisualStyleBackColor = true;
            this.btn_stop1.Click += new System.EventHandler(this.btn_stop1_Click);
            // 
            // btn_level5
            // 
            this.btn_level5.Location = new System.Drawing.Point(8, 80);
            this.btn_level5.Name = "btn_level5";
            this.btn_level5.Size = new System.Drawing.Size(96, 28);
            this.btn_level5.TabIndex = 10;
            this.btn_level5.Text = "第5级";
            this.btn_level5.UseVisualStyleBackColor = true;
            this.btn_level5.Click += new System.EventHandler(this.btn_level5_Click);
            // 
            // btn_level4
            // 
            this.btn_level4.Location = new System.Drawing.Point(107, 49);
            this.btn_level4.Name = "btn_level4";
            this.btn_level4.Size = new System.Drawing.Size(96, 28);
            this.btn_level4.TabIndex = 7;
            this.btn_level4.Text = "第4级";
            this.btn_level4.UseVisualStyleBackColor = true;
            this.btn_level4.Click += new System.EventHandler(this.btn_level4_Click);
            // 
            // btn_level3
            // 
            this.btn_level3.Location = new System.Drawing.Point(8, 49);
            this.btn_level3.Name = "btn_level3";
            this.btn_level3.Size = new System.Drawing.Size(96, 28);
            this.btn_level3.TabIndex = 8;
            this.btn_level3.Text = "第3级";
            this.btn_level3.UseVisualStyleBackColor = true;
            this.btn_level3.Click += new System.EventHandler(this.btn_level3_Click);
            // 
            // btn_level2
            // 
            this.btn_level2.Location = new System.Drawing.Point(107, 17);
            this.btn_level2.Name = "btn_level2";
            this.btn_level2.Size = new System.Drawing.Size(96, 28);
            this.btn_level2.TabIndex = 5;
            this.btn_level2.Text = "第2级";
            this.btn_level2.UseVisualStyleBackColor = true;
            this.btn_level2.Click += new System.EventHandler(this.btn_level2_Click);
            // 
            // btn_level1
            // 
            this.btn_level1.Location = new System.Drawing.Point(8, 17);
            this.btn_level1.Name = "btn_level1";
            this.btn_level1.Size = new System.Drawing.Size(96, 28);
            this.btn_level1.TabIndex = 6;
            this.btn_level1.Text = "第1级";
            this.btn_level1.UseVisualStyleBackColor = true;
            this.btn_level1.Click += new System.EventHandler(this.btn_level1_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_hl);
            this.groupBox5.Controls.Add(this.btn_qt);
            this.groupBox5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox5.Location = new System.Drawing.Point(867, 266);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(211, 56);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "手动检测";
            // 
            // btn_hl
            // 
            this.btn_hl.Location = new System.Drawing.Point(106, 20);
            this.btn_hl.Name = "btn_hl";
            this.btn_hl.Size = new System.Drawing.Size(96, 28);
            this.btn_hl.TabIndex = 5;
            this.btn_hl.Text = "后拉";
            this.btn_hl.UseVisualStyleBackColor = true;
            this.btn_hl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_hl_MouseDown);
            this.btn_hl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_hl_MouseUp);
            // 
            // btn_qt
            // 
            this.btn_qt.Location = new System.Drawing.Point(7, 20);
            this.btn_qt.Name = "btn_qt";
            this.btn_qt.Size = new System.Drawing.Size(96, 28);
            this.btn_qt.TabIndex = 6;
            this.btn_qt.Text = "前推";
            this.btn_qt.UseVisualStyleBackColor = true;
            this.btn_qt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_qt_MouseDown);
            this.btn_qt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_qt_MouseUp);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgv_level);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(866, 45);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(212, 164);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "相关数据 mm";
            // 
            // dgv_level
            // 
            this.dgv_level.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_level.Location = new System.Drawing.Point(14, 20);
            this.dgv_level.Name = "dgv_level";
            this.dgv_level.RowHeadersWidth = 62;
            this.dgv_level.RowTemplate.Height = 23;
            this.dgv_level.Size = new System.Drawing.Size(184, 135);
            this.dgv_level.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_biaojilingdian);
            this.groupBox2.Controls.Add(this.btn_zore);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(866, 215);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(212, 47);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标记零点";
            // 
            // txt_biaojilingdian
            // 
            this.txt_biaojilingdian.Location = new System.Drawing.Point(6, 18);
            this.txt_biaojilingdian.Name = "txt_biaojilingdian";
            this.txt_biaojilingdian.Size = new System.Drawing.Size(96, 21);
            this.txt_biaojilingdian.TabIndex = 289;
            // 
            // btn_zore
            // 
            this.btn_zore.Location = new System.Drawing.Point(106, 13);
            this.btn_zore.Name = "btn_zore";
            this.btn_zore.Size = new System.Drawing.Size(94, 28);
            this.btn_zore.TabIndex = 7;
            this.btn_zore.Text = "置零";
            this.btn_zore.UseVisualStyleBackColor = true;
            this.btn_zore.Click += new System.EventHandler(this.btn_zore_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.tChart_pm);
            this.groupBox9.Location = new System.Drawing.Point(8, 87);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(852, 550);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "检测";
            // 
            // tChart_pm
            // 
            // 
            // 
            // 
            this.tChart_pm.Aspect.ZOffset = 0D;
            this.tChart_pm.BackColor = System.Drawing.Color.White;
            this.tChart_pm.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // 
            // 
            this.tChart_pm.Header.Lines = new string[] {
        "平面内变形性能检测"};
            // 
            // 
            // 
            this.tChart_pm.Legend.Visible = false;
            this.tChart_pm.Location = new System.Drawing.Point(10, 35);
            this.tChart_pm.Name = "tChart_pm";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_pm.Panel.Brush.Color = System.Drawing.Color.White;
            // 
            // 
            // 
            this.tChart_pm.Panel.Brush.Gradient.Visible = false;
            this.tChart_pm.Panel.MarginLeft = 0D;
            this.tChart_pm.Panel.MarginRight = 2D;
            this.tChart_pm.Panel.MarginTop = 0D;
            this.tChart_pm.Series.Add(this.pm_Line);
            this.tChart_pm.Size = new System.Drawing.Size(836, 480);
            this.tChart_pm.TabIndex = 19;
            // 
            // pm_Line
            // 
            this.pm_Line.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.pm_Line.ColorEach = false;
            // 
            // 
            // 
            this.pm_Line.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.pm_Line.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None;
            this.pm_Line.Marks.Callout.ArrowHeadSize = 8;
            // 
            // 
            // 
            this.pm_Line.Marks.Callout.Brush.Color = System.Drawing.Color.Black;
            this.pm_Line.Marks.Callout.Distance = 0;
            this.pm_Line.Marks.Callout.Draw3D = false;
            this.pm_Line.Marks.Callout.Length = 10;
            this.pm_Line.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            this.pm_Line.Title = "fastLine1";
            this.pm_Line.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.pm_Line.XValues.DataMember = "X";
            this.pm_Line.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.pm_Line.YValues.DataMember = "Y";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_dqyl);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lbl_title);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(851, 59);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "状态";
            // 
            // lbl_dqyl
            // 
            this.lbl_dqyl.AutoSize = true;
            this.lbl_dqyl.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_dqyl.Location = new System.Drawing.Point(673, 25);
            this.lbl_dqyl.Name = "lbl_dqyl";
            this.lbl_dqyl.Size = new System.Drawing.Size(17, 16);
            this.lbl_dqyl.TabIndex = 24;
            this.lbl_dqyl.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(497, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "当前位移（mm）：";
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_title.Location = new System.Drawing.Point(77, 26);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(231, 16);
            this.lbl_title.TabIndex = 20;
            this.lbl_title.Text = "平面内变形性能检测  第0号 ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_reslevel);
            this.groupBox3.Location = new System.Drawing.Point(867, 446);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 52);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "检测结果";
            // 
            // txt_reslevel
            // 
            this.txt_reslevel.FormattingEnabled = true;
            this.txt_reslevel.Items.AddRange(new object[] {
            "第一级",
            "第二级",
            "第三级",
            "第四级",
            "第五级"});
            this.txt_reslevel.Location = new System.Drawing.Point(7, 20);
            this.txt_reslevel.Name = "txt_reslevel";
            this.txt_reslevel.Size = new System.Drawing.Size(194, 20);
            this.txt_reslevel.TabIndex = 185;
            // 
            // tim_PainPic
            // 
            this.tim_PainPic.Enabled = true;
            this.tim_PainPic.Interval = 700;
            this.tim_PainPic.Tick += new System.EventHandler(this.tim_PainPic_Tick);
            // 
            // tim_WY
            // 
            this.tim_WY.Interval = 500;
            this.tim_WY.Tick += new System.EventHandler(this.tim_WY_Tick);
            // 
            // tim_getType
            // 
            this.tim_getType.Enabled = true;
            this.tim_getType.Interval = 500;
            this.tim_getType.Tick += new System.EventHandler(this.tim_getType_Tick);
            // 
            // PlaneDeformation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1100, 678);
            this.Controls.Add(this.tc_RealTimeSurveillance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PlaneDeformation";
            this.Text = "PlaneDeformation";
            this.tc_RealTimeSurveillance.ResumeLayout(false);
            this.page_airtight.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_level)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc_RealTimeSurveillance;
        private System.Windows.Forms.TabPage page_airtight;
        private System.Windows.Forms.Button btn_datadispose;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_dqyl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.DataGridView dgv_level;
        private System.Windows.Forms.GroupBox groupBox3;
        private Steema.TeeChart.TChart tChart_pm;
        private Steema.TeeChart.Styles.FastLine pm_Line;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_hl;
        private System.Windows.Forms.Button btn_qt;
        private System.Windows.Forms.Button btn_zore;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_stop1;
        private System.Windows.Forms.Button btn_level5;
        private System.Windows.Forms.Button btn_level4;
        private System.Windows.Forms.Button btn_level3;
        private System.Windows.Forms.Button btn_level2;
        private System.Windows.Forms.Button btn_level1;
        private System.Windows.Forms.TextBox txt_biaojilingdian;
        private System.Windows.Forms.GroupBox groupBox7;
        public System.Windows.Forms.ComboBox txt_reslevel;
        private System.Windows.Forms.TextBox txt_desc;
        private System.Windows.Forms.Timer tim_PainPic;
        private System.Windows.Forms.Timer tim_WY;
        private System.Windows.Forms.Button btn_tgstart;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Timer tim_getType;
    }
}