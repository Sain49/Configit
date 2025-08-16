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
            string outputFilePath = Path.ChangeExtension(inputFilePath, ".out.txt");
            try
            {
                var lines = File.ReadAllLines(inputFilePath);
                var (initialPackages, dependencies) = ParseInput(lines);

                bool isValid = _resolver.IsValid(initialPackages, dependencies);
                string result = isValid ? "PASS" : "FAILED";
                File.WriteAllText(outputFilePath, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to process file '{inputFilePath}': {ex.Message}");
                File.WriteAllText(outputFilePath, "FAILED");
            }
        }

        private (List<Package> initialPackages, Dictionary<Package, List<Package>> dependencies) ParseInput(string[] lines)
        {
            if (lines.Length == 0)
            {
                throw new FormatException("Input file cannot be empty.");
            }

            int lineIndex = 0;

            int numPackages = int.Parse(lines[lineIndex++]);
            var initialPackages = new List<Package>();
            for (int i = 0; i < numPackages; i++)
            {
                if (lineIndex >= lines.Length) throw new FormatException("Input file ended unexpectedly while reading packages.");
                var line = lines[lineIndex++];
                if (string.IsNullOrEmpty(line)) continue;
                var parts = line.Split(',');
                if (parts.Length != 2) throw new FormatException($"Package line has {parts.Length} parts, expected 2.");
                initialPackages.Add(new Package(parts[0], parts[1]));
            }

            var dependencies = new Dictionary<Package, List<Package>>();
            if (lineIndex >= lines.Length)
            {
                // No dependency section is a valid state.
                return (initialPackages, dependencies);
            }

            int numDependencies = int.Parse(lines[lineIndex++]);
            for (int i = 0; i < numDependencies; i++)
            {
                if (lineIndex >= lines.Length) throw new FormatException("Input file ended unexpectedly while reading dependencies.");
                var line = lines[lineIndex++];
                if (string.IsNullOrEmpty(line)) continue;

                var parts = line.Split(',');
                // A dependency line must have at least 4 parts (p1,v1,p2,v2) and an even number of parts
                if (parts.Length < 4 || parts.Length % 2 != 0)
                {
                    throw new FormatException($"Dependency line has {parts.Length} parts, expected an even number >= 4.");
                }

                var package = new Package(parts[0], parts[1]);
                if (!dependencies.ContainsKey(package))
                {
                    dependencies[package] = new List<Package>();
                }

                // Loop through the dependencies on the current line, which start at index 2
                for (int j = 2; j < parts.Length; j += 2)
                {
                    var dependency = new Package(parts[j], parts[j + 1]);
                    dependencies[package].Add(dependency);
                }
            }

            return (initialPackages, dependencies);
        }
    }
}
