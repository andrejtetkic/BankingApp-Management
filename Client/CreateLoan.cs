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
    public partial class CreateLoan : Form
    {
        IBank proxy;
        List<Account> lstAccount;
        List<Bank> lstBank;

        public CreateLoan(IBank proxy, List<Account> lstAccount)
        {
            InitializeComponent();
            this.proxy = proxy;
            this.lstAccount = lstAccount;

            account_combo.Items.Clear();
            foreach (Account acc in lstAccount)
            {
                account_combo.Items.Add(acc.ProperAccountNumber);
            }

            if (lstAccount.Count > 0)
            {
                account_combo.SelectedIndex = 0;
            }

            lstBank = proxy.GetAllBanks();

            bank_combo.Items.Clear();
            foreach (Bank bank in lstBank)
            {
                bank_combo.Items.Add(bank.Name);
            }

            if (lstBank.Count > 0)
            {
                bank_combo.SelectedIndex = 0;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                Loan loan = new Loan();
                loan.Name = name_tb.Text;
                loan.BankID = lstBank[bank_combo.SelectedIndex].ID;
                loan.LenderJMBG = SessionManager.GetCurrentUser().UserId;
                loan.Amount = int.Parse(loan_amount.Text);
                loan.AccountNumber = Account.UnFormatID(account_combo.Text);

                proxy.CreateLoan(loan);

            } catch(FaultException<CException> ex)
            {
                error_label.Visible = true;
                error_label.Text = ex.Detail.Reason;
                return;
            }

            this.Close();
            MessageBox.Show("Your Loan has Automaticly been Approved. The fixed Interest is 5% of the amount");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loan_amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
