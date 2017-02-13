// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.Cli.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public static class TestCommandExtensions
    {
        public static TCommand SetUserProfile<TCommand>(this TCommand subject, string userProfileDirectory) where TCommand : TestCommand
        {
            string userProfileDirectoryName = "HOME";
            if (RuntimeEnvironment.OperatingSystemPlatform == Platform.Windows)
            {
                userDirectory = "USERPROFILE";
            }

            subject.WithEnvironmentVariable(userProfileDirectoryName, userProfileDirectory);
            return subject;
        }

        public static TCommand WithWorkingDirectory<TCommand>(this TCommand subject, string workingDirectory) where TCommand : TestCommand
        {
            subject.WorkingDirectory = workingDirectory;
            subject.SetUserProfile(Path.Combine(workingDirectory, "userprofile"));
            return subject;
        }

        public static TCommand WithWorkingDirectory<TCommand>(this TCommand subject, DirectoryInfo workingDirectory) where TCommand : TestCommand
        {
            subject.WorkingDirectory = workingDirectory.FullName;
            subject.SetUserProfile(Path.Combine(workingDirectory.ToString(), "userprofile"));
            return subject;
        }
        
        public static TCommand WithEnvironmentVariable<TCommand>(this TCommand subject, string name, string value) where TCommand : TestCommand
        {
            subject.Environment.Add(name, value);
            
            return subject;
        }
        
        public static TCommand WithOutputDataReceivedHandler<TCommand>(this TCommand subject, Action<string> writeLine) where TCommand : TestCommand
        {
            subject.OutputDataReceived += (s, e) => writeLine(e.Data);
            
            return subject;
        }
        
        public static TCommand WithErrorDataReceivedHandler<TCommand>(this TCommand subject, Action<string> writeLine) where TCommand : TestCommand
        {
            subject.ErrorDataReceived += (s, e) => writeLine(e.Data);
            
            return subject;
        }
        
        public static TCommand WithForwardingToConsole<TCommand>(this TCommand subject) where TCommand : TestCommand
        {
            subject.WithOutputDataReceivedHandler(s => Console.Out.WriteLine(s));

            subject.WithErrorDataReceivedHandler(s => Console.Error.WriteLine(s));
            
            return subject;
        }
    }
}
