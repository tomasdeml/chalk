using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Chalk.VaultExport
{
    public class DirectoryCleaner
    {
        const string HiddenNamePrefix = ".";

        readonly IFileSystem fileSystem;

        public DirectoryCleaner(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void DeleteContents(string path)
        { 
            DirectoryInfoBase directory = fileSystem.DirectoryInfo.FromDirectoryName(path);

            foreach (FileInfoBase file in GetDeletableFiles(directory))
                file.Delete();

            foreach (DirectoryInfoBase childDirectory in GetDeletableDirectories(directory))
            { 
                DeleteContents(childDirectory.FullName); 
                childDirectory.Delete();
            }
        }

        static IEnumerable<FileInfoBase> GetDeletableFiles(DirectoryInfoBase parentDirectory)
        {
            return parentDirectory.GetFiles().Where(IsDeletable);
        }

        static IEnumerable<DirectoryInfoBase> GetDeletableDirectories(DirectoryInfoBase parentDirectory)
        {
            return parentDirectory.GetDirectories().Where(IsDeletable);
        }

        static bool IsDeletable(FileSystemInfoBase fileSystemObject)
        {
            return !IsHiddenByAttributes(fileSystemObject.Attributes) && !IsHiddenByName(fileSystemObject.Name);
        }

        static bool IsHiddenByAttributes(FileAttributes attributes)
        {
            return attributes.HasFlag(FileAttributes.Hidden);
        }

        static bool IsHiddenByName(string name)
        {
            return name.StartsWith(HiddenNamePrefix);
        }
    }
}
