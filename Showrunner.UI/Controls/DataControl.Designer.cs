namespace Showrunner.UI.Controls
{
    partial class DataControl
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importDataButton = new System.Windows.Forms.Button();
            this.syncDbButton = new System.Windows.Forms.Button();
            this.newShowButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title});
            this.dataGridView.Location = new System.Drawing.Point(172, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(782, 589);
            this.dataGridView.TabIndex = 3;
            this.dataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // Title
            // 
            this.Title.DataPropertyName = "Title";
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // importDataButton
            // 
            this.importDataButton.Location = new System.Drawing.Point(3, 3);
            this.importDataButton.Name = "importDataButton";
            this.importDataButton.Size = new System.Drawing.Size(163, 38);
            this.importDataButton.TabIndex = 2;
            this.importDataButton.Text = "Import data from file";
            this.importDataButton.UseVisualStyleBackColor = true;
            this.importDataButton.Click += new System.EventHandler(this.ImportDataButton_Click);
            // 
            // syncDbButton
            // 
            this.syncDbButton.Location = new System.Drawing.Point(3, 47);
            this.syncDbButton.Name = "syncDbButton";
            this.syncDbButton.Size = new System.Drawing.Size(163, 38);
            this.syncDbButton.TabIndex = 4;
            this.syncDbButton.Text = "Sync local Db with Tvmaze";
            this.syncDbButton.UseVisualStyleBackColor = true;
            this.syncDbButton.Click += new System.EventHandler(this.syncDbButton_Click);
            // 
            // newShowButton
            // 
            this.newShowButton.Location = new System.Drawing.Point(3, 91);
            this.newShowButton.Name = "newShowButton";
            this.newShowButton.Size = new System.Drawing.Size(163, 38);
            this.newShowButton.TabIndex = 5;
            this.newShowButton.Text = "Add new show";
            this.newShowButton.UseVisualStyleBackColor = true;
            this.newShowButton.Click += new System.EventHandler(this.newShowButton_Click);
            // 
            // DataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newShowButton);
            this.Controls.Add(this.syncDbButton);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.importDataButton);
            this.Name = "DataControl";
            this.Size = new System.Drawing.Size(957, 595);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.Button importDataButton;
        private System.Windows.Forms.Button syncDbButton;
        private System.Windows.Forms.Button newShowButton;
    }
}
