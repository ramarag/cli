// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.Cli.Utils;
using NuGet.Frameworks;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public sealed class CacheCommand : TestCommand
    {
        private string _framework;
        private string _output;
        private string _runtime;

        public CacheCommand()
            : base("dotnet")
        {
        }

        public CacheCommand WithFramework(string framework)
        {
            _framework = framework;
            return this;
        }

        public CacheCommand WithFramework(NuGetFramework framework)
        {
            return WithFramework(framework.GetShortFolderName());
        }

        public CacheCommand WithOutput(string output)
        {
            _output = output;
            return this;
        }

        public CacheCommand WithRuntime(string runtime)
        {
            _runtime = runtime;
            return this;
        }

        public override CommandResult Execute(string args = "")
        {
            args = $"cache {BuildArgs()} {args}";
            return base.Execute(args);
        }

        public override CommandResult ExecuteWithCapturedOutput(string args = "")
        {
            args = $"cache {BuildArgs()} {args}";
            return base.ExecuteWithCapturedOutput(args);
        }

        private string BuildArgs()
        {
            return string.Join(" ", 
                FrameworkOption,
                OutputOption,
                RuntimeOption);
        }

        private string FrameworkOption => string.IsNullOrEmpty(_framework) ? "" : $"-f {_framework}";

        private string OutputOption => string.IsNullOrEmpty(_output) ? "" : $"-o {_output}";

        private string RuntimeOption => string.IsNullOrEmpty(_runtime) ? "" : $"-r {_runtime}";
    }
}
