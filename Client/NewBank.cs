using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class NewBank : Form
    {
        IBank proxy;
        public NewBank(IBank proxy)
        {
            InitializeComponent();
            this.proxy = proxy;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            error_label.Visible = true;

            if(bank_name.Text == "" || city_tb.Text == "" || main_branch_tb.Text == "" || jmbg_tb.Text == "" || email_tb.Text == "" || password_tb.Text == "" || conmf_password_tb.Text == "")
            {
                error_label.Text = "All fields must be Filled";
                return;
            } 

            if(jmbg_tb.Text.Length != 13)
            {
                error_label.Text = "Invalid JMBG";
                return;
            }
            if(password_tb.Text != conmf_password_tb.Text)
            {
                error_label.Text = "Passwords don't match";
                return;
            }

            User new_user = new User();
            if (proxy.GetUser(jmbg_tb.Text).JMBG != "")
            {
                try
                {
                    new_user.Ime = bank_name.Text;
                    new_user.Prezime = "";
                    new_user.Email = email_tb.Text;
                    proxy.UpdateUser(jmbg_tb.Text, new_user);
                } catch(FaultException<CException> ex)
                {
                    error_label.Text = ex.Detail.Reason;
                    return;
                }
                MessageBox.Show("Password will remain what it earlier was.\nYou can change it in User Settings");
            }
            else
            {
                try
                {
                    new_user.JMBG = jmbg_tb.Text;
                    new_user.Ime = bank_name.Text;
                    new_user.Prezime = "";
                    new_user.Email = email_tb.Text;
                    new_user.PasswordHash = password_tb.Text;
                    proxy.CreateUser(new_user);
                } catch (FaultException<CException> ex)
                {
                    error_label.Text = ex.Detail.Reason;
                    return;
                }
            }

            // I so don't like this
            // If one brakes, whole system is a mess
            try
            { 
                Bank bank = new Bank();
                bank.Name = bank_name.Text;
                bank.City = city_tb.Text;
                proxy.CreateBank(bank);

                int bankId = proxy.GetAllBankNamesWithIDs().FirstOrDefault(x => x.Value == bank.Name).Key;
                Branch branch = new Branch();
                branch.BankID = bankId;
                branch.Name = main_branch_tb.Text;
                branch.Address = bank.City;
                proxy.CreateBranch(branch);

                int branchId = proxy.GetAllBranchNamesWithIDs().FirstOrDefault(x => x.Value == main_branch_tb.Text).Key;
                bank.MainBranch = branchId;
                proxy.UpdateBank(bankId, bank);

                Account account = new Account();
                account.AccountNumber = $"{bankId.ToString("D4")}b";
                account.BankId = bankId;
                account.BranchId = branchId;
                account.LenderJMBG = new_user.JMBG;
                proxy.CreateAccount(account);

            } catch(FaultException<CException> ex)
            {
                error_label.Text = ex.Detail.Reason;
                return;
            }

            proxy.SetUserAdmin(new_user.JMBG);


            this.Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
