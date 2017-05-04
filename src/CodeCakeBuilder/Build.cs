using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Core;
using Cake.Core.IO;
using CodeCake;
using System.IO;
using System.IO.Compression;

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
                DirectoryPathCollection AllProj = Cake.GetDirectories("./*", p => !p.Path.FullPath.Contains("CodeCakeBuilder") && !p.Path.FullPath.Contains("wwwroot") && !p.Path.FullPath.Contains("Omega.DataManager.Test"));
                    foreach (DirectoryPath proj in AllProj)
                    {
                        Cake.DotNetCoreBuild(proj.FullPath);
                    }
                });

            Task( "Unit-Tests" )
                .IsDependentOn( "Build" )
                .Does( () =>
                {
                    var testProjects = Cake.GetDirectories( "./*.Tests" );
                    foreach( var project in testProjects ) Cake.DotNetCoreRun( project.FullPath );
                } );

            Task( "Create-Zip-To-Deploy" )
                .IsDependentOn( "Unit-Tests" )
                .Does( () =>
                {
                    CopyPasteHelper.DirectoryCopy( ".", "./CodeCakeBuilder/Release/OmegaProd", true );
                    if( Cake.FileExists( "./CodeCakeBuilder/Release/OmegaProd.zip" ) )
                        File.Delete( "./CodeCakeBuilder/Release/OmegaProd.zip" );
                    ZipFile.CreateFromDirectory( "./CodeCakeBuilder/Release/OmegaProd", "./CodeCakeBuilder/Release/OmegaProd.zip" );
                    Directory.Delete( "./CodeCakeBuilder/Release/OmegaProd", true );
                } );

            // The Default task for this script can be set here.
            Task( "Default" )
                .IsDependentOn( "Create-Zip-To-Deploy" );
        }
    }
}
