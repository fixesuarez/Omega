using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.OpenCover;
using Cake.Core;
using Cake.Core.IO;
using CodeCake;

namespace CodeCakeBuilder
{
    public class Build : CodeCakeHost
    {
        public Build()
        {

            Task("Clean")
                .Does(() =>
                {
                    DirectoryPathCollection AllProj = Cake.GetDirectories("./*", p => !p.Path.FullPath.Contains("CodeCakeBuilder"));
                    foreach (DirectoryPath proj in AllProj)
                    {
                        if (Cake.DirectoryExists(proj + "/bin"))
                        {
                            Cake.DeleteDirectory(proj + "/bin", true);
                        }
                        if (Cake.DirectoryExists(proj + "/obj"))
                        {
                            Cake.DeleteDirectory(proj + "/obj", true);
                        }
                    }
                });

            Task("Restore")
                .IsDependentOn("Clean")
                .Does(() =>
                {
                    Cake.DotNetCoreRestore();
                });

            Task("Restore-Tools")
                .IsDependentOn("Restore")
                .Does(() =>
                {
                    DirectoryPath PackagesDir = new DirectoryPath("../packages");

                    DotNetCoreRestoreSettings dotNetCoreRestoreSettings = new DotNetCoreRestoreSettings();

                    dotNetCoreRestoreSettings.PackagesDirectory = PackagesDir;
                    dotNetCoreRestoreSettings.ArgumentCustomization = args => args.Append("./CodeCakeBuilder/project.json");

                    Cake.DotNetCoreRestore(dotNetCoreRestoreSettings);
                });

            Task("Build")
                .IsDependentOn("Restore-Tools")
                .Does(() =>
                {
                DirectoryPathCollection AllProj = Cake.GetDirectories("./*", p => !p.Path.FullPath.Contains("CodeCakeBuilder") && !p.Path.FullPath.Contains("wwwroot") && !p.Path.FullPath.Contains("Omega.DataManager.Tests"));
                    foreach (DirectoryPath proj in AllProj)
                    {
                        Cake.DotNetCoreBuild(proj.FullPath);
                    }
                });

            Task("Unit-Tests")
                .IsDependentOn("Build")
                .Does(() =>
                {
                    var testProjects = Cake.GetDirectories("./*.Tests");
                    foreach(var project in testProjects) Cake.DotNetCoreRun(project.FullPath);
                });

            // The Default task for this script can be set here.
            Task("Default")
                .IsDependentOn("Unit-Tests");
        }
    }
}
