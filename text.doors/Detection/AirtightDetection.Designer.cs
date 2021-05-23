namespace text.doors.Detection
{
    partial class AirtightDetection
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
            this.btn_datadispose = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_jlgzj = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_kkfc = new System.Windows.Forms.Label();
            this.lbl_sjmj = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_f_fc = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lbl_z_fc = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lbl_f_mj = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lbl_z_mj = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.tChart_qm = new Steema.TeeChart.TChart();
            this.chart_cms_qm_click = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.export_image_qm = new System.Windows.Forms.ToolStripMenuItem();
            this.qm_Line = new Steema.TeeChart.Styles.FastLine();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_dqyl = new System.Windows.Forms.Label();
            this.lbl_setYL = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.流量原始数据 = new System.Windows.Forms.TabPage();
            this.dgv_ll = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdb_gfzh = new System.Windows.Forms.RadioButton();
            this.rdb_zdstl = new System.Windows.Forms.RadioButton();
            this.btn_losestart = new System.Windows.Forms.Button();
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_juststart = new System.Windows.Forms.Button();
            this.btn_loseready = new System.Windows.Forms.Button();
            this.btn_justready = new System.Windows.Forms.Button();
            this.rdb_fjstl = new System.Windows.Forms.RadioButton();
            this.btn_stop = new System.Windows.Forms.Button();
            this.tim_qm = new System.Windows.Forms.Timer(this.components);
            this.tim_getType = new System.Windows.Forms.Timer(this.components);
            this.tim_Top10 = new System.Windows.Forms.Timer(this.components);
            this.gv_list = new System.Windows.Forms.Timer(this.components);
            this.tim_PainPic = new System.Windows.Forms.Timer(this.components);
            this.tc_RealTimeSurveillance.SuspendLayout();
            this.page_airtight.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.chart_cms_qm_click.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.流量原始数据.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ll)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc_RealTimeSurveillance
            // 
            this.tc_RealTimeSurveillance.Controls.Add(this.page_airtight);
            this.tc_RealTimeSurveillance.ItemSize = new System.Drawing.Size(120, 26);
            this.tc_RealTimeSurveillance.Location = new System.Drawing.Point(0, 0);
            this.tc_RealTimeSurveillance.Name = "tc_RealTimeSurveillance";
            this.tc_RealTimeSurveillance.SelectedIndex = 0;
            this.tc_RealTimeSurveillance.Size = new System.Drawing.Size(1099, 675);
            this.tc_RealTimeSurveillance.TabIndex = 0;
            // 
            // page_airtight
            // 
            this.page_airtight.BackColor = System.Drawing.Color.White;
            this.page_airtight.Controls.Add(this.btn_datadispose);
            this.page_airtight.Controls.Add(this.groupBox4);
            this.page_airtight.Controls.Add(this.groupBox2);
            this.page_airtight.Controls.Add(this.groupBox9);
            this.page_airtight.Controls.Add(this.groupBox1);
            this.page_airtight.Controls.Add(this.tabControl1);
            this.page_airtight.Controls.Add(this.groupBox3);
            this.page_airtight.Location = new System.Drawing.Point(4, 30);
            this.page_airtight.Name = "page_airtight";
            this.page_airtight.Padding = new System.Windows.Forms.Padding(3);
            this.page_airtight.Size = new System.Drawing.Size(1091, 641);
            this.page_airtight.TabIndex = 0;
            this.page_airtight.Text = "气密监控";
            // 
            // btn_datadispose
            // 
            this.btn_datadispose.Location = new System.Drawing.Point(755, 506);
            this.btn_datadispose.Name = "btn_datadispose";
            this.btn_datadispose.Size = new System.Drawing.Size(326, 28);
            this.btn_datadispose.TabIndex = 14;
            this.btn_datadispose.Text = "数据处理";
            this.btn_datadispose.UseVisualStyleBackColor = true;
            this.btn_datadispose.Click += new System.EventHandler(this.btn_datadispose_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.lbl_jlgzj);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.lbl_kkfc);
            this.groupBox4.Controls.Add(this.lbl_sjmj);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(755, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(326, 63);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "相关数据";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(138, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 8;
            this.label9.Text = "米";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(8, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 7;
            this.label10.Text = "集流管直径：";
            // 
            // lbl_jlgzj
            // 
            this.lbl_jlgzj.AutoSize = true;
            this.lbl_jlgzj.Location = new System.Drawing.Point(86, 38);
            this.lbl_jlgzj.Name = "lbl_jlgzj";
            this.lbl_jlgzj.Size = new System.Drawing.Size(12, 12);
            this.lbl_jlgzj.TabIndex = 6;
            this.lbl_jlgzj.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(287, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "米";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(138, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "平米";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(176, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "可开逢长：";
            // 
            // lbl_kkfc
            // 
            this.lbl_kkfc.AutoSize = true;
            this.lbl_kkfc.Location = new System.Drawing.Point(240, 17);
            this.lbl_kkfc.Name = "lbl_kkfc";
            this.lbl_kkfc.Size = new System.Drawing.Size(12, 12);
            this.lbl_kkfc.TabIndex = 2;
            this.lbl_kkfc.Text = "0";
            // 
            // lbl_sjmj
            // 
            this.lbl_sjmj.AutoSize = true;
            this.lbl_sjmj.Location = new System.Drawing.Point(86, 17);
            this.lbl_sjmj.Name = "lbl_sjmj";
            this.lbl_sjmj.Size = new System.Drawing.Size(12, 12);
            this.lbl_sjmj.TabIndex = 1;
            this.lbl_sjmj.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "试件面积：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_f_fc);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.lbl_z_fc);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.lbl_f_mj);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.lbl_z_mj);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(754, 403);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 99);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "分级指标";
            // 
            // lbl_f_fc
            // 
            this.lbl_f_fc.AutoSize = true;
            this.lbl_f_fc.Location = new System.Drawing.Point(235, 77);
            this.lbl_f_fc.Name = "lbl_f_fc";
            this.lbl_f_fc.Size = new System.Drawing.Size(33, 12);
            this.lbl_f_fc.TabIndex = 9;
            this.lbl_f_fc.Text = "----";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(56, 77);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(173, 12);
            this.label19.TabIndex = 8;
            this.label19.Text = "幕墙可开负压单位逢长渗透量：";
            // 
            // lbl_z_fc
            // 
            this.lbl_z_fc.AutoSize = true;
            this.lbl_z_fc.Location = new System.Drawing.Point(235, 57);
            this.lbl_z_fc.Name = "lbl_z_fc";
            this.lbl_z_fc.Size = new System.Drawing.Size(33, 12);
            this.lbl_z_fc.TabIndex = 7;
            this.lbl_z_fc.Text = "----";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(56, 57);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(173, 12);
            this.label17.TabIndex = 6;
            this.label17.Text = "幕墙可开正压单位逢长渗透量：";
            // 
            // lbl_f_mj
            // 
            this.lbl_f_mj.AutoSize = true;
            this.lbl_f_mj.Location = new System.Drawing.Point(235, 37);
            this.lbl_f_mj.Name = "lbl_f_mj";
            this.lbl_f_mj.Size = new System.Drawing.Size(33, 12);
            this.lbl_f_mj.TabIndex = 5;
            this.lbl_f_mj.Text = "----";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(56, 37);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(173, 12);
            this.label15.TabIndex = 4;
            this.label15.Text = "幕墙整体负压单位面积渗透量：";
            // 
            // lbl_z_mj
            // 
            this.lbl_z_mj.AutoSize = true;
            this.lbl_z_mj.Location = new System.Drawing.Point(235, 16);
            this.lbl_z_mj.Name = "lbl_z_mj";
            this.lbl_z_mj.Size = new System.Drawing.Size(33, 12);
            this.lbl_z_mj.TabIndex = 3;
            this.lbl_z_mj.Text = "----";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(56, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(173, 12);
            this.label13.TabIndex = 2;
            this.label13.Text = "幕墙整体正压单位面积渗透量：";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.tChart_qm);
            this.groupBox9.Location = new System.Drawing.Point(8, 87);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(745, 550);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "检测";
            // 
            // tChart_qm
            // 
            // 
            // 
            // 
            this.tChart_qm.Aspect.ZOffset = 0D;
            this.tChart_qm.BackColor = System.Drawing.Color.White;
            this.tChart_qm.ContextMenuStrip = this.chart_cms_qm_click;
            this.tChart_qm.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // 
            // 
            this.tChart_qm.Header.Lines = new string[] {
        "气密检测"};
            this.tChart_qm.Location = new System.Drawing.Point(6, 20);
            this.tChart_qm.Name = "tChart_qm";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_qm.Panel.Brush.Color = System.Drawing.Color.White;
            // 
            // 
            // 
            this.tChart_qm.Panel.Brush.Gradient.Visible = false;
            this.tChart_qm.Panel.MarginLeft = 0D;
            this.tChart_qm.Panel.MarginRight = 2D;
            this.tChart_qm.Panel.MarginTop = 0D;
            this.tChart_qm.Series.Add(this.qm_Line);
            this.tChart_qm.Size = new System.Drawing.Size(724, 480);
            this.tChart_qm.TabIndex = 18;
            this.tChart_qm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseDown);
            // 
            // chart_cms_qm_click
            // 
            this.chart_cms_qm_click.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.chart_cms_qm_click.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.export_image_qm});
            this.chart_cms_qm_click.Name = "contextMenuStrip1";
            this.chart_cms_qm_click.Size = new System.Drawing.Size(125, 26);
            // 
            // export_image_qm
            // 
            this.export_image_qm.Name = "export_image_qm";
            this.export_image_qm.Size = new System.Drawing.Size(124, 22);
            this.export_image_qm.Text = "导出图片";
            this.export_image_qm.Click += new System.EventHandler(this.export_image_qm_Click);
            // 
            // qm_Line
            // 
            this.qm_Line.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.qm_Line.ColorEach = false;
            this.qm_Line.Cursor = System.Windows.Forms.Cursors.Cross;
            // 
            // 
            // 
            this.qm_Line.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.qm_Line.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None;
            this.qm_Line.Marks.Callout.ArrowHeadSize = 8;
            // 
            // 
            // 
            this.qm_Line.Marks.Callout.Brush.Color = System.Drawing.Color.Black;
            this.qm_Line.Marks.Callout.Distance = 0;
            this.qm_Line.Marks.Callout.Draw3D = false;
            this.qm_Line.Marks.Callout.Length = 10;
            this.qm_Line.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            this.qm_Line.ShowInLegend = false;
            this.qm_Line.Title = "fastLine1";
            this.qm_Line.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.qm_Line.XValues.DataMember = "X";
            this.qm_Line.XValues.DateTime = true;
            this.qm_Line.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.qm_Line.YValues.DataMember = "Y";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_dqyl);
            this.groupBox1.Controls.Add(this.lbl_setYL);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lbl_title);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(744, 59);
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
            // lbl_setYL
            // 
            this.lbl_setYL.AutoSize = true;
            this.lbl_setYL.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_setYL.Location = new System.Drawing.Point(672, 1);
            this.lbl_setYL.Name = "lbl_setYL";
            this.lbl_setYL.Size = new System.Drawing.Size(17, 16);
            this.lbl_setYL.TabIndex = 23;
            this.lbl_setYL.Text = "0";
            this.lbl_setYL.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(497, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "当前压力（帕）：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(497, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "设定压力（帕）：";
            this.label6.Visible = false;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_title.Location = new System.Drawing.Point(77, 26);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(214, 16);
            this.lbl_title.TabIndex = 20;
            this.lbl_title.Text = "幕墙气密性能检测  第0号 ";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.流量原始数据);
            this.tabControl1.Location = new System.Drawing.Point(754, 71);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(331, 329);
            this.tabControl1.TabIndex = 16;
            // 
            // 流量原始数据
            // 
            this.流量原始数据.Controls.Add(this.dgv_ll);
            this.流量原始数据.Location = new System.Drawing.Point(4, 22);
            this.流量原始数据.Name = "流量原始数据";
            this.流量原始数据.Padding = new System.Windows.Forms.Padding(3);
            this.流量原始数据.Size = new System.Drawing.Size(323, 303);
            this.流量原始数据.TabIndex = 1;
            this.流量原始数据.Text = "风速：米/秒";
            this.流量原始数据.UseVisualStyleBackColor = true;
            // 
            // dgv_ll
            // 
            this.dgv_ll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ll.Location = new System.Drawing.Point(6, 6);
            this.dgv_ll.Name = "dgv_ll";
            this.dgv_ll.RowHeadersWidth = 62;
            this.dgv_ll.RowTemplate.Height = 23;
            this.dgv_ll.Size = new System.Drawing.Size(310, 290);
            this.dgv_ll.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdb_gfzh);
            this.groupBox3.Controls.Add(this.rdb_zdstl);
            this.groupBox3.Controls.Add(this.btn_losestart);
            this.groupBox3.Controls.Add(this.btn_exit);
            this.groupBox3.Controls.Add(this.btn_juststart);
            this.groupBox3.Controls.Add(this.btn_loseready);
            this.groupBox3.Controls.Add(this.btn_justready);
            this.groupBox3.Controls.Add(this.rdb_fjstl);
            this.groupBox3.Controls.Add(this.btn_stop);
            this.groupBox3.Location = new System.Drawing.Point(755, 539);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(326, 98);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "国标检测";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // rdb_gfzh
            // 
            this.rdb_gfzh.AutoSize = true;
            this.rdb_gfzh.Location = new System.Drawing.Point(127, 14);
            this.rdb_gfzh.Name = "rdb_gfzh";
            this.rdb_gfzh.Size = new System.Drawing.Size(71, 16);
            this.rdb_gfzh.TabIndex = 20;
            this.rdb_gfzh.Tag = "stl";
            this.rdb_gfzh.Text = "固附之和";
            this.rdb_gfzh.UseVisualStyleBackColor = true;
            // 
            // rdb_zdstl
            // 
            this.rdb_zdstl.AutoSize = true;
            this.rdb_zdstl.Location = new System.Drawing.Point(205, 13);
            this.rdb_zdstl.Name = "rdb_zdstl";
            this.rdb_zdstl.Size = new System.Drawing.Size(71, 16);
            this.rdb_zdstl.TabIndex = 0;
            this.rdb_zdstl.Tag = "stl";
            this.rdb_zdstl.Text = "总的渗透";
            this.rdb_zdstl.UseVisualStyleBackColor = true;
            // 
            // btn_losestart
            // 
            this.btn_losestart.Location = new System.Drawing.Point(135, 64);
            this.btn_losestart.Name = "btn_losestart";
            this.btn_losestart.Size = new System.Drawing.Size(62, 28);
            this.btn_losestart.TabIndex = 4;
            this.btn_losestart.Text = "负压开始";
            this.btn_losestart.UseVisualStyleBackColor = true;
            this.btn_losestart.Click += new System.EventHandler(this.btn_losestart_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(216, 64);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(67, 28);
            this.btn_exit.TabIndex = 19;
            this.btn_exit.Text = "退出";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btn_juststart
            // 
            this.btn_juststart.Location = new System.Drawing.Point(51, 64);
            this.btn_juststart.Name = "btn_juststart";
            this.btn_juststart.Size = new System.Drawing.Size(66, 28);
            this.btn_juststart.TabIndex = 4;
            this.btn_juststart.Text = "正压开始";
            this.btn_juststart.UseVisualStyleBackColor = true;
            this.btn_juststart.Click += new System.EventHandler(this.btn_juststart_Click);
            // 
            // btn_loseready
            // 
            this.btn_loseready.Location = new System.Drawing.Point(135, 33);
            this.btn_loseready.Name = "btn_loseready";
            this.btn_loseready.Size = new System.Drawing.Size(62, 28);
            this.btn_loseready.TabIndex = 4;
            this.btn_loseready.Text = "负压预备";
            this.btn_loseready.UseVisualStyleBackColor = true;
            this.btn_loseready.Click += new System.EventHandler(this.btn_loseready_Click);
            // 
            // btn_justready
            // 
            this.btn_justready.Location = new System.Drawing.Point(51, 34);
            this.btn_justready.Name = "btn_justready";
            this.btn_justready.Size = new System.Drawing.Size(66, 28);
            this.btn_justready.TabIndex = 4;
            this.btn_justready.Text = "正压预备";
            this.btn_justready.UseVisualStyleBackColor = true;
            this.btn_justready.Click += new System.EventHandler(this.btn_justready_Click);
            // 
            // rdb_fjstl
            // 
            this.rdb_fjstl.AutoSize = true;
            this.rdb_fjstl.Checked = true;
            this.rdb_fjstl.Location = new System.Drawing.Point(51, 14);
            this.rdb_fjstl.Name = "rdb_fjstl";
            this.rdb_fjstl.Size = new System.Drawing.Size(71, 16);
            this.rdb_fjstl.TabIndex = 0;
            this.rdb_fjstl.TabStop = true;
            this.rdb_fjstl.Tag = "stl";
            this.rdb_fjstl.Text = "附加渗透";
            this.rdb_fjstl.UseVisualStyleBackColor = true;
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(216, 34);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(67, 28);
            this.btn_stop.TabIndex = 12;
            this.btn_stop.Text = "停止加压";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // tim_qm
            // 
            this.tim_qm.Enabled = true;
            this.tim_qm.Interval = 2000;
            this.tim_qm.Tick += new System.EventHandler(this.tim_qm_Tick);
            // 
            // tim_getType
            // 
            this.tim_getType.Enabled = true;
            this.tim_getType.Interval = 500;
            this.tim_getType.Tick += new System.EventHandler(this.tim_getType_Tick);
            // 
            // tim_Top10
            // 
            this.tim_Top10.Interval = 1000;
            this.tim_Top10.Tick += new System.EventHandler(this.tim_Top10_Tick);
            // 
            // gv_list
            // 
            this.gv_list.Interval = 1000;
            this.gv_list.Tick += new System.EventHandler(this.gv_list_Tick);
            // 
            // tim_PainPic
            // 
            this.tim_PainPic.Enabled = true;
            this.tim_PainPic.Interval = 1000;
            this.tim_PainPic.Tick += new System.EventHandler(this.tim_PainPic_Tick);
            // 
            // AirtightDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 678);
            this.Controls.Add(this.tc_RealTimeSurveillance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AirtightDetection";
            this.Text = "RealTimeSurveillance";
            this.tc_RealTimeSurveillance.ResumeLayout(false);
            this.page_airtight.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.chart_cms_qm_click.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.流量原始数据.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ll)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc_RealTimeSurveillance;
        private System.Windows.Forms.TabPage page_airtight;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdb_zdstl;
        private System.Windows.Forms.Button btn_losestart;
        private System.Windows.Forms.Button btn_juststart;
        private System.Windows.Forms.Button btn_loseready;
        private System.Windows.Forms.Button btn_justready;
        private System.Windows.Forms.RadioButton rdb_fjstl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_datadispose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 流量原始数据;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_title;
        private Steema.TeeChart.TChart tChart_qm;
        private System.Windows.Forms.DataGridView dgv_ll;
        private Steema.TeeChart.Styles.FastLine qm_Line;
        private System.Windows.Forms.ContextMenuStrip chart_cms_qm_click;
        private System.Windows.Forms.ToolStripMenuItem export_image_qm;
        private System.Windows.Forms.Label lbl_dqyl;
        private System.Windows.Forms.Timer tim_Top10;
        private System.Windows.Forms.Timer gv_list;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Timer tim_qm;
        private System.Windows.Forms.Timer tim_getType;
        private System.Windows.Forms.Timer tim_PainPic;
        private System.Windows.Forms.Label lbl_setYL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rdb_gfzh;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_jlgzj;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_kkfc;
        private System.Windows.Forms.Label lbl_sjmj;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_f_fc;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbl_z_fc;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lbl_f_mj;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lbl_z_mj;
        private System.Windows.Forms.Label label13;
    }
}