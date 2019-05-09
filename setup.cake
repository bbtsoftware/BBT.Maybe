#load nuget:?package=Cake.Recipe&version=1.0.0

#tool nuget:?package=inheritdoc&version=2.2.0

//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////

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
    shouldRunCodecov: true,
    shouldDeployGraphDocumentation: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/src/BBT.MaybePattern.Tests/*.cs" },
    testCoverageFilter: "+[*]* -[xunit.*]* -[*.Tests]* -[Shouldly]*",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

//////////////////////////////////////////////////////////////////////
// CUSTOM TASKS
//////////////////////////////////////////////////////////////////////

Task("Rewrite-Inheritdoc")
    .IsDependentOn("DotNetCore-Build")
    .Does(() =>
{
    var sourceDirectory = MakeAbsolute(Directory("./")).Combine("src");
    StartProcess(
        "./tools/InheritDoc.2.2.0/tools/InheritDoc.exe",
        new ProcessSettings
        {
            Arguments = "-f BBT.Maybe.* -b " + sourceDirectory.FullPath + " -o",
            WorkingDirectory = sourceDirectory.FullPath
        });
});

BuildParameters.Tasks.CreateNuGetPackagesTask
    .IsDependentOn("Rewrite-Inheritdoc");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

Build.RunDotNetCore();