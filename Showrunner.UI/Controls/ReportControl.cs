using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Showrunner.Data.Models;
using Showrunner.Data.Helpers;
using Showrunner.Data.Utils;

namespace Showrunner.UI.Controls
{
    public partial class ReportControl : UserControl
    {
        Report currentReport = Report.Schedule;

        public ReportControl()
        {
            InitializeComponent();
        }

        public void RefreshData()
        {
            Data.Enums.ReportType type = Data.Enums.ReportType.Text;
            if (csvCheckbutton.Checked)
                type = Data.Enums.ReportType.CSV;

            switch (currentReport)
            {
                case Report.Schedule:
                    Task.Run(() => ReportHelpers.GetNextWeekScheduleReport(type, new TvmazeApi())).ContinueWith((task) => UpdateReport(task.Result));
                    break;
                case Report.TopTen:
                    UpdateReport(ReportHelpers.TopTenShowsReport(type));
                    break;

            }
        }

        private void UpdateReport(string report)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateReport(report)));
                return;
            }

            this.reportEdit.Text = report;
        }

        private void csvCheckbutton_CheckedChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void topTenButton_Click(object sender, EventArgs e)
        {
            currentReport = Report.TopTen;
            RefreshData();
        }

        private void nextWeekScheduleButton_Click(object sender, EventArgs e)
        {
            currentReport = Report.Schedule;
            RefreshData();
        }

        private enum Report
        {
            Schedule,
            TopTen,
        }

    }
}
