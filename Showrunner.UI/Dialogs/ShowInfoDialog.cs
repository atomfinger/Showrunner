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
    public partial class ShowInfoDialog : Form
    {
        public ShowInfoDialog(Show show)
        {
            InitializeComponent();

            this.Text = show.Title;

            if (show.Network != null)
                this.networkLabel.Text = show.Network.Name;

            if (show.Premiered.HasValue)
                this.premieredLabel.Text = show.Premiered.Value.ToShortDateString();

            if (show.Rating.HasValue)
                this.ratingLabel.Text = Math.Round(show.Rating.Value, 1).ToString();

            this.textBox.Text = show.Summary;
        }
    }
}
