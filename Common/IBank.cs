using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IBank
    {
        [OperationContract]
        User AuthenticateUser(string username, string password);

        #region CRUD Account
        [OperationContract]
        void CreateAccount(Account account);

        [OperationContract]
        List<Account> GetAllAccounts();

        [OperationContract]
        List<Account> GetAllAccountsOfUser(User user);

        [OperationContract]
        bool UpdateAccount(int account_id, Account account);

        [OperationContract]
        bool DeleteAccount(int accout_id);

        #endregion


        #region CRUD User
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void CreateUser(User user);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        bool UpdateUser(int user_id, User user);

        [OperationContract]
        bool DeleteUser(int user_id);

        #endregion


        #region CRUD Bank
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void CreateBank(Bank bank);

        [OperationContract]
        List<Bank> GetAllBanks();

        [OperationContract]
        bool UpdateBank(int bank_id, Bank bank);

        [OperationContract]
        bool DeleteBank(int bank_id);

        #endregion


        #region CRUD Branch
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void CreateBranch(Branch branch);

        [OperationContract]
        List<Branch> GetAllBranches();

        [OperationContract]
        bool UpdateBranch(int branch_id, Branch branch);

        [OperationContract]
        bool DeleteBranch(int branch_id);

        #endregion


        #region CRUD Loan

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void CreateLoan(Loan loan);

        [OperationContract]
        List<Loan> GetAllLoans();


        [OperationContract]
        List<Loan> GetAllLoansOfUser(User user);

        [OperationContract]
        bool UpdateLoan(int loan_id, Loan loan);

        [OperationContract]
        bool DeleteLoan(int loan_id);

        #endregion


        #region CRUD Transaction

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void CreateTransaction(Transaction transaction);

        [OperationContract]
        List<Transaction> GetAllTransactions();


        [OperationContract]
        List<Transaction> GetAllTransactionsOfUser(User user);

        [OperationContract]
        bool UpdateTransaction(int transaction_id, Transaction transaction);

        [OperationContract]
        bool DeleteTransaction(int transaction_id);

        #endregion

    }
}
