using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Helpers;
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
    public partial class SyncDialog : Form
    {
        ShowrunnerDbContext context;
        CancellationTokenSource source;

        public SyncDialog()
        {
            InitializeComponent();
            Disposed += SyncDialog_Disposed;
        }

        private void SyncDialog_Disposed(object sender, EventArgs e)
        {
            if (context != null)
                context.Dispose();

            if (source != null)
                source.Dispose();
        }

        private void startSyncButton_Click(object sender, EventArgs e)
        {
            startSyncButton.Enabled = false;

            context = DbContextFactory.GetDbContext();
            var progressReport = new ProgressReport();
            progressReport.ProgressReported += ProgressReport_ProgressReported;

            source = new CancellationTokenSource();

            Task.Run(() => ShowHelper.UpdateShows(context, context.Shows.ToArray(), new TvmazeApi(), source.Token, progressReport));
        }

        private void SyncComplete()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SyncComplete()));
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (source != null)
                source.Cancel();
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ProgressReport_ProgressReported(object sender, int progress)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ProgressReport_ProgressReported(sender, progress)));
                return;
            }

            this.progressBar.Value = progress;

            if (progress == 100)
                SyncComplete();
        }

        public class ProgressReport : IProgress<int>
        {
            public event EventHandler<int> ProgressReported;

            public void Report(int value)
            {
                var h = ProgressReported;
                if (h != null)
                    h.Invoke(this, value);
            }
        }
    }
}
