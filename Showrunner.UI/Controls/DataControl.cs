using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Helpers;
using Showrunner.UI.Dialogs;

namespace Showrunner.UI.Controls
{
    public partial class DataControl : UserControl
    {
        ShowrunnerDbContext context;

        public DataControl()
        {
            InitializeComponent();

            Disposed += DataControl_Disposed;

            this.Load += DataControl_Load;
        }

        private void DataControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            context = DbContextFactory.GetDbContext();
            RefreshData();
        }

        private void DataControl_Disposed(object sender, EventArgs e)
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

        private void syncDbButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new SyncDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
            }

            RefreshData();
        }
    }
}
