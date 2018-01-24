﻿namespace Showrunner.UI.Controls
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
            this.nextWeekScheduleButton.Text = "Next week schedule report";
            this.nextWeekScheduleButton.UseVisualStyleBackColor = true;
            this.nextWeekScheduleButton.Click += new System.EventHandler(this.nextWeekScheduleButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Report:";
            // 
            // reportEdit
            // 
            this.reportEdit.Location = new System.Drawing.Point(311, 28);
            this.reportEdit.Name = "reportEdit";
            this.reportEdit.ReadOnly = true;
            this.reportEdit.Size = new System.Drawing.Size(777, 646);
            this.reportEdit.TabIndex = 4;
            this.reportEdit.Text = "";
            // 
            // csvCheckbutton
            // 
            this.csvCheckbutton.AutoSize = true;
            this.csvCheckbutton.Location = new System.Drawing.Point(356, 6);
            this.csvCheckbutton.Name = "csvCheckbutton";
            this.csvCheckbutton.Size = new System.Drawing.Size(84, 17);
            this.csvCheckbutton.TabIndex = 8;
            this.csvCheckbutton.Text = "CSV version";
            this.csvCheckbutton.UseVisualStyleBackColor = true;
            this.csvCheckbutton.CheckedChanged += new System.EventHandler(this.csvCheckbutton_CheckedChanged);
            // 
            // ReportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
    }
}