using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Loan
    {
        int id;
        int bank_id;
        string lender_jmbg;
        string name;
        long amount;
        int interest;
        string account_number;
        float balance;

        public Loan(int id, int bank_id, string lender_jmbg, string name, int amount, int interest, string account_number, float balance)
        {
            this.id = id;
            this.bank_id = bank_id;
            this.lender_jmbg = lender_jmbg;
            this.name = name;
            this.amount = amount;
            this.interest = interest;
            this.account_number = account_number;
            this.balance = balance;
        }

        public Loan()
        {
            this.id = 0;
            this.bank_id = 0;
            this.lender_jmbg = "";
            this.name = "";
            this.amount = 0;
            this.interest = 0;
            this.account_number = "";
            this.balance = 0;
        }

        [DataMember]
        public int ID { get => id; set => this.id = value; }
        [DataMember]
        public int BankID { get => bank_id; set => this.bank_id = value; }
        [DataMember]
        public string LenderJMBG { get => lender_jmbg; set => lender_jmbg = value; }
        [DataMember]
        public string Name { get => name; set => this.name = value; }
        [DataMember]
        public long Amount { get => amount; set => this.amount = value; }
        [DataMember]
        public int Interest { get => interest; set => this.interest = value; }
        [DataMember]
        public string AccountNumber { get => account_number; set => this.account_number = value; }
        [DataMember]
        public float Balance { get => balance; set => this.balance = value; }


        public Loan(Dictionary<string, object> v)
        {
            this.id = (int)v["id_kredita"];
            this.bank_id = (int)v["id_banke"];
            this.lender_jmbg = (string)v["jmbg_korisnika"];
            this.name = (string)v["naziv_kredita"];
            this.amount = (long)v["suma"];
            this.interest = (int)v["kamata"];
            this.account_number = (string)v["broj_racuna"];
            this.balance = Convert.ToSingle(v["stanje"]);
        }
    }
}
