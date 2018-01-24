using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Showrunner.UI.Dialogs
{
    public partial class SelectGenreDialog : Form
    {
        ShowrunnerDbContext context;
        public SelectGenreDialog()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;
            Disposed += SelectGenreDialog_Disposed;

            context = DbContextFactory.GetDbContext();
            dataGridView.DataSource = context.Genres.ToList();

            SelectedGenres = new List<Data.Models.Genre>();
        }

        public List<Genre> SelectedGenres { get; private set; }

        private void SelectGenreDialog_Disposed(object sender, EventArgs e)
        {
            if (context != null)
                context.Dispose();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var genre = row.DataBoundItem as Genre;
                if (genre == null)
                    continue;

                if (row.Cells[1].Value != null && (bool)row.Cells[1].Value)
                    SelectedGenres.Add(genre);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
