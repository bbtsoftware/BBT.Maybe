#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context, 
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "BBT.Maybe",
    repositoryOwner: "bbtsoftware",
    repositoryName: "BBT.Maybe",
    appVeyorAccountName: "BBTSoftwareAG",
    shouldPublishMyGet: false,
    shouldRunCodecov: false,
    shouldDeployGraphDocumentation: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/src/BBT.Maybe.Tests/*.cs" },
    testCoverageFilter: "+[*]* -[xunit.*]* -[*.Tests]* ",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();
