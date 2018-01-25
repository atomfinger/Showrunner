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
using Showrunner.Data.Models;

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
            if (context != null)
                context.Dispose();
            context = DbContextFactory.GetDbContext();
            dataGridView.DataSource = context.Shows.OrderBy(s => s.Title).ToArray();
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

        private void newShowButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new ShowSearchDialog())
                dlg.ShowDialog();
            RefreshData();
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                DeleteSelectedShow();
                e.Handled = true;
            }
        }

        private Show GetSelectedShow()
        {
            var rows = this.dataGridView.SelectedRows;
            if (rows == null || rows.Count == 0)
                return null;

            return rows[0].DataBoundItem as Show;
        }

        private void DeleteSelectedShow()
        {
            var show = GetSelectedShow();
            if (show == null)
                return;

            show.Genres.Clear();
            show.Episodes.Clear();
            context.Shows.Remove(show);
            context.SaveChanges();
            this.RefreshData();
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            var show = GetSelectedShow();
            if (show == null)
                return;

            using (var dlg = new ShowInfoDialog(show))
                dlg.ShowDialog();
        }
    }
}
