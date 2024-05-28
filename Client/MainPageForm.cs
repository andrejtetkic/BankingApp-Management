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
    public partial class MainPageForm : Form
    {
        IBank proxy;
        ChannelFactory<IBank> ch;
        List<Account> lstAccount;
        Dictionary<int, string> bankID2Name;
        Dictionary<int, string> branchId2Name;
        List<Transaction> lstTransactions;
        float totalBalance;

        public MainPageForm()
        {
            InitializeComponent();

            ch = new ChannelFactory<IBank>(new BasicHttpBinding(),
                new EndpointAddress("http://localhost:8000"));
            ch.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior());
            proxy = ch.CreateChannel();


            // This should be on DashboardLoad
            lstAccount = proxy.GetAllAccountsOfUser(SessionManager.GetCurrentUser().UserId);
            bankID2Name = proxy.GetAllBankNamesWithIDs();
            branchId2Name = proxy.GetAllBranchNamesWithIDs();

            totalBalance = 0;
            foreach (Account account in lstAccount)
            {
                dataGridView1.Rows.Add(account.ProperAccountNumber, "$" + account.Balance, bankID2Name[account.BankId], branchId2Name[account.BranchId]);
                totalBalance += account.Balance;
            }

            label3.Text = "$" + totalBalance.ToString();


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            lstTransactions = proxy.GetAllTransactionsOfAccount(
                Account.UnFormatID((string)dataGridView1.CurrentCell.OwningRow.Cells[0].Value));

            foreach (Transaction tran in lstTransactions)
            {
                string type = tran.Type == "sent" ? "↑" : "↓";
                Color color = tran.Type == "sent" ? Color.Red : Color.Green;


                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView2);
                row.Cells[0].Value = tran.TransactionDate;
                row.Cells[1].Value = "$" + tran.Amount;
                row.Cells[2].Value = type;
                row.Cells[2].Style = new DataGridViewCellStyle { ForeColor = color , Alignment = DataGridViewContentAlignment.MiddleCenter };

                dataGridView2.Rows.Add(row);
            }
        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
