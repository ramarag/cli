// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Tools.MSBuild;

namespace Microsoft.DotNet.Tools.Cache
{
    public partial class CacheCommand
    {
        public static int Run(string[] args)
        {
            DebugHelper.HandleDebugSwitch(ref args);

            CommandLineApplication app = new CommandLineApplication(throwOnUnexpectedArg: false);
            app.Name = "dotnet cache";
            app.FullName = LocalizableStrings.AppFullName;
            app.Description = LocalizableStrings.AppDescription;
            app.AllowArgumentSeparator = true;
            app.ArgumentSeparatorHelpText = HelpMessageStrings.MSBuildAdditionalArgsHelpText;
            app.HelpOption("-h|--help");

            CommandArgument projectArgument = app.Argument($"<{LocalizableStrings.ProjectArgument}>",
                LocalizableStrings.ProjectArgDescription,
                true);

            CommandOption frameworkOption = app.Option(
                $"-f|--framework <{LocalizableStrings.FrameworkOption}>", LocalizableStrings.FrameworkOptionDescription,
                CommandOptionType.SingleValue);

            CommandOption runtimeOption = app.Option(
                $"-r|--runtime <{LocalizableStrings.RuntimeOption}>", LocalizableStrings.RuntimeOptionDescription,
                CommandOptionType.SingleValue);

            CommandOption outputOption = app.Option(
                $"-o|--output <{LocalizableStrings.OutputOption}>", LocalizableStrings.OutputOptionDescription,
                CommandOptionType.SingleValue);

            CommandOption fxOption = app.Option(
                $"--fx-version <{LocalizableStrings.FxVersionOption}>", LocalizableStrings.FxVersionOptionDescription,
                CommandOptionType.SingleValue);
            CommandOption verbosityOption = MSBuildForwardingApp.AddVerbosityOption(app);

            app.OnExecute(() =>
            {

                var publish = new CacheCommand();
                publish.ProjectPath = projectArgument.Value;
                publish.Framework = frameworkOption.Value();
                publish.Runtime = runtimeOption.Value();
                publish.OutputPath = outputOption.Value();
                publish.FxVersion = fxOption.Value();
                publish.Verbosity = verbosityOption.Value();
                publish.ExtraMSBuildArguments = app.RemainingArguments;

                return publish.Execute();
            });

            return app.Execute(args);
        }
    }
}
