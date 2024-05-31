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
        ChannelFactory<IBank> ch;

        public LogInForm()
        {
            InitializeComponent();
            Utilities.SetTextBoxPlaceholder(username_tb, "E-mail");
            Utilities.SetTextBoxPlaceholder(password_tb, "Password");


            ch = new ChannelFactory<IBank>(new BasicHttpBinding(),
                new EndpointAddress("http://localhost:8000"));
            ch.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior());
            proxy = ch.CreateChannel();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            User user = proxy.AuthenticateUser(username_tb.Text, password_tb.Text);

            if (user != null)
            {

                var userSession = new UserSession
                {
                    UserId = user.JMBG,
                    Username = user.Email,
                    Role = (PrivilagesEnum)user.Privilage
                };
                SessionManager.SetUserSession(userSession);

                if (SessionManager.IsAdmin())
                {
                    
                    ch.Close();
                    this.Hide();
                    MainPageForm mainPageForm = new MainPageForm();
                    mainPageForm.Closed += (s, args) => this.Close();
                    mainPageForm.Show();
                }
                else
                {
                    ch.Close();
                    this.Hide();
                    MainPageForm mainPageForm = new MainPageForm();
                    mainPageForm.Closed += (s, args) => this.Close();
                    mainPageForm.Show();
                }
                
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
            if (username_tb.Text == "E-mail" && password_tb.Text == "Password")
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
            Register registerForm = new Register(proxy);
            registerForm.Closed += (s, args) => this.Close();
            registerForm.Show();

        }
    }
}
