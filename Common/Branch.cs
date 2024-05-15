using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    internal class Branch
    {
        int id;
        int bank_id;
        string name;
        string address;

        public Branch(int id, int bank_id, string name, string address)
        {
            this.id = id;
            this.bank_id = bank_id;
            this.name = name;
            this.address = address;
        }

        public Branch()
        {
            this.id = 0;
            this.bank_id = 0;
            this.name = "";
            this.address = "";
        }

        [DataMember]
        public int ID { get => id; set => id = value; }
        [DataMember]
        public int BankID { get => bank_id; set => bank_id = value; }
        [DataMember]
        public string Name { get => name; set => name = value; }
        [DataMember]
        public string Address { get => address; set => address = value; }
    }
}
