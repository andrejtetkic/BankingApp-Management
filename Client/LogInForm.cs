using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using System.ServiceModel;

namespace Client
{
    public partial class LogInForm : Form
    {
        IBank proxy;
        public LogInForm()
        {
            InitializeComponent();

            ChannelFactory<IBank> ch = new ChannelFactory<IBank>(new BasicHttpBinding(),
                new EndpointAddress("http://localhost:8000"));
            ch.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior());
            proxy = ch.CreateChannel();
            

            // Remove this
            this.Hide();
            MainPageForm mainPageForm = new MainPageForm();
            mainPageForm.Closed += (s, args) => this.Close();
            mainPageForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            User user = proxy.AuthenticateUser(username_tb.Text, password_tb.Text);
            MessageBox.Show("gesgdg");
            if (user != null)
            {

                var userSession = new UserSession
                {
                    UserId = user.JMBG,
                    Username = user.Email,
                    Role = user.Privilage
                };
                SessionManager.SetUserSession(userSession);


                this.Hide();
                MainPageForm mainPageForm = new MainPageForm();
                mainPageForm.Closed += (s, args) => this.Close();
                mainPageForm.Show();
            }
            else
            {
                username_tb.Text = string.Empty;
                password_tb.Text = string.Empty;
                wrongInput_label.Visible = true;
            }
        }

        private void onLogInInputChange(object sender, EventArgs e)
        {
            if (username_tb.Text.Length == 0 || password_tb.Text.Length == 0)
            {
                logIn_btn.Enabled = false;
            }
            else
            {
                logIn_btn.Enabled = true;
            }

            wrongInput_label.Visible = false;
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.Closed += (s, args) => this.Close();
            signUpForm.Show();

        }
    }
}
