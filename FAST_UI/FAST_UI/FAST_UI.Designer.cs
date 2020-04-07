namespace FAST_UI
{
    partial class FAST_UI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAST_UI));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.indicatorPanel = new System.Windows.Forms.Panel();
            this.FS_Butt = new System.Windows.Forms.Button();
            this.CAN_Butt = new System.Windows.Forms.Button();
            this.ConnectPanel = new System.Windows.Forms.Panel();
            this.SideMenuPanel = new System.Windows.Forms.Panel();
            this.DiagnosticsButt = new System.Windows.Forms.Button();
            this.LiveDataButt = new System.Windows.Forms.Button();
            this.TachButt = new System.Windows.Forms.Button();
            this.SideMenuLogoPanel = new System.Windows.Forms.Panel();
            this.LiveDataPanel = new System.Windows.Forms.Panel();
            this.ExportLogsButt = new System.Windows.Forms.Button();
            this.LiveStopLogButt = new System.Windows.Forms.Button();
            this.LiveStartLogButt = new System.Windows.Forms.Button();
            this.LiveDataChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ConnectPanel.SuspendLayout();
            this.SideMenuPanel.SuspendLayout();
            this.LiveDataPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LiveDataChart)).BeginInit();
            this.SuspendLayout();
            // 
            // indicatorPanel
            // 
            this.indicatorPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.indicatorPanel.BackColor = System.Drawing.Color.LimeGreen;
            this.indicatorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.indicatorPanel.Location = new System.Drawing.Point(200, 137);
            this.indicatorPanel.Name = "indicatorPanel";
            this.indicatorPanel.Size = new System.Drawing.Size(5, 100);
            this.indicatorPanel.TabIndex = 2;
            // 
            // FS_Butt
            // 
            this.FS_Butt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FS_Butt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.FS_Butt.FlatAppearance.BorderSize = 0;
            this.FS_Butt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FS_Butt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FS_Butt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.FS_Butt.Location = new System.Drawing.Point(168, 116);
            this.FS_Butt.Name = "FS_Butt";
            this.FS_Butt.Size = new System.Drawing.Size(138, 100);
            this.FS_Butt.TabIndex = 0;
            this.FS_Butt.Text = "Connect to FS";
            this.FS_Butt.UseVisualStyleBackColor = false;
            this.FS_Butt.Click += new System.EventHandler(this.FS_Butt_Click);
            // 
            // CAN_Butt
            // 
            this.CAN_Butt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CAN_Butt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.CAN_Butt.FlatAppearance.BorderSize = 0;
            this.CAN_Butt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CAN_Butt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CAN_Butt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.CAN_Butt.Location = new System.Drawing.Point(465, 116);
            this.CAN_Butt.Name = "CAN_Butt";
            this.CAN_Butt.Size = new System.Drawing.Size(138, 100);
            this.CAN_Butt.TabIndex = 1;
            this.CAN_Butt.Text = "Connect to CAN";
            this.CAN_Butt.UseVisualStyleBackColor = false;
            this.CAN_Butt.Click += new System.EventHandler(this.CAN_Butt_Click);
            // 
            // ConnectPanel
            // 
            this.ConnectPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ConnectPanel.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ConnectPanel.Controls.Add(this.CAN_Butt);
            this.ConnectPanel.Controls.Add(this.FS_Butt);
            this.ConnectPanel.Location = new System.Drawing.Point(28, 61);
            this.ConnectPanel.Name = "ConnectPanel";
            this.ConnectPanel.Size = new System.Drawing.Size(746, 357);
            this.ConnectPanel.TabIndex = 0;
            // 
            // SideMenuPanel
            // 
            this.SideMenuPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SideMenuPanel.Controls.Add(this.DiagnosticsButt);
            this.SideMenuPanel.Controls.Add(this.LiveDataButt);
            this.SideMenuPanel.Controls.Add(this.TachButt);
            this.SideMenuPanel.Controls.Add(this.SideMenuLogoPanel);
            this.SideMenuPanel.Location = new System.Drawing.Point(0, 0);
            this.SideMenuPanel.Name = "SideMenuPanel";
            this.SideMenuPanel.Size = new System.Drawing.Size(200, 450);
            this.SideMenuPanel.TabIndex = 3;
            // 
            // DiagnosticsButt
            // 
            this.DiagnosticsButt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DiagnosticsButt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.DiagnosticsButt.FlatAppearance.BorderSize = 0;
            this.DiagnosticsButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DiagnosticsButt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiagnosticsButt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.DiagnosticsButt.Location = new System.Drawing.Point(0, 347);
            this.DiagnosticsButt.Name = "DiagnosticsButt";
            this.DiagnosticsButt.Size = new System.Drawing.Size(196, 100);
            this.DiagnosticsButt.TabIndex = 3;
            this.DiagnosticsButt.Text = "Diagnostics";
            this.DiagnosticsButt.UseVisualStyleBackColor = false;
            this.DiagnosticsButt.Click += new System.EventHandler(this.DiagnosticsButt_Click);
            // 
            // LiveDataButt
            // 
            this.LiveDataButt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LiveDataButt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LiveDataButt.FlatAppearance.BorderSize = 0;
            this.LiveDataButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LiveDataButt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LiveDataButt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LiveDataButt.Location = new System.Drawing.Point(0, 243);
            this.LiveDataButt.Name = "LiveDataButt";
            this.LiveDataButt.Size = new System.Drawing.Size(196, 100);
            this.LiveDataButt.TabIndex = 2;
            this.LiveDataButt.Text = "LiveData";
            this.LiveDataButt.UseVisualStyleBackColor = false;
            this.LiveDataButt.Click += new System.EventHandler(this.LiveDataButt_Click);
            // 
            // TachButt
            // 
            this.TachButt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TachButt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.TachButt.FlatAppearance.BorderSize = 0;
            this.TachButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TachButt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TachButt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.TachButt.Location = new System.Drawing.Point(0, 137);
            this.TachButt.Name = "TachButt";
            this.TachButt.Size = new System.Drawing.Size(196, 100);
            this.TachButt.TabIndex = 1;
            this.TachButt.Text = "Tachometer";
            this.TachButt.UseVisualStyleBackColor = false;
            this.TachButt.Click += new System.EventHandler(this.TachButt_Click);
            // 
            // SideMenuLogoPanel
            // 
            this.SideMenuLogoPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SideMenuLogoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.SideMenuLogoPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SideMenuLogoPanel.BackgroundImage")));
            this.SideMenuLogoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SideMenuLogoPanel.Location = new System.Drawing.Point(0, 0);
            this.SideMenuLogoPanel.Name = "SideMenuLogoPanel";
            this.SideMenuLogoPanel.Size = new System.Drawing.Size(200, 120);
            this.SideMenuLogoPanel.TabIndex = 0;
            // 
            // LiveDataPanel
            // 
            this.LiveDataPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LiveDataPanel.Controls.Add(this.ExportLogsButt);
            this.LiveDataPanel.Controls.Add(this.LiveStopLogButt);
            this.LiveDataPanel.Controls.Add(this.LiveStartLogButt);
            this.LiveDataPanel.Controls.Add(this.LiveDataChart);
            this.LiveDataPanel.Location = new System.Drawing.Point(206, 14);
            this.LiveDataPanel.Name = "LiveDataPanel";
            this.LiveDataPanel.Size = new System.Drawing.Size(584, 424);
            this.LiveDataPanel.TabIndex = 4;
            // 
            // ExportLogsButt
            // 
            this.ExportLogsButt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ExportLogsButt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ExportLogsButt.FlatAppearance.BorderSize = 0;
            this.ExportLogsButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportLogsButt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportLogsButt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.ExportLogsButt.Location = new System.Drawing.Point(350, 355);
            this.ExportLogsButt.Name = "ExportLogsButt";
            this.ExportLogsButt.Size = new System.Drawing.Size(150, 60);
            this.ExportLogsButt.TabIndex = 6;
            this.ExportLogsButt.Text = "Export Logs";
            this.ExportLogsButt.UseVisualStyleBackColor = false;
            // 
            // LiveStopLogButt
            // 
            this.LiveStopLogButt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LiveStopLogButt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LiveStopLogButt.FlatAppearance.BorderSize = 0;
            this.LiveStopLogButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LiveStopLogButt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LiveStopLogButt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LiveStopLogButt.Location = new System.Drawing.Point(176, 355);
            this.LiveStopLogButt.Name = "LiveStopLogButt";
            this.LiveStopLogButt.Size = new System.Drawing.Size(150, 60);
            this.LiveStopLogButt.TabIndex = 5;
            this.LiveStopLogButt.Text = "Stop Logging";
            this.LiveStopLogButt.UseVisualStyleBackColor = false;
            this.LiveStopLogButt.Click += new System.EventHandler(this.LiveStopLogButt_Click);
            // 
            // LiveStartLogButt
            // 
            this.LiveStartLogButt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LiveStartLogButt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LiveStartLogButt.FlatAppearance.BorderSize = 0;
            this.LiveStartLogButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LiveStartLogButt.Font = new System.Drawing.Font("Agency FB", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LiveStartLogButt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LiveStartLogButt.Location = new System.Drawing.Point(5, 355);
            this.LiveStartLogButt.Name = "LiveStartLogButt";
            this.LiveStartLogButt.Size = new System.Drawing.Size(150, 60);
            this.LiveStartLogButt.TabIndex = 4;
            this.LiveStartLogButt.Text = "Start Logging";
            this.LiveStartLogButt.UseVisualStyleBackColor = false;
            this.LiveStartLogButt.Click += new System.EventHandler(this.LiveStartLogButt_Click);
            // 
            // LiveDataChart
            // 
            this.LiveDataChart.BackColor = System.Drawing.Color.DimGray;
            this.LiveDataChart.BackImageTransparentColor = System.Drawing.Color.DimGray;
            this.LiveDataChart.BackSecondaryColor = System.Drawing.Color.DimGray;
            chartArea1.Name = "ChartArea1";
            this.LiveDataChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.LiveDataChart.Legends.Add(legend1);
            this.LiveDataChart.Location = new System.Drawing.Point(5, 3);
            this.LiveDataChart.Name = "LiveDataChart";
            this.LiveDataChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Speed";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "FuelRate";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Legend = "Legend1";
            series3.Name = "FuelLevel";
            this.LiveDataChart.Series.Add(series1);
            this.LiveDataChart.Series.Add(series2);
            this.LiveDataChart.Series.Add(series3);
            this.LiveDataChart.Size = new System.Drawing.Size(576, 340);
            this.LiveDataChart.TabIndex = 0;
            // 
            // FAST_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LiveDataPanel);
            this.Controls.Add(this.SideMenuPanel);
            this.Controls.Add(this.indicatorPanel);
            this.Controls.Add(this.ConnectPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FAST_UI";
            this.Text = "FAST";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FAST_UI_FormClosing);
            this.ConnectPanel.ResumeLayout(false);
            this.SideMenuPanel.ResumeLayout(false);
            this.LiveDataPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LiveDataChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel indicatorPanel;
        private System.Windows.Forms.Button FS_Butt;
        private System.Windows.Forms.Button CAN_Butt;
        private System.Windows.Forms.Panel ConnectPanel;
        private System.Windows.Forms.Panel SideMenuPanel;
        private System.Windows.Forms.Button DiagnosticsButt;
        private System.Windows.Forms.Button LiveDataButt;
        private System.Windows.Forms.Button TachButt;
        private System.Windows.Forms.Panel SideMenuLogoPanel;
        private System.Windows.Forms.Panel LiveDataPanel;
        private System.Windows.Forms.Button ExportLogsButt;
        private System.Windows.Forms.Button LiveStopLogButt;
        private System.Windows.Forms.Button LiveStartLogButt;
        private System.Windows.Forms.DataVisualization.Charting.Chart LiveDataChart;
    }
}

