## Testreporter

Testreporter is a tool to quickly run a set of performance tests that I made for my GSoC 2014 project on improving switch statement performance in the mono C# compiler.
Can a set of tests multiple times and has the options to select the compiler and runtime.

### How to run

> TestReporter.exe -t:[Target] -c:[Compiler] -n:[runs] -r:

**Options**

* -t: .cs file to compile.
* -c: compiler to use can be mcs or csc. Compiler should be on the path. Default is csc.exe
* -n: number of times that the test should be run. Default is 10.
* -r: runtime in which the tests are run, can be mono or ms. Default is ms.

All options that have a default can be omitted. Output of the test is written to result.txt in the current directory.