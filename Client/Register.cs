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
    public partial class Register : Form
    {
        IBank proxy;
        public Register(IBank proxy)
        {
            InitializeComponent();
            Utilities.SetTextBoxPlaceholder(f_name_tb, "First Name");
            Utilities.SetTextBoxPlaceholder(l_name_tb, "Last Name");
            Utilities.SetTextBoxPlaceholder(email_tb, "E-mail");
            Utilities.SetTextBoxPlaceholder(jmbg_tb, "JMBG");
            Utilities.SetTextBoxPlaceholder(password_tb, "Password");
            Utilities.SetTextBoxPlaceholder(password_again_tb, "Password Again");

            this.proxy = proxy;

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
            if(f_name_tb.Text == "First Name" || l_name_tb.Text == "Last Name" || email_tb.Text == "E-mail" || jmbg_tb.Text == "JMBG" || password_tb.Text == "Password" || password_again_tb.Text == "Password Again")
            {
                register_lb_error.Text = "You must fill out everything";
                return;
            }

            if(password_tb.Text != password_again_tb.Text)
            {
                register_lb_error.Text = "Passwords don't match";
                return;
            }

            if(jmbg_tb.Text.Length != 13)
            {
                register_lb_error.Text = "JMBG not valid";
                return;
            }

            User user = new User();
            user.JMBG = jmbg_tb.Text;
            user.Ime = f_name_tb.Text;
            user.Prezime = l_name_tb.Text;
            user.Email = email_tb.Text;
            user.PasswordHash = password_tb.Text;

            proxy.CreateUser(user);

            // Back to LogIn
            this.Hide();
            LogInForm loginForm = new LogInForm();
            loginForm.Closed += (s, args) => this.Close();
            loginForm.Show();
        }
    }
}
