using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace Common
{
    [DataContract]
    public class Transaction
    {
        int transaction_id;

        string sender_account_id;
        int sender_branch_id;
        int sender_bank_id;

        string reciver_account_id;
        int reciver_branch_id;
        int reciver_bank_id;

        string transaction_type;
        float amount;
        DateTime transaction_date;
        string description;
        float bank_fee_procentage;
        string type;

        public Transaction(int transaction_id, string sender_account_id, int sender_branch_id, int sender_bank_id, string reciver_account_id, int reciver_branch_id, int reciver_bank_id, string transaction_type, float amount, DateTime transaction_date, string description, float bank_fee_procentage, string type)
        {
            this.transaction_id = transaction_id;
            this.sender_account_id = sender_account_id;
            this.sender_branch_id = sender_branch_id;
            this.sender_bank_id = sender_bank_id;
            this.reciver_account_id = reciver_account_id;
            this.reciver_branch_id = reciver_branch_id;
            this.reciver_bank_id = reciver_bank_id;
            this.transaction_type = transaction_type;
            this.transaction_date = transaction_date;
            this.description = description;
            this.bank_fee_procentage = bank_fee_procentage;
            this.amount = amount;
            this.type = type;
        }

        public Transaction()
        {
            this.transaction_id = -1;
            this.sender_account_id = "";
            this.sender_branch_id = -1;
            this.sender_bank_id = -1;
            this.reciver_account_id = "";
            this.reciver_branch_id = -1;
            this.reciver_bank_id = -1;
            this.transaction_type = "";
            this.transaction_date = DateTime.Today;
            this.description = "";
            this.bank_fee_procentage = 0;
            this.amount = 0;
            this.type = "";
        }

        [DataMember]
        public int TransactionID { get => transaction_id; set => transaction_id = value; }
        [DataMember]
        public string SenderAccountID { get => sender_account_id; set => sender_account_id = value; }
        [DataMember]
        public int SenderBranchID { get => sender_branch_id; set => sender_branch_id = value; }
        [DataMember]
        public int SenderBankID { get => sender_bank_id; set => sender_bank_id = value; }
        [DataMember]
        public string ReciverAccountID { get => reciver_account_id; set => reciver_account_id = value; }
        [DataMember]
        public int ReciverBranchID { get => reciver_branch_id; set => reciver_branch_id = value; }
        [DataMember]
        public int ReciverBankID { get => reciver_bank_id; set => reciver_bank_id = value; }
        [DataMember]
        public string TransactionType { get => transaction_type; set => transaction_type = value; }
        [DataMember]
        public DateTime TransactionDate { get => transaction_date; set => transaction_date = value; }
        [DataMember]
        public string Description { get => description; set => description = value; }
        [DataMember]
        public float BankFeeProcentage { get => bank_fee_procentage; set => bank_fee_procentage = value; }
        [DataMember]
        public float Amount { get => amount; set => amount = value; }
        [DataMember]
        public string Type { get => type; set => type = value; }

        public Transaction(Dictionary<string, object> v)
        {
            this.transaction_id = (int)v["TransactionId"];
            this.sender_account_id = (string)v["SenderAccountId"];
            this.sender_branch_id = (int)v["SenderBranchId"];
            this.sender_bank_id = (int)v["SenderBankId"];
            this.reciver_account_id = (string)v["ReceiverAccountId"];
            this.reciver_branch_id = (int)v["ReceiverBranchId"];
            this.reciver_bank_id = (int)v["ReceiverBankId"];
            this.transaction_type = (string)v["TransactionType"];
            this.transaction_date = (DateTime)v["TransactionDate"];
            this.description = (string)v["Description"];
            this.bank_fee_procentage = Convert.ToSingle(v["BankFeePercentage"]);
            this.amount = Convert.ToSingle(v["Amount"]);
            if(v.ContainsKey("type"))
            {
                this.type = (string)v["type"];
            } else
            {
                this.type = "";
            }
        }
    }
}
