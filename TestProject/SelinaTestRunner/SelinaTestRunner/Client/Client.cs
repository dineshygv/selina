using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelinaTestRunner
{
    class Client
    {
        public string Address {get; set;}
        public ClientStatus status { get; set; }

        public Client(string Address) {             
            this.Address = Address;
        }
    }

    public enum ClientStatus { 
        Free = 0,
        Busy = 1
    }
}
