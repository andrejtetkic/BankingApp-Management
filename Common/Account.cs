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
        string proper_account_number;
        int branch_id;
        int bank_id;
        string lender_jmbg;
        float balance;

        public Account(string account_number, int branch_id, int bank_id, string lender_jmbg, float balance)
        {
            this.account_number = account_number;
            this.proper_account_number = $"{account_number[0]}-{account_number.Substring(1, 2)}-{account_number[3]}";
            this.branch_id = branch_id;
            this.bank_id = bank_id;
            this.lender_jmbg = lender_jmbg;
            this.balance = balance;
        }

        public Account()
        {
            this.account_number = "";
            this.proper_account_number = $"{this.account_number[0]}-{this.account_number.Substring(1, 2)}-{this.account_number[3]}"; ;
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
        public float Balance { get => balance; set => balance = value; }
        [DataMember]
        public string ProperAccountNumber { get => proper_account_number; set => proper_account_number = value; }

        public Account(Dictionary<string, object> v)
        {
            this.account_number = (string)v["broj_racuna"];
            this.proper_account_number = $"{this.account_number[0]}-{this.account_number.Substring(1, 2)}-{this.account_number[3]}";
            this.branch_id = (int)v["id_filijale"];
            this.bank_id = (int)v["id_banke"];
            this.lender_jmbg = (string)v["jmbg_korisnika"];
            this.balance = Convert.ToSingle(v["stanje"]);
        }

        public static string UnFormatID(string formattedId)
        {
            return formattedId.Replace("-", "");
        }
    }
}
