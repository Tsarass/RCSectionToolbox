namespace RCSectionToolbox
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.diameter2Box = new System.Windows.Forms.ComboBox();
            this.as2Box = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.diameter1Box = new System.Windows.Forms.ComboBox();
            this.as1Box = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.fyBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.betonBox = new System.Windows.Forms.ComboBox();
            this.coverBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.axialBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.widthBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.safetyCheck = new System.Windows.Forms.CheckBox();
            this.txtSummary = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            resources.ApplyResources(this.chart1, "chart1");
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Mphi";
            this.chart1.Series.Add(series1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.diameter2Box);
            this.groupBox1.Controls.Add(this.as2Box);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.diameter1Box);
            this.groupBox1.Controls.Add(this.as1Box);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.fyBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.betonBox);
            this.groupBox1.Controls.Add(this.coverBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.axialBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.widthBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.heightBox);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // diameter2Box
            // 
            this.diameter2Box.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.diameter2Box.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.diameter2Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.diameter2Box.FormattingEnabled = true;
            this.diameter2Box.Items.AddRange(new object[] {
            resources.GetString("diameter2Box.Items"),
            resources.GetString("diameter2Box.Items1"),
            resources.GetString("diameter2Box.Items2"),
            resources.GetString("diameter2Box.Items3"),
            resources.GetString("diameter2Box.Items4"),
            resources.GetString("diameter2Box.Items5")});
            resources.ApplyResources(this.diameter2Box, "diameter2Box");
            this.diameter2Box.Name = "diameter2Box";
            // 
            // as2Box
            // 
            resources.ApplyResources(this.as2Box, "as2Box");
            this.as2Box.Name = "as2Box";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // diameter1Box
            // 
            this.diameter1Box.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.diameter1Box.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.diameter1Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.diameter1Box.FormattingEnabled = true;
            this.diameter1Box.Items.AddRange(new object[] {
            resources.GetString("diameter1Box.Items"),
            resources.GetString("diameter1Box.Items1"),
            resources.GetString("diameter1Box.Items2"),
            resources.GetString("diameter1Box.Items3"),
            resources.GetString("diameter1Box.Items4"),
            resources.GetString("diameter1Box.Items5")});
            resources.ApplyResources(this.diameter1Box, "diameter1Box");
            this.diameter1Box.Name = "diameter1Box";
            // 
            // as1Box
            // 
            resources.ApplyResources(this.as1Box, "as1Box");
            this.as1Box.Name = "as1Box";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // fyBox
            // 
            resources.ApplyResources(this.fyBox, "fyBox");
            this.fyBox.Name = "fyBox";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // betonBox
            // 
            this.betonBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.betonBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.betonBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.betonBox.FormattingEnabled = true;
            this.betonBox.Items.AddRange(new object[] {
            resources.GetString("betonBox.Items"),
            resources.GetString("betonBox.Items1"),
            resources.GetString("betonBox.Items2"),
            resources.GetString("betonBox.Items3"),
            resources.GetString("betonBox.Items4"),
            resources.GetString("betonBox.Items5"),
            resources.GetString("betonBox.Items6"),
            resources.GetString("betonBox.Items7"),
            resources.GetString("betonBox.Items8")});
            resources.ApplyResources(this.betonBox, "betonBox");
            this.betonBox.Name = "betonBox";
            // 
            // coverBox
            // 
            resources.ApplyResources(this.coverBox, "coverBox");
            this.coverBox.Name = "coverBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // axialBox
            // 
            resources.ApplyResources(this.axialBox, "axialBox");
            this.axialBox.Name = "axialBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // widthBox
            // 
            resources.ApplyResources(this.widthBox, "widthBox");
            this.widthBox.Name = "widthBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // heightBox
            // 
            resources.ApplyResources(this.heightBox, "heightBox");
            this.heightBox.Name = "heightBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // safetyCheck
            // 
            resources.ApplyResources(this.safetyCheck, "safetyCheck");
            this.safetyCheck.Checked = true;
            this.safetyCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.safetyCheck.Name = "safetyCheck";
            this.safetyCheck.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.txtSummary.AcceptsReturn = true;
            resources.ApplyResources(this.txtSummary, "textBox1");
            this.txtSummary.Name = "textBox1";
            this.txtSummary.ReadOnly = true;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSummary);
            this.Controls.Add(this.safetyCheck);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox widthBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox heightBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox axialBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox coverBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox betonBox;
        private System.Windows.Forms.TextBox fyBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox as1Box;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox diameter2Box;
        private System.Windows.Forms.TextBox as2Box;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox diameter1Box;
        private System.Windows.Forms.CheckBox safetyCheck;
        private System.Windows.Forms.TextBox txtSummary;
    }
}

