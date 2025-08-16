
using ResolveDependencies;

if (args.Length == 0)
{
    Console.WriteLine("Please provide an input directory path.");
    return;
}

string inputDirectoryPath = args[0];

var resolver = new Resolver();
var fileProcessor = new FileProcessor(resolver);

fileProcessor.ProcessDirectory(inputDirectoryPath);