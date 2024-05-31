using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class BankServer : IBank
    {
        public User AuthenticateUser(string username, string password)
        {
            
            string sqlQuery = $"select * from korisnik where email='{username}' and password='{Hash.HashPassword(password)}'";
            List<Dictionary<string, object>> accounts = Database.ExecuteSelectCommand(sqlQuery);
            if (accounts.Count() > 0)
            {
                return new User(accounts[0]);

            }

            return null;
        }

        public bool ChangeAccountBalance(string account_id, float change)
        {
            string sqlQuery = $"UPDATE racun SET stanje = stanje + {change} " +
                $"WHERE broj_racuna = '{account_id}'";

            string check_query = $"SELECT COUNT(*) FROM racun WHERE broj_racuna = '{account_id}'";

            if (Database.ExecuteSelectCommand(check_query).Count == 0)
            {
                return false;
            }


            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public bool ChangeUserPassword(string user_id, string new_password, string current_password)
        {

            string password_query = $"SELECT password FROM korisnik WHERE jmbg_korisnika = '{user_id}'";

            if (Hash.HashPassword(current_password) != (string)Database.ExecuteSelectCommand(password_query)[0]["password"])
            {
                return false;
            }

            


            string sqlQuery = $"UPDATE korisnik SET password='{Hash.HashPassword(new_password)}' " +
                $"WHERE jmbg_korisnika = '{user_id}'";

            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public bool ChangeLoanBalance(string account_number, float change)
        {
            string sqlQuery = $"UPDATE kredit SET stanje = stanje + {change} " +
                $"WHERE broj_racuna = '{account_number}' and stanje < 0" ;


            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public void CreateAccount(Account account)
        {

            if(account.AccountNumber.Length != 4)
            {
                CException exception = new CException($"Invalid Account Number Format");
                throw new FaultException<CException>(exception);
            }

            string sqlQuery = $"INSERT INTO racun VALUES('{account.AccountNumber}', {account.BranchId}, {account.BankId}, '{account.LenderJMBG}', {account.Balance})";
            try
            {
                if (!Database.ExecuteNonQueryCommand(sqlQuery))
                {
                    CException exception = new CException($"Account with an Account Number {account.AccountNumber} already exists!");
                    throw new FaultException<CException>(exception);
                }
            }
            catch (Exception)
            {
                CException exception = new CException($"Account Number already exists!");
                throw new FaultException<CException>(exception);
            }
        }

        public void CreateBank(Bank bank)
        {
            string sqlQuery = $"INSERT INTO banka VALUES('{bank.Name}', '{bank.City}')";
            // TODO: check if there is a bank with that name and that city already

            try
            {
                if (!Database.ExecuteNonQueryCommand(sqlQuery))
                {
                    CException exception = new CException("Something went Wrong");
                    throw new FaultException<CException>(exception);
                }
            } catch (Exception)
            {
                CException exception = new CException("Something went Wrong");
                throw new FaultException<CException>(exception);
            }
}

        public void CreateBranch(Branch branch)
        {
            string sqlQuery = $"INSERT INTO filijala VALUES({branch.BankID}, '{branch.Name}', '{branch.Address}')";
            // TODO: check if there is a branch with that name and that bank already nad the address

            try
            {
                if (!Database.ExecuteNonQueryCommand(sqlQuery))
                {
                    CException exception = new CException("Something went Wrong");
                    throw new FaultException<CException>(exception);
                }
            } catch (Exception)
            {
                CException exception = new CException("Something went wrong");
                throw new FaultException<CException>(exception);
            }
            
        }

        public void CreateLoan(Loan loan)
        {

            Account account = GetAccountById(loan.AccountNumber);

            if (loan.BankID != account.BankId)
            {
                CException exception = new CException("Your Account is not in the Selected Bank");
                throw new FaultException<CException>(exception);
            }

            if (loan.Amount <= 0)
            {
                CException exception = new CException("Cannot have negative Amount");
                throw new FaultException<CException>(exception);
            }

            string check_query = $"SELECT stanje FROM kredit WHERE broj_racuna = '{loan.AccountNumber}'";

            List<Dictionary<string, object>> select = Database.ExecuteSelectCommand(check_query);


            foreach (Dictionary<string, object> s in select)
            {
                if (Convert.ToSingle(s["stanje"]) < 0)
                {
                    CException exception = new CException("Cannot have Multiple Active Loans on the same Account");
                    throw new FaultException<CException>(exception);
                }
            }

            loan.Interest = 5;
            loan.Balance = -loan.Amount * (1f + 5f/100);



            string sqlQuery = $"INSERT INTO kredit VALUES({loan.BankID}, '{loan.LenderJMBG}', '{loan.Name}', {loan.Amount}, {loan.Interest}, {loan.Balance}, {loan.AccountNumber})";

            try
            {
                if (!Database.ExecuteNonQueryCommand(sqlQuery))
                {
                    CException exception = new CException("Something went Wrong");
                    throw new FaultException<CException>(exception);
                }
            }
            catch (Exception)
            {
                CException exception = new CException("Something went wrong");
                throw new FaultException<CException>(exception);
            }

            
            Transaction transaction = new Transaction();
            transaction.SenderBranchID = GetBankById(loan.BankID).MainBranch;
            transaction.SenderBankID = loan.BankID;
            transaction.SenderAccountID = $"{loan.BankID.ToString("D4")}";

            transaction.ReciverBranchID = account.BranchId;
            transaction.ReciverBankID = account.BankId;
            transaction.ReciverAccountID = account.AccountNumber;

            transaction.Amount = loan.Amount;
            transaction.Description = $"Loan of {loan.Name}";
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = "Loan Disbursement";

            CreateTransaction(transaction);

        }

        User GetUserFromAccount(string account)
        {
            string sqlQuery = $"select * from korisnik k join racun r on k.jmbg_korisnika=r.jmbg_korisnika where r.broj_racuna='{account}'";
            List<Dictionary<string, object>> users = Database.ExecuteSelectCommand(sqlQuery);

            if(users.Count == 0) return null;

            return new User(users[0]);

        }

        Account GetAccountById(string accountId)
        {
            string sqlQuery = $"select * from racun where broj_racuna='{accountId}'";
            List<Dictionary<string, object>> accounts = Database.ExecuteSelectCommand(sqlQuery);

            if(accounts.Count == 0) return null;

            return new Account(accounts[0]);
        }

        Bank GetBankById(int bankId)
        {
            string sqlQuery = $"select * from banka where id_banke={bankId}";
            List<Dictionary<string, object>> banks = Database.ExecuteSelectCommand(sqlQuery);

            if (banks.Count == 0) return null;

            return new Bank(banks[0]);
        }

        float GetBankFeeProcentage(string transaction_type)
        {
            float feePercentage = 0f;

            switch (transaction_type)
            {
                case "inner":
                    feePercentage = 0f;
                    break;
                case "transfer":
                    feePercentage = 1.0f; // Example: 1% for transfers
                    break;
                case "Transfer":
                    feePercentage = 1.0f; // Example: 1% for transfers
                    break;
                case "Deposit":
                    feePercentage = 0.5f; // Example: 0.5% for deposits
                    break;
                case "Withdrawal":
                    feePercentage = 1.5f; // Example: 1.5% for withdrawals
                    break;
                case "Payment":
                    feePercentage = 1.0f; // Example: 1% for payments
                    break;
                case "Loan Disbursement":
                    feePercentage = 0f; // Example: 0.2% for loan disbursements
                    break;
                case "Loan Repayment":
                    feePercentage = 0.0f; // Example: 0% for loan repayments
                    break;
                case "Interest Payment":
                    feePercentage = 0.0f; // Example: 0% for interest payments
                    break;
                case "Fee":
                    feePercentage = 0.0f; // Example: 0% for fees
                    break;
                case "Refund":
                    feePercentage = 0.0f; // Example: 0% for refunds
                    break;
                case "Adjustment":
                    feePercentage = 0.0f; // Example: 0% for adjustments
                    break;
                default:
                    feePercentage = 5f;
                    break;
            }

            return feePercentage;
        }

        public void CreateTransaction(Transaction transaction)
        {
            CException exception = new CException("Something went Wrong");


            User sender = GetUserFromAccount(transaction.SenderAccountID);
            User reciver = GetUserFromAccount(transaction.ReciverAccountID);


            if(sender == null || reciver == null)
            {
                exception.Reason = "Invalid Reciver Account";
                throw new FaultException<CException>(exception);
            }

            Account sender_account = GetAccountById(transaction.SenderAccountID);
            if(sender_account.Balance < transaction.Amount && transaction.TransactionType != "Loan Disbursement")
            {
                exception.Reason = "Not enough Money on your Account";
                throw new FaultException<CException>(exception);
            }


            Account reciver_account = GetAccountById(transaction.ReciverAccountID);
            if(reciver_account == null)
            {
                exception.Reason = "Invalid Reciver Account";
                throw new FaultException<CException>(exception);
            }

            transaction.ReciverBankID = reciver_account.BankId;
            transaction.ReciverBranchID = reciver_account.BranchId;


            if (sender.JMBG == reciver.JMBG)
            {
                transaction.BankFeeProcentage = GetBankFeeProcentage("inner");
            }
            else
            {
                transaction.BankFeeProcentage = GetBankFeeProcentage(transaction.TransactionType);
            }

            

            string sqlQuery = $"INSERT INTO Transactions VALUES('{transaction.SenderAccountID}', {transaction.SenderBranchID}, {transaction.SenderBankID}, '{transaction.ReciverAccountID}', {transaction.ReciverBranchID}, {transaction.ReciverBankID}, '{transaction.TransactionType}', {transaction.Amount}, '{transaction.TransactionDate}', '{transaction.Description}', {transaction.BankFeeProcentage})";


            try
            {
                if (!Database.ExecuteNonQueryCommand(sqlQuery))
                {
                    throw new FaultException<CException>(exception);
                }
            }
            catch (Exception)
            {
                throw new FaultException<CException>(exception);
            }


            if(transaction.TransactionType == "Loan Repayment")
            {
                ChangeLoanBalance(transaction.SenderAccountID, transaction.Amount);
            }

            float bank_fee = transaction.Amount * transaction.BankFeeProcentage / 100;
            ChangeAccountBalance(transaction.SenderAccountID, -transaction.Amount);
            ChangeAccountBalance(transaction.ReciverAccountID, transaction.Amount - bank_fee);
            ChangeAccountBalance($"{transaction.SenderBankID.ToString("D4")}", bank_fee); // Bank account


        }

        public void CreateUser(User user)
        {
            string sqlQuery = $"INSERT INTO korisnik VALUES('{user.JMBG}', '{user.Ime}', '{user.Prezime}', '{user.Email}', '{Hash.HashPassword(user.PasswordHash)}', {user.Privilage})";
            CException exception = new CException($"User of JMBG {user.JMBG} already exists! (or unknown error)");

            try
            {
                if (!Database.ExecuteNonQueryCommand(sqlQuery))
                {
                    throw new FaultException<CException>(exception);
                }
            }
            catch (Exception)
            {
                throw new FaultException<CException>(exception);
            }
        }

        public bool DeleteAccount(string accout_id)
        {

            throw new NotImplementedException();
            // Such action should not be allowed as to not lose reference to Transactions. 
            // Attribute isActive could be added to the database, but for the purpuses od the project - it is not nececery

        }

        public bool DeleteBank(int bank_id)
        {
            throw new NotImplementedException();
            // Again, such action would be destructive and as such should not exist
        }

        public bool DeleteBranch(int branch_id)
        {
            throw new NotImplementedException();
            // Again, such action would be destructive and as such should not exist
        }

        public bool DeleteLoan(int loan_id)
        {
            string sqlQuery = $"DELETE FROM kredit WHERE id_kredita={loan_id}";

            string check_query = $"SELECT * FROM kredit WHERE id_kredita={loan_id}";
            if (!Database.ExecuteScalarCommand(check_query))
            {
                return false;
            }


            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public bool DeleteTransaction(int transaction_id)
        {
            throw new NotImplementedException();
            //Instead of deleating a Transaction, an opposite Transaction should be executed
        }

        public bool DeleteUser(int user_id)
        {
            throw new NotImplementedException();
            // Again, such action would be destructive and as such should not exist
        }

        public List<Account> GetAllAccounts()
        {
            string sqlQuery = $"select * from racun";
            List<Dictionary<string, object>> accounts = Database.ExecuteSelectCommand(sqlQuery);

            List<Account> return_accounts = new List<Account>();
            foreach (Dictionary<string, object> account in accounts)
            {
                return_accounts.Add(new Account(account));
            }

            return return_accounts;
        }

        public List<Account> GetAllAccountsOfUser(string user_id)
        {
            string sqlQuery = $"select * from racun where jmbg_korisnika='{user_id}'";
            List<Dictionary<string, object>> accounts = Database.ExecuteSelectCommand(sqlQuery);

            List<Account> return_accounts = new List<Account>();
            foreach (Dictionary<string, object> account in accounts)
            {
                return_accounts.Add(new Account(account));
            }

            return return_accounts;
        }

        public Dictionary<int, string> GetAllBankNamesWithIDs()
        {
            string sqlQuery = $"select id_banke, naziv_banke from banka";

            List<Dictionary<string, object>> banks = Database.ExecuteSelectCommand(sqlQuery);
            Dictionary<int, string> return_values = new Dictionary<int, string>();


            foreach (Dictionary<string, object> bank in banks)
            {
                return_values.Add((int)bank["id_banke"], (string)bank["naziv_banke"]);
            }

            return return_values;
        }

        public List<Bank> GetAllBanks()
        {
            string sqlQuery = $"select * from banka";

            List<Dictionary<string, object>> branches = Database.ExecuteSelectCommand(sqlQuery);
            List<Bank> return_values = new List<Bank>();


            foreach (Dictionary<string, object> branch in branches)
            {
                return_values.Add(new Bank(branch));
            }

            return return_values;
        }

        public List<Branch> GetAllBranches()
        {
            string sqlQuery = $"select * from filijala";

            List<Dictionary<string, object>> branches = Database.ExecuteSelectCommand(sqlQuery);
            List<Branch> return_values = new List<Branch>();


            foreach (Dictionary<string, object> branch in branches)
            {
                return_values.Add(new Branch(branch));
            }

            return return_values;
        }

        public Dictionary<int, string> GetAllBranchNamesWithIDs()
        {
            string sqlQuery = $"select id_filijale, naziv_filijale from filijala";

            List<Dictionary<string, object>> branches = Database.ExecuteSelectCommand(sqlQuery);
            Dictionary<int, string> return_values = new Dictionary<int, string>();


            foreach (Dictionary<string, object> branch in branches)
            {
                return_values.Add((int)branch["id_filijale"], (string)branch["naziv_filijale"]);
            }

            return return_values;
        }

        public List<Loan> GetAllLoans()
        {
            string sqlQuery = $"select * from kredit";
            List<Dictionary<string, object>> loans = Database.ExecuteSelectCommand(sqlQuery);

            List<Loan> return_loans = new List<Loan>();
            foreach (Dictionary<string, object> loan in loans)
            {
                return_loans.Add(new Loan(loan));
            }

            return return_loans;
        }

        public List<Loan> GetAllLoansOfUser(string user_id)
        {
            string sqlQuery = $"select * from kredit where jmbg_korisnika='{user_id}'";
            List<Dictionary<string, object>> loans = Database.ExecuteSelectCommand(sqlQuery);

            List<Loan> return_loans = new List<Loan>();
            foreach (Dictionary<string, object> loan in loans)
            {
                return_loans.Add(new Loan(loan));
            }

            return return_loans;
        }

        public List<Transaction> GetAllTransactions()
        {
            string sqlQuery = $"select * from Transactions";
            List<Dictionary<string, object>> transactions = Database.ExecuteSelectCommand(sqlQuery);

            List<Transaction> return_transactions = new List<Transaction>();
            foreach (Dictionary<string, object> transaction in transactions)
            {
                return_transactions.Add(new Transaction(transaction));
            }

            return return_transactions;
        }

        public List<Transaction> GetAllTransactionsOfAccount(string account_id, DateTime from, DateTime to)
        {
            //TODO
            string sqlQuery = $"SELECT t.* , iif(t.SenderAccountId = rs.broj_racuna, 'sent', 'received') [type] " +
                $"FROM Transactions t left JOIN racun rs ON t.SenderAccountId = rs.broj_racuna OR " +
                $"t.ReceiverAccountId = rs.broj_racuna WHERE rs.broj_racuna = '{account_id}' and t.TransactionDate BETWEEN '{from}' and '{to}'";

            List<Dictionary<string, object>> transactions = Database.ExecuteSelectCommand(sqlQuery);

            List<Transaction> return_transactions = new List<Transaction>();
            foreach (Dictionary<string, object> transaction in transactions)
            {
                return_transactions.Add(new Transaction(transaction));
            }

            return return_transactions;
        }

        public List<User> GetAllUsers()
        {
            string sqlQuery = $"select * from korisnik";
            List<Dictionary<string, object>> users = Database.ExecuteSelectCommand(sqlQuery);
            
            List<User> return_users = new List<User>();
            foreach(Dictionary<string, object> user in users)
            {
                return_users.Add(new User(user));
            }

            return return_users;
        }

        public string GetNameOfAccountOwner(string account_id)
        {
            string sqlQuery = $"SELECT k.ime, k.prezime FROM korisnik k INNER JOIN racun r ON k.jmbg_korisnika = r.jmbg_korisnika WHERE r.broj_racuna = '{account_id}'";

            List<Dictionary<string, object>> res = Database.ExecuteSelectCommand(sqlQuery);

            string return_name = "";
            if(res.Count > 0) return_name = (string)res[0]["ime"] + " " + (string)res[0]["prezime"];

            return return_name;
        }

        public bool SetUserAdmin(string user_id)
        {
            string sqlQuery = $"UPDATE korisnik SET privileges={(int)PrivilagesEnum.Admin} WHERE jmbg_korisnika = '{user_id}'";

          


            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public bool UpdateAccount(string account_id, Account account)
        {
            string sqlQuery = $"UPDATE racun SET id_filijale = {account.BranchId}, id_banke = {account.BankId}, " +
                $"jmbg_korisnika = '{account.LenderJMBG}', stanje = {account.Balance} " +
                $"WHERE broj_racuna = '{account_id}'";

            string check_query = $"SELECT * FROM racun WHERE broj_racuna='{account_id}'";
            if (!Database.ExecuteScalarCommand(check_query))
            {
                return false;
            }


            return Database.ExecuteNonQueryCommand(sqlQuery);
            // This would probably never be used

        }

        public bool UpdateBank(int bank_id, Bank bank)
        {
            string sqlQuery = $"UPDATE banka SET naziv_banke = '{bank.Name}', " +
                $"grad = '{bank.City}' WHERE id_banke = {bank_id}";

            string check_query = $"SELECT * FROM banka WHERE id_banke={bank_id}";
            if (!Database.ExecuteScalarCommand(check_query))
            {
                return false;
            }


            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public bool UpdateBranch(int branch_id, Branch branch)
        {
            string sqlQuery = $"UPDATE filijala SET naziv_filijale = '{branch.Name}', " +
                $"adresa_filijale = '{branch.Address}' WHERE id_filijale = {branch_id}";

            string check_query = $"SELECT * FROM filijala WHERE id_filijale={branch_id}";
            if (!Database.ExecuteScalarCommand(check_query))
            {
                return false;
            }


            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public bool UpdateLoan(int loan_id, Loan loan)
        {
            throw new NotImplementedException(); // Dont do this
        }

        public bool UpdateTransaction(int transaction_id, Transaction transaction)
        {
            throw new NotImplementedException(); // Dont do this
        }

        public bool UpdateUser(string user_id, User user)
        {
            string sqlQuery = $"UPDATE korisnik SET ime = '{user.Ime}', " +
                $"prezime = '{user.Prezime}', email='{user.Email}' WHERE jmbg_korisnika = '{user_id}'";

            return Database.ExecuteNonQueryCommand(sqlQuery);
        }

        public User GetUser(string user_id)
        {
            string sqlQuery = $"select * from korisnik where jmbg_korisnika='{user_id}'";
            List<Dictionary<string, object>> users = Database.ExecuteSelectCommand(sqlQuery);

            if (users.Count == 0) return null;

            return new User(users[0]);
        }
    }
}
