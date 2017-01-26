namespace Microsoft.DotNet.Tools.Cache
{
    internal class LocalizableStrings
    {
        public const string AppFullName = ".NET Cache";

        public const string AppDescription = "Cahes Assemblies specified for the .NET Platform, By Default these will be optimized for the target runtime and framework";

        public const string ProjectEntries = "ProjectEntries";

        public const string ProjectEntryDescription = "The MSBuild project file used to specify the list of packages to be cached.";

        public const string FrameworkOption = "FRAMEWORK";

        public const string FrameworkOptionDescription = "Target framework for which to cache for";

        public const string RuntimeOption = "RUNTIME_IDENTIFIER";

        public const string RuntimeOptionDescription = "Target runtime to cache for.";

        public const string OutputOption = "OUTPUT_DIR";

        public const string OutputOptionDescription = "Path in which to cache the given assemblies";
    
        public const string FrameworkVersionOption = "FrameworkVersion";

        public const string FrameworkVersionOptionDescription = "The Microsoft.NETCore.App package version that will be used to run the assemblies";

        public const string SkipOptmizationOptionDescription = "This is will skip the optimization phase";

        public const string IntermediateWorkingDirOption = "IntermediateWorkingDir";

        public const string IntermediateWorkingDirOptionDescription = "The directory used by the command to do its work";

        public const string PreserveIntermediateWorkingDirOptionDescription = "The directory used by the command to do its work will not be removed";

        public const string SpecifyEntries = "Please specify atleast one entry with --entries";

        public const string IntermediateDirExists = "Intermediate working directory {0} exists, remove {0} or specify another directory with -w";
    }
}
