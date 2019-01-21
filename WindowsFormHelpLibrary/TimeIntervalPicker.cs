using System;
using System.Windows.Forms;

namespace WindowsFormHelpLibrary
{
    public partial class TimeIntervalPicker : Form
    {
        public DateTime From => dtpFrom.Value;
        public DateTime To => dtpTo.Value;
        public TimeIntervalPicker()
        {
            InitializeComponent();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void OnOk(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
