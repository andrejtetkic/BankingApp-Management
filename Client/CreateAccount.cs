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
    public partial class CreateAccount : Form
    {
        IBank proxy;
        Dictionary<int, string> bankID2Name;
        List<Branch> branches;

        public CreateAccount(IBank proxy)
        {
            InitializeComponent();
            Utilities.SetTextBoxPlaceholder(account_number, "0-00-0");
            branch_combo.Items.Clear();

            bankID2Name = proxy.GetAllBankNamesWithIDs();
            branches = proxy.GetAllBranches();
            foreach (Branch br in branches)
            {
                branch_combo.Items.Add($"{br.Name} - {bankID2Name[br.BankID]}");
            }
            branch_combo.SelectedIndex = 0;

            this.proxy = proxy;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(account_number.Text == "0-00-0")
            {
                error_label.Visible = true;
                error_label.Text = "Account Number cannot be Empty";
                return;
            }

            if(!(account_number.Text.Length == 4 || account_number.Text.Length == 6))
            {
                error_label.Visible = true;
                error_label.Text = "Invalid Account Number Format";
                return;
            }

            Account account = new Account();
            account.AccountNumber = Account.UnFormatID(account_number.Text);
            account.BranchId = branches[branch_combo.SelectedIndex].ID;
            account.BankId = branches[branch_combo.SelectedIndex].BankID;
            account.LenderJMBG = SessionManager.GetCurrentUser().UserId;

            try
            {
                proxy.CreateAccount(account);
            }
            catch(FaultException<CException> ex)
            {
                error_label.Visible = true;
                error_label.Text = ex.Detail.Reason;
                return;
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void account_number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
        }
    }
}
