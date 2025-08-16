using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace ResolveDependencies
{
    public class FileProcessor
    {
        private readonly Resolver _resolver;
        public FileProcessor(Resolver resolver) { _resolver = resolver; }

        public void ProcessDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Input directory not found: {directoryPath}");
                return;
            }

            var inputFiles = Directory.GetFiles(directoryPath, "*.txt")
                .Where(file => !file.EndsWith(".out.txt"));

            foreach (var inputFile in inputFiles)
                ProcessFile(inputFile);

        }

        private void ProcessFile(string inputFilePath)
        {
           throw new NotImplementedException();
        }
    }
}
