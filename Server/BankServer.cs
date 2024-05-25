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

        public void CreateAccount(Account account)
        {
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
                CException exception = new CException("Something went Wrong");
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
            string sqlQuery = $"INSERT INTO kredit VALUES({loan.BankID}, '{loan.LenderJMBG}', '{loan.Name}', {loan.Amount}, {loan.Interest})";

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
        }

        public void CreateTransaction(Transaction transaction)
        {
            string sqlQuery = $"INSERT INTO Transactions VALUES('{transaction.SenderAccountID}', {transaction.SenderBranchID}, {transaction.SenderBankID}, '{transaction.ReciverAccountID}', {transaction.ReciverBranchID}, {transaction.ReciverBankID}, '{transaction.TransactionType}', {transaction.Amount}, '{transaction.TransactionDate}', '{transaction.Description}', {transaction.BankFeeProcentage})";

            CException exception = new CException("Something went Wrong");

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

            //TODO: Update both parties balances
        }

        public void CreateUser(User user)
        {
            string sqlQuery = $"INSERT INTO korisnik VALUES('{user.JMBG}', '{user.Ime}', '{user.Prezime}', '{user.Email}', '{Hash.HashPassword(user.PasswordHash)}', {user.Privilage})";
            // TODO: check if there is a branch with that name and that bank already nad the address
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
            string sqlQuery = $"select * from racun where jmbg_korisnika={user_id}";
            List<Dictionary<string, object>> accounts = Database.ExecuteSelectCommand(sqlQuery);

            List<Account> return_accounts = new List<Account>();
            foreach (Dictionary<string, object> account in accounts)
            {
                return_accounts.Add(new Account(account));
            }

            return return_accounts;
        }

        public List<Bank> GetAllBanks()
        {
            throw new NotImplementedException();
        }

        public List<Branch> GetAllBranches()
        {
            throw new NotImplementedException();
        }

        public List<Loan> GetAllLoans()
        {
            throw new NotImplementedException();
        }

        public List<Loan> GetAllLoansOfUser(User user)
        {
            throw new NotImplementedException();
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

        public List<Transaction> GetAllTransactionsOfUser(string user_id)
        {
            //TODO
            string sqlQuery = $"select * from Transactions where ";
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

        public bool UpdateAccount(string account_id, Account account)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBank(int bank_id, Bank bank)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBranch(int branch_id, Branch branch)
        {
            throw new NotImplementedException();
        }

        public bool UpdateLoan(int loan_id, Loan loan)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTransaction(int transaction_id, Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(int user_id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
