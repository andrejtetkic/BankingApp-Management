using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class User
    {
        string jmbg;
        string ime;
        string prezime;
        string email;
        string password_hash;
        int privilage;

        public User(string jmbg, string ime, string prezime, string email, string password_hash, int privilage)
        {
            this.jmbg = jmbg;
            this.ime = ime;
            this.prezime = prezime;
            this.email = email;
            this.password_hash = password_hash;
            this.privilage = privilage;
        }

        public User()
        {
            this.jmbg = "";
            this.ime = "";
            this.prezime = "";
            this.email = "";
            this.password_hash = "";
            this.privilage = 1;
        }


        [DataMember]
        public string JMBG { get => jmbg; set => jmbg = value; }
        [DataMember]
        public string Ime { get => ime; set => ime = value; }
        [DataMember]
        public string Prezime { get => prezime; set => prezime = value; }
        [DataMember]
        public string Email { get => email; set => email = value; }
        [DataMember]
        public string PasswordHash { get => password_hash; set => password_hash = value; }
        [DataMember]
        public int Privilage { get => privilage; set => privilage = value; }

        public User(Dictionary<string, object> v)
        {
            this.jmbg = (string)v["jmbg_korisnika"];
            this.ime = (string)v["ime"];
            this.prezime = (string)v["prezime"];
            this.email = (string)v["email"];
            this.password_hash = (string)v["password"];
            this.privilage = (int)v["privileges"];
        }
    }
}
