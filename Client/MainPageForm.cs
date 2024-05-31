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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class MainPageForm : Form
    {
        private Dictionary<TabPage, Action> tabFunctions;


        IBank proxy;
        ChannelFactory<IBank> ch;
        List<Account> lstAccount;
        Dictionary<int, string> bankID2Name;
        Dictionary<int, string> branchId2Name;
        List<Transaction> lstTransactions;
        float totalBalance;
        List<Loan> lstLoans;

        public MainPageForm()
        {
            InitializeComponent();
            InitializeTabFunctions();
            Utilities.SetTextBoxPlaceholder(search_transaction_tb, "Search Transactions");


            ch = new ChannelFactory<IBank>(new BasicHttpBinding(),
                new EndpointAddress("http://localhost:8000"));
            ch.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior());
            proxy = ch.CreateChannel();

            DashboardLoad();

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            lstTransactions = proxy.GetAllTransactionsOfAccount(
                Account.UnFormatID((string)dataGridView1.CurrentCell.OwningRow.Cells[0].Value), 
                DateTime.Now.AddDays(-30), DateTime.Now);

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
            accounts_data_view.Rows.Clear();

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // Get the selected tab page
            TabPage selectedTab = e.TabPage;

            // Execute the associated function (if it exists)
            if (tabFunctions.TryGetValue(selectedTab, out Action function))
            {
                function.Invoke();
            }
        }

        private void InitializeTabFunctions()
        {
            // Initialize the dictionary with tab pages and their associated functions
            tabFunctions = new Dictionary<TabPage, Action>
            {
                { dashboard_tab, DashboardLoad },
                { transactions_tab, TransactionsLoad},
                { accounts_tab, AccountsLoad},
                { loans_tab, LoansLoad},
            };
        }

        private void TransactionsLoad()
        {
            search_transaction_tb.Text = "Search Transactions";

            date_transaction_from.Value = DateTime.Now.AddDays(-30);
            date_transaction_to.Value = DateTime.Now.AddDays(1);


            //Fill Account combobox
            account_combo.Items.Clear();
            foreach(Account acc in lstAccount)
            {
                account_combo.Items.Add(acc.ProperAccountNumber);
            }

            if(lstAccount.Count > 0)
            {
                account_combo.SelectedIndex = 0;
            }

            transaction_type_combo.SelectedIndex = 0;


            //Get all transactions
            SetTransactions();
        }

        private void DashboardLoad()
        {
            dataGridView1.Rows.Clear();

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

        private void AccountsLoad()
        {
            accounts_data_view.Rows.Clear();

            lstAccount = proxy.GetAllAccountsOfUser(SessionManager.GetCurrentUser().UserId);
            bankID2Name = proxy.GetAllBankNamesWithIDs();
            branchId2Name = proxy.GetAllBranchNamesWithIDs();

            foreach (Account account in lstAccount)
            {
                accounts_data_view.Rows.Add(account.ProperAccountNumber, bankID2Name[account.BankId], branchId2Name[account.BranchId], "$" + account.Balance);
            }
        }

        private void LoansLoad()
        {
            loans_view.Rows.Clear();

            lstLoans = proxy.GetAllLoansOfUser(SessionManager.GetCurrentUser().UserId);

            foreach (Loan loan in lstLoans)
            {
                loans_view.Rows.Add(loan.ID, loan.Name, bankID2Name[loan.BankID], "$" + loan.Amount, Account.FormatID(loan.AccountNumber), loan.Interest, "$" + -loan.Balance);
            }

        }

        private void SetTransactions()
        {
            transactions.Rows.Clear();

            lstTransactions = proxy.GetAllTransactionsOfAccount(
                Account.UnFormatID(account_combo.Text), date_transaction_from.Value, date_transaction_to.Value);

            foreach (Transaction tran in lstTransactions)
            {
                string type = tran.Type == "sent" ? "↑" : "↓";
                Color color = tran.Type == "sent" ? Color.Red : Color.Green;

                string tran_type = tran.TransactionType;
                string account = "";
                string name = proxy.GetNameOfAccountOwner(tran.SenderAccountID);
                if (tran.TransactionType == "transfer" || tran.TransactionType == "Transfer")
                {
                    if (tran.Type == "sent")
                    {
                        tran_type = "Transfer To";
                        name = proxy.GetNameOfAccountOwner(tran.ReciverAccountID);
                    }
                    else
                    {
                        tran_type = "Transfer From";
                    }

                }

                if (tran.Type == "sent")
                {
                    name = proxy.GetNameOfAccountOwner(tran.ReciverAccountID);
                    account = Account.FormatID(tran.ReciverAccountID);
                }
                else
                {
                    account = Account.FormatID(tran.SenderAccountID);
                }


                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(transactions);
                row.Cells[0].Value = tran.TransactionDate;
                row.Cells[1].Value = tran_type;
                row.Cells[2].Value = name;
                row.Cells[3].Value = account;
                row.Cells[4].Value = tran.Description;
                row.Cells[5].Value = "$" + tran.Amount;
                row.Cells[6].Value = tran.BankFeeProcentage * tran.Amount/100;
                row.Cells[7].Value = tran.Amount * (1 - tran.BankFeeProcentage/100);
                row.Cells[8].Value = type;
                row.Cells[8].Style = new DataGridViewCellStyle { ForeColor = color, Alignment = DataGridViewContentAlignment.MiddleCenter };


                if (tran_type != transaction_type_combo.Text && transaction_type_combo.Text != "Any")
                {
                    row.Visible = false;
                }

                transactions.Rows.Add(row);

            }
        }

        private void account_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTransactions();
        }

        private void filter_btn_Click(object sender, EventArgs e)
        {
            SetTransactions();
        }

        private void start_transaction_btn_Click(object sender, EventArgs e)
        {
            NewTransaction new_transaction_form = new NewTransaction(proxy, lstAccount);
            new_transaction_form.ShowDialog();

            SetTransactions();
        }

        private void accounts_data_view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string account = (string)accounts_data_view[0, e.RowIndex].Value;
            tabControl1.SelectedIndex = 4; // Set doubleClick page

            TransactionsLoad();
            account_combo.SelectedIndex = account_combo.Items.IndexOf(account);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateAccount createAccount = new CreateAccount(proxy);
            createAccount.ShowDialog();


            lstAccount = proxy.GetAllAccountsOfUser(SessionManager.GetCurrentUser().UserId);
            AccountsLoad();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateLoan createLoan = new CreateLoan(proxy, lstAccount);
            createLoan.ShowDialog();

            
            LoansLoad();
        }
    }
}
