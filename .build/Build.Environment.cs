using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitVersion;

partial class Build : NukeBuild
{
    /// <summary>
    /// Path to the main solution file of the project.
    /// </summary>
    readonly AbsolutePath MainSolutionFile = RootDirectory / "RazorEngineCore.sln";

    /// <summary>
    /// Path to store coverage reports.
    /// </summary>
    readonly AbsolutePath TestCoverageDirectory = RootDirectory / "coverage";

    /// <summary>
    /// Path to store local artifacts.
    /// </summary>
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "artifacts";

    /// <summary>
    /// Path to store local NuGet artifacts.
    /// </summary>
    readonly AbsolutePath NuGetArtifactsDirectory = RootDirectory / "artifacts" / "nuget";

    [GitVersion(Framework = "net6.0", NoFetch = true)]
    readonly GitVersion GitVersion;
}