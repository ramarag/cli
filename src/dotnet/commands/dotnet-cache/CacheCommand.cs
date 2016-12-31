﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Tools.MSBuild;
using Microsoft.DotNet.Tools.Restore;

namespace Microsoft.DotNet.Tools.Cache
{
    public partial class CacheCommand
    {
        public string ProjectPath { get; set; }
        public string Framework { get; set; }
        public string Runtime { get; set; }
        public string OutputPath { get; set; }
        public string Verbosity { get; set; }

        public List<string> ExtraMSBuildArguments { get; set; }

        private CacheCommand()
        {
        }

        public int Execute(List<string> msbuildArgs)
        {

            if (!string.IsNullOrEmpty(ProjectPath))
            {
                msbuildArgs.Add(ProjectPath);
            }

            msbuildArgs.Add("/t:ComposeCache");

            if (!string.IsNullOrEmpty(Framework))
            {
                msbuildArgs.Add($"/p:TargetFramework={Framework}");
            }

            if (!string.IsNullOrEmpty(Runtime))
            {
                msbuildArgs.Add($"/p:RuntimeIdentifier={Runtime}");
            }

            if (!string.IsNullOrEmpty(OutputPath))
            {
                msbuildArgs.Add($"/p:PublishDir={OutputPath}");
            }

            if (!string.IsNullOrEmpty(Verbosity))
            {
                msbuildArgs.Add($"/verbosity:{Verbosity}");
            }

            msbuildArgs.AddRange(ExtraMSBuildArguments);

            return new MSBuildForwardingApp(msbuildArgs).Execute();
        }
    }
}
