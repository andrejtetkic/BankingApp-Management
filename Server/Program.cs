using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(BankServer)))
            {
                host.AddServiceEndpoint(typeof(IBank),
                    new BasicHttpBinding(), new Uri("http://localhost:8000"));
                host.Open();
                Console.WriteLine("Servis pokrenut...");
                Console.ReadKey();
            }
        }
    }
}
