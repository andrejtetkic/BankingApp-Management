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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            Utilities.SetTextBoxPlaceholder(f_name_tb, "First Name");
            Utilities.SetTextBoxPlaceholder(l_name_tb, "Last Name");
            Utilities.SetTextBoxPlaceholder(email_tb, "E-mail");
            Utilities.SetTextBoxPlaceholder(jmbg_tb, "JMBG");
            Utilities.SetTextBoxPlaceholder(password_tb, "Password");
            Utilities.SetTextBoxPlaceholder(password_again_tb, "Password Again");

        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogInForm loginForm = new LogInForm();
            loginForm.Closed += (s, args) => this.Close();
            loginForm.Show();
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            // Check Register


            // Back to LogIn
            this.Hide();
            LogInForm loginForm = new LogInForm();
            loginForm.Closed += (s, args) => this.Close();
            loginForm.Show();
        }
    }
}
