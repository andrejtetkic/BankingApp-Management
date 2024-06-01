using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Common
{
    [ServiceContract]
    public interface IBank
    {
        

        #region CRUD Account
        [OperationContract]
        [FaultContract(typeof(CException))]
        void CreateAccount(Account account);

        [OperationContract]
        List<Account> GetAllAccounts();

        [OperationContract]
        List<Account> GetAllAccountsOfUser(string user_id);

        [OperationContract]
        bool UpdateAccount(string account_id, Account account);

        [OperationContract]
        bool DeleteAccount(string accout_id);

        [OperationContract]
        string GetNameOfAccountOwner(string account_id);

        #endregion


        #region CRUD User
        [OperationContract]
        [FaultContract(typeof(CException))]
        void CreateUser(User user);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        User GetUserFromAccount(string account_id);

        [OperationContract]
        User GetUser(string user_id);

        [OperationContract]
        bool UpdateUser(string user_id, User user);

        [OperationContract]
        bool DeleteUser(int user_id);




        [OperationContract]
        User AuthenticateUser(string username, string password);


        [OperationContract]
        bool SetUserAdmin(string user_id);

        [OperationContract]
        bool ChangeUserPassword(string user_id, string password, string current_password);

        #endregion


        #region CRUD Bank
        [OperationContract]
        [FaultContract(typeof(CException))]
        void CreateBank(Bank bank);

        [OperationContract]
        List<Bank> GetAllBanks();

        [OperationContract]
        bool UpdateBank(int bank_id, Bank bank);

        [OperationContract]
        bool DeleteBank(int bank_id);


        [OperationContract]
        Dictionary<int, string> GetAllBankNamesWithIDs();

        [OperationContract]
        Bank GetBank(int bank_id);

        #endregion


        #region CRUD Branch
        [OperationContract]
        [FaultContract(typeof(CException))]
        void CreateBranch(Branch branch);

        [OperationContract]
        List<Branch> GetAllBranches();

        [OperationContract]
        List<Branch> GetAllBranchesOfBank(int bank_id);

        [OperationContract]
        bool UpdateBranch(int branch_id, Branch branch);

        [OperationContract]
        bool DeleteBranch(int branch_id);

        [OperationContract]
        Branch GetBranch(int branch_id);



        [OperationContract]
        Dictionary<int, string> GetAllBranchNamesWithIDs();

        #endregion


        #region CRUD Loan

        [OperationContract]
        [FaultContract(typeof(CException))]
        void CreateLoan(Loan loan);

        [OperationContract]
        List<Loan> GetAllLoans();


        [OperationContract]
        List<Loan> GetAllLoansOfUser(string user_id);

        [OperationContract]
        bool UpdateLoan(int loan_id, Loan loan);

        [OperationContract]
        bool DeleteLoan(int loan_id);

        #endregion


        #region CRUD Transaction

        [OperationContract]
        [FaultContract(typeof(CException))]
        void CreateTransaction(Transaction transaction);

        [OperationContract]
        List<Transaction> GetAllTransactions();


        [OperationContract]
        List<Transaction> GetAllTransactionsOfAccount(string account_id, DateTime from, DateTime to);

        [OperationContract]
        bool UpdateTransaction(int transaction_id, Transaction transaction);

        [OperationContract]
        bool DeleteTransaction(int transaction_id);

        #endregion

    }
}
