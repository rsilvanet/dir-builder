using System;
using System.IO;

namespace DirBuilder.DTO
{
    public class FileDTO
    {
        public FileDTO(string name, DirectoryDTO directory)
        {
            Name = name;
            Directory = directory;
        }

        public string Name { get; private set; }
        public DirectoryDTO Directory { get; private set; }

        public string GetFullPath()
        {
            return PathHelper.GetFullPath(this);
        }
    }
}
