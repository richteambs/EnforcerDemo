# EnforcerDI

This is a console app that benchmarks [Enforcer](https://www.identityserver.com/documentation/enforcer/)
against a hard-coded policy engine. It uses [BenchmarkDotNet](https://benchmarkdotnet.org/) to execute
multiple benchmarks for comparison.

## Benchmark description

There are four benchmarks in total, two that execute Enforcer and two that execute the hard-coded
policy. To attempt to represent policies of differing complexity, each pair has two variants. One variant
executes as a 'standard' user, who has access to all resources. The other variant executes as a 'restricted'
user, who has access to a subset of resources for which the resource's Group ID is assigned to the permitted
groups for that user.

Both the hard-coded and Enforcer policies are optimised for rapid approval if the user is not restricted.

## Running benchmarks

To run the benchmarks, enter the following commands at a command prompt, starting in
the solution root directory:

```powershell
cd ./EnforcerDI
dotnet run --configuration release --filter *PolicyBenchmarks.* --memory
```

Note: all the standard [BenchmarkDotNet command-line parameters](https://benchmarkdotnet.org/articles/guides/console-args.html) are available.
