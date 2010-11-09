using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayUsage();

            Console.ReadLine();
        }

        private static void DisplayUsage()
        {
            Console.WriteLine(Strings.AppName);
            Console.WriteLine();
            Console.WriteLine(Strings.Usage);
        }
    }
}
