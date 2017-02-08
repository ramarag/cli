// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FluentAssertions;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.DotNet.Tools.Test.Utilities;
using Xunit;

namespace Microsoft.DotNet.Cli.Publish.Tests
{
    public class GivenDotnetPublishPublishesProjects : TestBase
    {
        [Fact]
        public void ItPublishesARunnablePortableApp()
        {
            var testAppName = "SimpleDependencies";
            var testInstance = TestAssetsManager
                .CreateTestInstance(testAppName);

            var testProjectDirectory = testInstance.TestRoot;
            var tfm = "netcoreapp2.0";
            var frameworkVersion = "2.0.0-*";
            var rid = DotnetLegacyRuntimeIdentifiers.InferLegacyRestoreRuntimeIdentifier();
            var localAssemblyCache = Path.Combine(testProjectDirectory, "localAssemblyCache");
            var workingDirectory = Path.Combine(testProjectDirectory, "workingDirectory");
            var profileProjectName = "NewtonsoftFilterProfile";
            var profileProject = Path.Combine(TestAssetsManager.AssetsRoot, profileProjectName, $"{profileProjectName}.csproj");

            new RestoreCommand()
                .WithWorkingDirectory(testProjectDirectory)
                .Execute("/p:SkipInvalidConfigurations=true")
                .Should().Pass();

            new CacheCommand()
                .WithWorkingDirectory(testProjectDirectory)
                .Execute($"--entries {profileProject} --framework {tfm} --runtime {rid} --output {localAssemblyCache} --working-dir {workingDirectory} --preserve-working-dir --framework-version {frameworkVersion}")
                .Should().Pass();

            var configuration = Environment.GetEnvironmentVariable("CONFIGURATION") ?? "Debug";
           
            new PublishCommand()
                .WithFramework(tfm)
                .WithWorkingDirectory(testProjectDirectory)
                //Workaround for https://github.com/dotnet/cli/issues/4501
                .WithEnvironmentVariable("SkipInvalidConfigurations", "true")
                .WithProFileProject(profileProject)
                .Execute("/p:SkipInvalidConfigurations=true")
                .Should().Pass();

            var outputDll = Path.Combine(testProjectDirectory, "bin", configuration, tfm, "publish", $"{testAppName}.dll");

            new TestCommand("dotnet")
                .WithEnvironmentVariable("DOTNET_SHARED_PACKAGES", localAssemblyCache)
                .ExecuteWithCapturedOutput(outputDll)
                .Should().Pass()
                .And.HaveStdOutContaining("{}");

            new TestCommand("dotnet")
                .ExecuteWithCapturedOutput(outputDll)
                .Should().Fail()
                .And.HaveStdErrContaining("assembly specified in the dependencies manifest was not found -- package: 'newtonsoft.json',");
        }

       // [Fact]`
        public void ItPublishesARunnableSelfContainedApp()
        {
            var testAppName = "MSBuildTestApp";

            var testInstance = TestAssets.Get(testAppName)
                .CreateInstance()
                .WithSourceFiles()
                .WithRestoreFiles();

            var testProjectDirectory = testInstance.Root;

            var rid = DotnetLegacyRuntimeIdentifiers.InferLegacyRestoreRuntimeIdentifier();

            new PublishCommand()
                .WithFramework("netcoreapp1.0")
                .WithRuntime(rid)
                .WithWorkingDirectory(testProjectDirectory)
                //Workaround for https://github.com/dotnet/cli/issues/4501
                .WithEnvironmentVariable("SkipInvalidConfigurations", "true")
                .Execute("/p:SkipInvalidConfigurations=true")
                .Should().Pass();

            var configuration = Environment.GetEnvironmentVariable("CONFIGURATION") ?? "Debug";

            var outputProgram = testProjectDirectory
                .GetDirectory("bin", configuration, "netcoreapp1.0", rid, "publish", $"{testAppName}{Constants.ExeSuffix}")
                .FullName;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //Workaround for https://github.com/dotnet/corefx/issues/15516
                Process.Start("chmod", $"u+x {outputProgram}").WaitForExit();
            }

            new TestCommand(outputProgram)
                .ExecuteWithCapturedOutput()
                .Should().Pass()
                     .And.HaveStdOutContaining("Hello World");
        }

       // [Fact]
        public void ItPublishesAppWhenRestoringToSpecificPackageDirectory()
        {
            var rootPath = TestAssetsManager.CreateTestDirectory().Path;
            var rootDir = new DirectoryInfo(rootPath);

            string dir = "pkgs";
            string args = $"--packages {dir}";

            new NewCommand()
                .WithWorkingDirectory(rootPath)
                .Execute()
                .Should()
                .Pass();

            new RestoreCommand()
                .WithWorkingDirectory(rootPath)
                .Execute(args)
                .Should()
                .Pass();

            new PublishCommand()
                .WithWorkingDirectory(rootPath)
                .ExecuteWithCapturedOutput()
                .Should().Pass();

            var configuration = Environment.GetEnvironmentVariable("CONFIGURATION") ?? "Debug";

            var outputProgram = rootDir
                .GetDirectory("bin", configuration, "netcoreapp2.0", "publish", $"{rootDir.Name}.dll")
                .FullName;

            new TestCommand(outputProgram)
                .ExecuteWithCapturedOutput()
                .Should().Pass()
                     .And.HaveStdOutContaining("Hello World");
        }
    }
}
