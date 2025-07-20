using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[DotNetVerbosityMapping]
[ShutdownDotNetAfterServerBuild]
[AppVeyor(AppVeyorImage.VisualStudioLatest, InvokedTargets = [nameof(Test), nameof(Pack)])]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution = null!;
    [GitVersion(Framework = "net9.0")] readonly GitVersion GitVersion = null!;

    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            foreach (var dir in RootDirectory.GlobDirectories("**/bin", "**/obj").Except(RootDirectory.GlobDirectories("build/**")))
                dir.CreateOrCleanDirectory();
            OutputDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVersion(GitVersion.FullSemVer)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());

            var unexpectedPackages = Solution.AllProjects
                .Where(p => !ExpectedPackages.Contains(p.Name)
                    && p.GetMSBuildProject().GetPropertyValue("IsPackable").Equals("true", StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (unexpectedPackages.Any())
            {
                Log.Error("Unexpected packages: {Packages}", unexpectedPackages.Select(p => p.Name));
                Assert.Fail("Unexpected packages");
            }
        });

    Target Pack => _ => _
        .DependsOn(Clean, Compile)
        .After(Test)
        .Produces(OutputDirectory  / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetVersion(GitVersion.FullSemVer)
                .SetOutputDirectory(OutputDirectory)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    static readonly string[] ExpectedPackages =
        [
            "SlackNet",
            "SlackNet.AspNetCore",
            "SlackNet.Autofac",
            "SlackNet.AzureFunctions",
            "SlackNet.Bot",
            "SlackNet.Extensions.DependencyInjection",
            "SlackNet.SimpleInjector"
        ];
}