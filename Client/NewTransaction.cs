using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class NewTransaction : Form
    {
        IBank proxy;
        List<Account> lstAccount;
        public NewTransaction(IBank proxy, List<Account> lstAccount)
        {
            InitializeComponent();
            this.proxy = proxy;
            this.lstAccount = lstAccount;

            from_accoutn_combo.Items.Clear();
            to_account_combo.Items.Clear();
            foreach (Account acc in lstAccount)
            {
                from_accoutn_combo.Items.Add(acc.ProperAccountNumber);
                to_account_combo.Items.Add(acc.ProperAccountNumber);
            }

            if (lstAccount.Count > 0)
            {
                from_accoutn_combo.SelectedIndex = 0;
            }

            transaction_type.SelectedIndex = 0;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void submit_btn_Click(object sender, EventArgs e)
        {
            error_text_label.Visible = true;
            if (to_account_combo.Text == "")
            {
                error_text_label.Text = "To Account cannot be empty";
                return;
            }
            if (amount.Text == "")
            {
                error_text_label.Text = "Amount cannot be empty";
                return;
            }


            Transaction transaction = new Transaction();
            transaction.SenderAccountID = Account.UnFormatID(from_accoutn_combo.Text);

            foreach(Account account in lstAccount)
            {
                if (account.ProperAccountNumber == from_accoutn_combo.Text)
                {
                    transaction.SenderBranchID = account.BranchId;
                    transaction.SenderBankID = account.BankId;
                    break;
                }
            }

            transaction.ReciverAccountID = Account.UnFormatID(to_account_combo.Text);
            transaction.TransactionType = transaction_type.Text;
            transaction.Amount = float.Parse(amount.Text);
            transaction.Description = description.Text;
            transaction.TransactionDate = DateTime.Now;


            try
            {
                proxy.CreateTransaction(transaction);
            } catch (FaultException<CException> ex)
            {
                error_text_label.Text = ex.Detail.Reason;
                return;
            }

            this.Close();
        }
    }
}
