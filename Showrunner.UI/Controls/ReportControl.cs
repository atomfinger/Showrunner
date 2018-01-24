﻿using System;
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
using Showrunner.UI.Dialogs;

namespace Showrunner.UI.Controls
{
    public partial class ReportControl : UserControl
    {
        Report currentReport = Report.Schedule;
        Genre[] selectedGenres;

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
                case Report.TopNetworks:
                    UpdateReport(ReportHelpers.TopNetworksReport(type));
                    break;
                case Report.ShowOverView:
                    UpdateReport(ReportHelpers.ShowReport(type));
                    break;
                case Report.Recommendations:
                    UpdateReport(ReportHelpers.RecommendedShowsReport(selectedGenres.ToArray(), type));
                    break;
                default:
                    throw new NotSupportedException();
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

        private void topNetworksButton_Click(object sender, EventArgs e)
        {
            currentReport = Report.TopNetworks;
            RefreshData();
        }

        private void showOverViewButton_Click(object sender, EventArgs e)
        {
            currentReport = Report.ShowOverView;
            RefreshData();
        }

        private void recommendationButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new SelectGenreDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                selectedGenres = dlg.SelectedGenres.ToArray();

                if (selectedGenres.Length <= 0)
                    return;
            }

            currentReport = Report.Recommendations;
            RefreshData();
        }

        private enum Report
        {
            Schedule,
            TopTen,
            TopNetworks,
            ShowOverView,
            Recommendations
        }
    }
}
