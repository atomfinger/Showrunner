using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Showrunner.UI
{
    public partial class MainForm : Form
    {
        ShowrunnerDbContext context;
        public MainForm()
        {
            InitializeComponent();

            context = DbContextFactory.GetDbContext();

            this.Disposed += MainForm_Disposed;
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            if (context != null)
                context.Dispose();
        }

        private void ImportDataButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Multiselect = false;
                dlg.Filter = "CSV files (.csv)|";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    ImportFileHelper.Import(dlg.FileName);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    RefreshData();
                }
            }
        }

        private void RefreshData()
        {
            dataGridView.DataSource = context.Shows.ToArray();
        }
    }
}
