using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    public class CException
    {
        string reason;

        [DataMember]
        public string Reason { get => reason; set => reason = value; }

        public CException(string razlog)
        {
            this.Reason = razlog;
        }

        public CException()
        {
            this.Reason = "Error!";
        }
    }
}
