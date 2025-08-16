using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveDependencies
{
    public class Resolver
    {
        public bool IsValid(List<Package> initialPackages, Dictionary<Package, List<Package>> dependencies)
        {
            var requiredPackages = new Dictionary<string, string>();
            var packagesToProcess = new Queue<Package>(initialPackages);

            while (packagesToProcess.Count > 0)
            {
                var currentPackage = packagesToProcess.Dequeue();

                if (requiredPackages.TryGetValue(currentPackage.Name, out var requiredVersion)){
                    if (requiredVersion != currentPackage.Version) 
                        return false; // conflict detected
                }
                else
                {
                    requiredPackages.Add(currentPackage.Name, currentPackage.Version);

                    if (dependencies.TryGetValue(currentPackage, out var packageDependencies))
                    {
                        foreach (var dependency in packageDependencies) 
                            packagesToProcess.Enqueue(dependency);
                    }
                }
            }

            return true;
        }
    }
}
