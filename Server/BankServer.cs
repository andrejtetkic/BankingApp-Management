using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class BankServer : IBank
    {
        public User AuthenticateUser(string username, string password)
        {
            
            string sqlQuery = $"select * from korisnik where email='{username}' and password='{password}'";
            List<Dictionary<string, object>> accounts = Database.ExecuteSelectCommand(sqlQuery);
            if (accounts.Count() > 0)
            {
                return new User(accounts[0]);

            }

            return null;
        }

        public void CreateAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public void CreateBank(Bank bank)
        {
            throw new NotImplementedException();
        }

        public void CreateBranch(Branch branch)
        {
            throw new NotImplementedException();
        }

        public void CreateLoan(Loan loan)
        {
            throw new NotImplementedException();
        }

        public void CreateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAccount(int accout_id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBank(int bank_id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBranch(int branch_id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLoan(int loan_id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTransaction(int transaction_id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int user_id)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAllAccountsOfUser(User user)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public List<Transaction> GetAllTransactionsOfUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public bool UpdateAccount(int account_id, Account account)
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
