using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    public class Exception
    {
        string reason;

        [DataMember]
        public string Reason { get => reason; set => reason = value; }

        public Exception(string razlog)
        {
            this.Reason = razlog;
        }

        public Exception()
        {
            this.Reason = "";
        }
    }
}
