using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using Seterlund.CodeGuard;

namespace Chalk.VaultExport
{
    class FilePersistedLastVersionMarker : ILastVersionMarker
    {
        const int NonePlaceholderVersion = -1;
        const string MarkerFileName = ".chalk-lastversion";

        readonly string workspacePath;
        readonly IFileSystem fileSystem;

        public FilePersistedLastVersionMarker(string workspacePath, IFileSystem fileSystem)
        {
            Guard.That(workspacePath, "workspacePath").IsNotNullOrEmpty();
            Guard.That(fileSystem, "fileSystem").IsNotNull();
            this.workspacePath = workspacePath;
            this.fileSystem = fileSystem;
        }

        public int? GetNext()
        {
            int? version = ReadVersion();

            if (!CanGetNext(version)) 
                return null;

            return ++version;
        }

        static bool CanGetNext(int? version)
        {
            return version != null && version != NonePlaceholderVersion;
        }

        int? ReadVersion()
        {
            string markerPath = GetMarkerFilePath();

            if (!fileSystem.File.Exists(markerPath))
                return null;

            return int.Parse(fileSystem.File.ReadAllText(markerPath));
        }

        public void Mark(int version)
        {
            string markerPath = GetMarkerFilePath();
            fileSystem.File.WriteAllText(markerPath, version.ToString(CultureInfo.InvariantCulture)); 
        }

        string GetMarkerFilePath()
        {
            return Path.Combine(workspacePath, MarkerFileName);
        }

        public void MarkNone()
        {
            Mark(NonePlaceholderVersion);
        }

        public bool Exists()
        {
            string markerPath = GetMarkerFilePath();
            return fileSystem.File.Exists(markerPath);
        }
    }
}