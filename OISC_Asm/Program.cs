using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OISC_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                DisplayUsage(); 
                return;
            }

            String filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("{0} not found", filePath);
                return;
            }

            // Load the source.
            Console.WriteLine(Strings.AppName);
            Console.WriteLine();
            Console.WriteLine("Loading source ({0})", filePath);
            String[] sourceCodeLines = File.ReadAllLines(filePath);

            // Compile source.
            OISCAsm compiler = new OISCAsm(sourceCodeLines);
            byte[] compiledBytes = compiler.Compile();

            // Output compiled binary to file.
            String outputFilePath = GenerateOutputFilename(filePath);
            File.WriteAllBytes(outputFilePath, compiledBytes);
        }

        private static String GenerateOutputFilename(String inputFileName)
        {
            String nameWithoutExtension = Path.GetFileNameWithoutExtension(inputFileName);
            String path = Path.GetDirectoryName(inputFileName);

            return Path.Combine(path, nameWithoutExtension + ".bin");
        }

        private static void DisplayUsage()
        {
            Console.WriteLine(Strings.AppName);
            Console.WriteLine();
            Console.WriteLine(Strings.Usage);
        }
    }
}
