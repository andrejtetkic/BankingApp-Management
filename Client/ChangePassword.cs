using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ChangePassword : Form
    {
        IBank proxy;
        public ChangePassword(IBank proxy)
        {
            InitializeComponent();
            this.proxy = proxy;
            Utilities.SetTextBoxPlaceholder(curr_pass_tb, "Current Password");
            Utilities.SetTextBoxPlaceholder(new_pass_tb, "New Password");
            Utilities.SetTextBoxPlaceholder(new_pass_ag_tb, "Current Password Again");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(new_pass_tb.Text == "")
            {
                return;
            }

            if(new_pass_tb.Text != new_pass_ag_tb.Text)
            {
                return;
            }

            proxy.ChangeUserPassword(SessionManager.GetCurrentUser().UserId, new_pass_tb.Text, curr_pass_tb.Text);

            this.Close();
        }
    }
}
