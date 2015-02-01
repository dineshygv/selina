using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SelinaTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var testRunner = new MultiTestRunner();
            testRunner.RunTests();            
            Console.ReadKey();
        }
    }
}
