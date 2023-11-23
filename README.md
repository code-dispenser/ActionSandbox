[![.NET](https://github.com/code-dispenser/ActionSandbox/actions/workflows/dotnet.yml/badge.svg)](https://github.com/code-dispenser/ActionSandbox/actions/workflows/dotnet.yml) [![Coverage Status](https://coveralls.io/repos/github/code-dispenser/ActionSandbox/badge.svg?branch=main)](https://coveralls.io/github/code-dispenser/ActionSandbox?branch=main)
# ActionSandbox
This repo was added just for testing GitHub actions. Getting a coverage report badge was annoyingly confusing, not because it is hard, but because for dotnet it really is not very well documented. Everybody only seems to give you half of the solution and then you spend hours trying to figure out which bit you are getting wrong.

For dotnet apps using XUnit or MSTest then this is what works and is used in this test repo. Here are the issues I faced.
1. I had no idea what format Coveralls expected and how to give it to them.
2. I never paid much attention to the coverlet.collector being added to my Test projects references.
3. I was not sure whether it was this coverlet.collector or the coverlet.msbuild package that I needed to use.
4. I had no idea of any of the commands I needed, the list goes on.

Once I figured out that coverlet.collector is quite generic and it was all I needed i.e no coverlet.msbuild, a quick look on the coverlet.collector site gave me the first command I needed to add to the github action.
The default run: dot net test became:

**run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory TestResults**

The above line just tells the coverlett.collector to output all coverage reports (**'coverage.cobertura.xml'**) for all tests in all projects. The **--restuls.directory** just tells it to make a directory called **TestResults** in the root of solution and to put all of the files in there instead of creating a default TestResults folder for each test project. I generally have sepearate test projects for Unit Tests, Integration Tests etc, so in my case there would be mulitple **'coverage.cobertura.xml'** files all in the one root folder **TestResults** instead of multiple folders each with a **'coverage.cobertura.xml'** file.

This following statement on the coverlet github site was extremely helpful and very important, it read:

*NB: By design VSTest platform will create your file under a random named folder(guid string) so if you need stable path to load file to some gui report system(i.e. coveralls, codecov, reportgenerator etc..) that doesn't support glob patterns or hierarchical search, you'll need to manually move resulting file to a predictable folder*

The above meant that at this stage I would have a **TestResults** folder with two guid named folders each containing **'coverage.cobertura.xml'** file.
I was/still am under the impression that Coveralls.io would not find the xml files and if they did, they would not read them so I needed to merge the files into one directory and put them in a format that I thought Coveralls.io would read.

By far the biggest help for me was when i found https://github.com/danielpalme/ReportGenerator that I could use to get the output files from the **TestResults** folder and put them in another root folder in a format that Coveralls.io would understand. What was even better was that Daniel has a page that helps you configure stuff that you can then paste into your github worflow file, https://reportgenerator.io/usage (make sure to look on the lefthand side for GitHub Action)).

I then pretty much just added the copied configuration tool output to my workflow file, the full file now read:

```
name: .NET

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:
  build:

    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore
    - name: Test
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory TestResults
    - name: ReportGenerator
      uses: danielpalme/ReportGenerator-GitHub-Action@5.2.0
      with:
       reports: TestResults/**/coverage.cobertura.xml
       targetdir: CoverageResults
       reporttypes: Html;lcov
    - name: Coveralls GitHub Action
      uses: coverallsapp/github-action@v2.2.3
```
Please note the **-name: Coveralls GithHub Actions** and the line below it were the copy and paste from the coveralls integration page. 
 
This repo was just for a test as I did not want to try all of this on my other repo and break stuff without knowing why things were breaking. I was going to delete this repo but then I thought I would leave it as a reminder to myself and any others who may also be banging there head against a brickwall trying to get the code coverage badge stuff to work.
