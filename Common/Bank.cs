using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Bank
    {
        int id;
        string name;
        string city;
        int main_branch;

        public Bank(int id, string name, string city, int main_branch)
        {
            this.id = id;
            this.name = name;
            this.city = city;
            this.main_branch = main_branch;
        }

        public Bank()
        {
            this.id = 0;
            this.name = "";
            this.city = "";
            this.main_branch = 0;
        }

        [DataMember]
        public int ID { get =>  id; set => id = value; }
        [DataMember]
        public string Name { get => name; set => name = value; }
        [DataMember]
        public string City { get => city; set => city = value; }
        [DataMember]
        public int MainBranch { get => main_branch; set => main_branch = value; }

        public Bank(Dictionary<string, object> v)
        {
            this.id = (int)v["id_banke"];
            this.name = (string)v["naziv_banke"];
            this.city = (string)v["grad"];
            this.main_branch = (int)v["main_branch"];
        }
    }
}
