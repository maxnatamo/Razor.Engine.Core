using System.Collections.Generic;

using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    [Parameter("NuGet package endpoint for nuget.org")]
    readonly string NugetSource = "https://api.nuget.org/v3/index.json";

    [Secret]
    [Parameter("NuGet API key for authorization for nuget.org")]
    readonly string NugetApiKey;

    Target Pack => _ => _
        .DependsOn(Compile, Format, Test)
        .Produces(NuGetArtifactsDirectory / "*.nupkg")
        .Produces(NuGetArtifactsDirectory / "*.snupkg")
        .Requires(() => Configuration.IsRelease)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(MainSolutionFile)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetIncludeSymbols(false)
                .SetIncludeSource(false)
                .SetDescription("Razor Templating Engine for .NET Core")
                .SetPackageTags("razor template templating c# core library")
                .SetPackageProjectUrl("https://github.com/maxnatamo/RazorEngineCore")
                .SetNoDependencies(false)
                .SetOutputDirectory(NuGetArtifactsDirectory)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetVersion(GitVersion.SemVer));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Requires(() => NugetSource)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.IsRelease)
        .Executes(() =>
        {
            IReadOnlyCollection<AbsolutePath> packages = NuGetArtifactsDirectory.GlobFiles("*.nupkg");

            foreach(AbsolutePath package in packages)
            {
                DotNetNuGetPush(c => c
                    .SetTargetPath(package)
                    .SetSource(NugetSource)
                    .SetApiKey(NugetApiKey)
                    .EnableSkipDuplicate());
            }
        });
}