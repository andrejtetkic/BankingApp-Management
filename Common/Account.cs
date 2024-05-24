using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Account
    {
        string account_number;
        int branch_id;
        int bank_id;
        string lender_jmbg;
        long balance;

        public Account(string account_number, int branch_id, int bank_id, string lender_jmbg, int balance)
        {
            this.account_number = account_number;
            this.branch_id = branch_id;
            this.bank_id = bank_id;
            this.lender_jmbg = lender_jmbg;
            this.balance = balance;
        }

        public Account()
        {
            this.account_number = "";
            this.branch_id = 0;
            this.bank_id = 0;
            this.lender_jmbg = "";
            this.balance = 0;
        }

        [DataMember]
        public string AccountNumber { get => account_number; set => account_number = value; }
        [DataMember]
        public int BranchId { get => branch_id; set => branch_id = value; }
        [DataMember]
        public int BankId { get => bank_id; set => bank_id = value; }
        [DataMember]
        public string LenderJMBG { get => lender_jmbg; set => lender_jmbg = value; }
        [DataMember]
        public long Balance { get => balance; set => balance = value; }
    }
}
