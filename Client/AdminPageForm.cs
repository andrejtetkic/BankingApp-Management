using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Client
{
    public partial class AdminPageForm : Form
    {
        private Dictionary<TabPage, Action> tabFunctions;
        IBank proxy;
        ChannelFactory<IBank> ch;

        List<User> users;
        List<Account> accounts;
        List<Loan> loans;
        List<Transaction> lstTransactions;
        List<Bank> banks;
        List<Branch> branches;

        Dictionary<int, string> bankID2Name;
        Dictionary<int, string> branchId2Name;

        public AdminPageForm()
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

        private void InitializeTabFunctions()
        {
            // Initialize the dictionary with tab pages and their associated functions
            tabFunctions = new Dictionary<TabPage, Action>
            {
                { dashboard_tab, DashboardLoad },
                { transactions_tab, TransactionsLoad },
                { accounts_tab, AccountsLoad },
                { loans_tab, LoansLoad },
            };
        }

        private void DashboardLoad()
        {
            users = proxy.GetAllUsers();
            accounts = proxy.GetAllAccounts();
            loans = proxy.GetAllLoans();

            user_amount_lb.Text = $"{users.Count} Users";
            account_amount_lb.Text = $"{accounts.Count} Accounts";

            long loaned = 0;
            foreach(Loan loan in loans)
            {
                loaned += loan.Amount;
            }

            loan_amount_lb.Text = $"${loaned} Loaned";
        }

        private void TransactionsLoad()
        {
            search_transaction_tb.Text = "Search Transactions";

            date_transaction_from.Value = DateTime.Now.AddDays(-30);
            date_transaction_to.Value = DateTime.Now.AddDays(1);


            //Fill Account combobox
            account_combo.Items.Clear();
            foreach (Account acc in accounts)
            {
                account_combo.Items.Add(acc.ProperAccountNumber);
            }

            if (accounts.Count > 0)
            {
                account_combo.SelectedIndex = 0;
            }

            transaction_type_combo.SelectedIndex = 0;


            //Get all transactions
            SetTransactions();
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
                row.Cells[6].Value = tran.BankFeeProcentage * tran.Amount / 100;
                row.Cells[7].Value = tran.Amount * (1 - tran.BankFeeProcentage / 100);
                row.Cells[8].Value = type;
                row.Cells[8].Style = new DataGridViewCellStyle { ForeColor = color, Alignment = DataGridViewContentAlignment.MiddleCenter };


                if (tran_type != transaction_type_combo.Text && transaction_type_combo.Text != "Any")
                {
                    row.Visible = false;
                }

                // Search
                if (search_transaction_tb.Text != "Search Transactions")
                {
                    if (!(name.ToLower().Contains(search_transaction_tb.Text.ToLower()) ||
                        tran.Description.ToLower().Contains(search_transaction_tb.Text.ToLower())))
                    {
                        row.Visible = false;
                    }
                }

                transactions.Rows.Add(row);

            }
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

        private void account_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTransactions();
        }

        private void filter_btn_Click(object sender, EventArgs e)
        {
            SetTransactions();
        }

        private void search_transaction_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                SetTransactions();
                e.Handled = true;
            }
        }

        private void SetAccounts()
        {
            accounts_data_view.Rows.Clear();

            accounts = proxy.GetAllAccounts();

            List<string> owners = new List<string>();

            foreach (Account account in accounts)
            {

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(accounts_data_view);
                row.Cells[0].Value = account.ProperAccountNumber;
                row.Cells[1].Value = proxy.GetNameOfAccountOwner(account.AccountNumber);
                row.Cells[2].Value = bankID2Name[account.BankId];
                row.Cells[3].Value = branchId2Name[account.BranchId];
                row.Cells[4].Value = "$" + account.Balance;


                if (!(bankID2Name[account.BankId] == account_bank_combo.Text || account_bank_combo.Text == "Any"))
                {
                    row.Visible = false;
                }
                if (!(branchId2Name[account.BranchId] == account_branch_combo.Text || account_branch_combo.Text == "Any"))
                {
                    row.Visible = false;
                }
                User user = proxy.GetUserFromAccount(account.AccountNumber);
                if (account_show_combo.Text != "All" && owners.Contains(user.JMBG))
                {
                    row.Visible = false;
                }
                owners.Add(user.JMBG);

                accounts_data_view.Rows.Add(row);
            }
        }

        private void AccountsLoad()
        {
            bankID2Name = proxy.GetAllBankNamesWithIDs();
            branchId2Name = proxy.GetAllBranchNamesWithIDs();

            account_bank_combo.Items.Clear();
            account_bank_combo.Items.Add("Any");
            banks = proxy.GetAllBanks();
            foreach (Bank bank in banks)
            {
                account_bank_combo.Items.Add(bank.Name);
            }

            account_bank_combo.SelectedIndex = 0;


            account_show_combo.SelectedIndex = 0;

            SetAccounts();
        }

        private void accounts_data_view_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string account = (string)accounts_data_view[0, e.RowIndex].Value;
            tabControl1.SelectedIndex = 2; // Set doubleClick page

            TransactionsLoad();
            account_combo.SelectedIndex = account_combo.Items.IndexOf(account);
        }

        private void account_bank_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (account_bank_combo.SelectedIndex == 0)
            {
                account_branch_combo.Items.Clear();
                account_branch_combo.Items.Add("Any");


                branches = proxy.GetAllBranches();
                foreach (Branch branch in branches)
                {
                    account_branch_combo.Items.Add(branch.Name);
                }

            }
            else
            {
                account_branch_combo.Items.Clear();
                account_branch_combo.Items.Add("Any");


                branches = proxy.GetAllBranchesOfBank(banks[account_bank_combo.SelectedIndex - 1].ID);
                foreach (Branch branch in branches)
                {
                    account_branch_combo.Items.Add(branch.Name);
                }
            }

            SetAccounts();


            account_branch_combo.SelectedIndex = 0;

        }

        private void account_branch_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAccounts();
        }

        private void account_show_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAccounts();
        }

        private void LoansLoad()
        {
            bankID2Name = proxy.GetAllBankNamesWithIDs();
            branchId2Name = proxy.GetAllBranchNamesWithIDs();

            loans_view.Rows.Clear();

            loans = proxy.GetAllLoans();

            User user;
            foreach (Loan loan in loans)
            {
                user = proxy.GetUser(loan.LenderJMBG);
                loans_view.Rows.Add(loan.ID, $"{user.Ime} {user.Prezime}", loan.Name, bankID2Name[loan.BankID], "$" + loan.Amount, Account.FormatID(loan.AccountNumber), loan.Interest, "$" + -loan.Balance);
            }

        }
    }
}
