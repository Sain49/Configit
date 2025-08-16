# Configit
# Dependency Resolver

This project is a C# console application that resolves software package dependencies to detect version conflicts.

## Description

The program analyzes a set of packages to be installed along with their dependencies. It determines if the installation is valid by checking for version conflicts. A conflict occurs if two different versions of the same package are required at any point in the dependency chain.

## How to Run

The program is a console application that requires the .NET SDK to build and run.

1.  **Build the project:**
    ```bash
    dotnet build
    ```
2.  **Run the program:**
    The program takes a single command-line argument: the path to a directory containing the input test files.
    ```bash
    dotnet run --project ResolveDependencies/ResolveDependencies.csproj -- "path/to/your/input/folder"
    ```
    For example:
    ```bash
    dotnet run --project ResolveDependencies/ResolveDependencies.csproj -- "C:\TestFiles"
    ```

## Input File Format

The program processes all `.txt` files in the specified input directory. Each file must follow this format:

*   **Line 1:** An integer `N`, the number of packages to install.
*   **Next `N` lines:** The packages to install, one per line, in the format `package_name,version`.
*   **Next line:** An integer `M`, the number of dependency rule lines.
*   **Following `M` lines:** The dependency rules. Each line defines the dependencies for one package. The format is flexible: `p1,v1,p2,v2,p3,v3,...`, meaning package `p1` at `v1` depends on `p2` at `v2`, `p3` at `v3`, and so on.

**Example Input (`test1.txt`):**
```
1
B,2
2
B,2,A,1,C,1
C,1,A,2
```

## Output

For each input file (e.g., `test1.txt`), the program will generate a corresponding output file in the same directory with a `.out.txt` extension (e.g., `test1.out.txt`).

The output file will contain a single word:
*   `PASS`: If the dependencies can be resolved without any version conflicts.
*   `FAILED`: If a version conflict is found or if the input file is malformed.
