using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.ServiceModel;
using System.Security.Principal;

namespace ClientConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBank> ch = new ChannelFactory<IBank>(new BasicHttpBinding(),
                new EndpointAddress("http://localhost:8000"));
            IBank proxy = ch.CreateChannel();

            //Account account = new Account();
            //account.AccountNumber = "4-18-5";
            //account.BranchId = 1;
            //account.BankId = 2;
            //account.LenderJMBG = "123456";
            //account.Balance = 1521;

            //try
            //{
            //    proxy.CreateAccount(account);
            //}
            //catch (FaultException<CException> ex)
            //{
            //    Console.WriteLine(ex.Detail.Reason);
            //}

            //Bank bank = new Bank();
            //bank.ID = 101;
            //bank.Name = "Test";
            //bank.City = "City";

            //try
            //{
            //    proxy.CreateBank(bank);
            //}
            //catch (FaultException<CException> ex)
            //{
            //    Console.WriteLine(ex.Reason);
            //}

            //Branch branch = new Branch();

            //branch.BankID = 3;
            //branch.Name = "TestBranch";
            //branch.Address = "TestAdress";

            //try
            //{
            //    proxy.CreateBranch(branch);
            //}
            //catch (FaultException<CException> ex)
            //{
            //    Console.WriteLine(ex.Reason);
            //}

            //User user = new User();
            //user.JMBG = "15423";
            //user.Ime = "Aleksandar";
            //user.Prezime = "Balanesko";
            //user.Email = "email";
            //user.PasswordHash = "password";
            //user.Privilage = 0;

            //try
            //{
            //    proxy.CreateUser(user);
            //}
            //catch (FaultException<CException> ex)
            //{
            //    Console.WriteLine(ex.Detail.Reason);
            //}

            //Transaction transaction = new Transaction();

            //transaction.SenderAccountID = "3-08-1";
            //transaction.SenderBranchID = 1;
            //transaction.SenderBankID = 2;

            //transaction.ReciverAccountID = "4-18-5";
            //transaction.ReciverBranchID = 1;
            //transaction.ReciverBankID = 2;

            //transaction.TransactionType = "transfer";
            //transaction.Amount = 100;

            //transaction.TransactionDate = DateTime.Today;
            //transaction.Description = "Description";
            //transaction.BankFeeProcentage = 0.5f;


            //try
            //{
            //    proxy.CreateTransaction(transaction);
            //}
            //catch (FaultException<CException> ex)
            //{
            //    Console.WriteLine(ex.Detail.Reason);
            //}

            //Loan loan = new Loan();
            //loan.BankID = 2;
            //loan.LenderJMBG = "123456";
            //loan.Name = "name";
            //loan.Amount = 10000;
            //loan.Interest = 5;

            //try
            //{
            //    proxy.CreateLoan(loan);
            //}
            //catch (FaultException<CException> ex)
            //{
            //    Console.WriteLine(ex.Detail.Reason);
            //}

            //if (proxy.DeleteLoan(3))
            //{
            //    Console.WriteLine("yes");
            //}
            //Console.WriteLine("no");


            proxy.GetAllAccountsOfUser("00").ForEach(user => { Console.WriteLine(user.Balance); });


            Console.ReadLine();
        }
    }
}
