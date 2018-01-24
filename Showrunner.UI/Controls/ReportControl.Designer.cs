namespace Showrunner.UI.Controls
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
            this.label2 = new System.Windows.Forms.Label();
            this.nextWeekScheduleButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.reportEdit = new System.Windows.Forms.RichTextBox();
            this.csvCheckbutton = new System.Windows.Forms.CheckBox();
            this.topTenButton = new System.Windows.Forms.Button();
            this.topNetworksButton = new System.Windows.Forms.Button();
            this.showOverViewButton = new System.Windows.Forms.Button();
            this.recommendationButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Reports:";
            // 
            // nextWeekScheduleButton
            // 
            this.nextWeekScheduleButton.Location = new System.Drawing.Point(8, 26);
            this.nextWeekScheduleButton.Name = "nextWeekScheduleButton";
            this.nextWeekScheduleButton.Size = new System.Drawing.Size(134, 23);
            this.nextWeekScheduleButton.TabIndex = 6;
            this.nextWeekScheduleButton.Text = "Next Week Schedule";
            this.nextWeekScheduleButton.UseVisualStyleBackColor = true;
            this.nextWeekScheduleButton.Click += new System.EventHandler(this.nextWeekScheduleButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Report:";
            // 
            // reportEdit
            // 
            this.reportEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportEdit.Location = new System.Drawing.Point(148, 26);
            this.reportEdit.Name = "reportEdit";
            this.reportEdit.ReadOnly = true;
            this.reportEdit.Size = new System.Drawing.Size(954, 646);
            this.reportEdit.TabIndex = 4;
            this.reportEdit.Text = "";
            // 
            // csvCheckbutton
            // 
            this.csvCheckbutton.AutoSize = true;
            this.csvCheckbutton.Checked = true;
            this.csvCheckbutton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.csvCheckbutton.Location = new System.Drawing.Point(193, 6);
            this.csvCheckbutton.Name = "csvCheckbutton";
            this.csvCheckbutton.Size = new System.Drawing.Size(84, 17);
            this.csvCheckbutton.TabIndex = 8;
            this.csvCheckbutton.Text = "CSV version";
            this.csvCheckbutton.UseVisualStyleBackColor = true;
            this.csvCheckbutton.CheckedChanged += new System.EventHandler(this.csvCheckbutton_CheckedChanged);
            // 
            // topTenButton
            // 
            this.topTenButton.Location = new System.Drawing.Point(8, 55);
            this.topTenButton.Name = "topTenButton";
            this.topTenButton.Size = new System.Drawing.Size(134, 23);
            this.topTenButton.TabIndex = 9;
            this.topTenButton.Text = "Top 10";
            this.topTenButton.UseVisualStyleBackColor = true;
            this.topTenButton.Click += new System.EventHandler(this.topTenButton_Click);
            // 
            // topNetworksButton
            // 
            this.topNetworksButton.Location = new System.Drawing.Point(8, 84);
            this.topNetworksButton.Name = "topNetworksButton";
            this.topNetworksButton.Size = new System.Drawing.Size(134, 23);
            this.topNetworksButton.TabIndex = 10;
            this.topNetworksButton.Text = "Top Networks";
            this.topNetworksButton.UseVisualStyleBackColor = true;
            this.topNetworksButton.Click += new System.EventHandler(this.topNetworksButton_Click);
            // 
            // showOverViewButton
            // 
            this.showOverViewButton.Location = new System.Drawing.Point(8, 113);
            this.showOverViewButton.Name = "showOverViewButton";
            this.showOverViewButton.Size = new System.Drawing.Size(134, 23);
            this.showOverViewButton.TabIndex = 11;
            this.showOverViewButton.Text = "Show Overview";
            this.showOverViewButton.UseVisualStyleBackColor = true;
            this.showOverViewButton.Click += new System.EventHandler(this.showOverViewButton_Click);
            // 
            // recommendationButton
            // 
            this.recommendationButton.Location = new System.Drawing.Point(8, 142);
            this.recommendationButton.Name = "recommendationButton";
            this.recommendationButton.Size = new System.Drawing.Size(134, 23);
            this.recommendationButton.TabIndex = 12;
            this.recommendationButton.Text = "Show Recommendations";
            this.recommendationButton.UseVisualStyleBackColor = true;
            this.recommendationButton.Click += new System.EventHandler(this.recommendationButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(8, 649);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(134, 23);
            this.exportButton.TabIndex = 13;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            // 
            // ReportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.recommendationButton);
            this.Controls.Add(this.showOverViewButton);
            this.Controls.Add(this.topNetworksButton);
            this.Controls.Add(this.topTenButton);
            this.Controls.Add(this.csvCheckbutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nextWeekScheduleButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportEdit);
            this.Name = "ReportControl";
            this.Size = new System.Drawing.Size(1105, 686);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button nextWeekScheduleButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox reportEdit;
        private System.Windows.Forms.CheckBox csvCheckbutton;
        private System.Windows.Forms.Button topTenButton;
        private System.Windows.Forms.Button topNetworksButton;
        private System.Windows.Forms.Button showOverViewButton;
        private System.Windows.Forms.Button recommendationButton;
        private System.Windows.Forms.Button exportButton;
    }
}
