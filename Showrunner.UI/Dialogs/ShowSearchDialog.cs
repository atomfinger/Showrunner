using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Helpers;
using Showrunner.Data.Models;
using Showrunner.Data.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Showrunner.UI.Dialogs
{
    public partial class ShowSearchDialog : Form
    {
        public ShowSearchDialog()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            DoSearch(textBox.Text);
        }

        private void DoSearch(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return;

            var api = new TvmazeApi();
            var task = Task.Run(() => api.SearchShow(search));
            task.Wait();
            dataGridView.DataSource = task.Result;
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                DoSearch(textBox.Text);
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            importButton.Enabled = true;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            var rows = dataGridView.SelectedRows;
            if (rows.Count == 0)
                return;

            var show = rows[0].DataBoundItem as Show;
            if (show == null)
                return;

            using (var context = DbContextFactory.GetDbContext())
            {
                if (!context.Shows.Any(s => s.ApiId == show.ApiId))
                {
                    context.Entry(show).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                }
                else
                {
                    show = context.Shows.FirstOrDefault(s => s.ApiId == show.ApiId);
                }

                Task.Run(() => ShowHelper.UpdateShows(context, new[] { show }, new TvmazeApi(), CancellationToken.None)).Wait();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
