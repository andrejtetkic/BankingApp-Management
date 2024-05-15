using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    internal class Bank
    {
        int id;
        string name;
        string city;

        public Bank(int id, string name, string city)
        {
            this.id = id;
            this.name = name;
            this.city = city;
        }

        public Bank()
        {
            this.id = 0;
            this.name = "";
            this.city = "";
        }

        [DataMember]
        public int ID { get =>  id; set => id = value; }
        [DataMember]
        public string Name { get => name; set => name = value; }
        [DataMember]
        public string City { get => city; set => city = value; }
    }
}
