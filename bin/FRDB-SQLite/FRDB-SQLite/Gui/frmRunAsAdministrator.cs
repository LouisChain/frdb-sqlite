using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace FRDB_SQLite
{
    public partial class frmRunAsAdministrator : DevExpress.XtraEditors.XtraForm
    {
        public frmRunAsAdministrator()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            Process.Start("UserGuide.txt");
        }
    }
}