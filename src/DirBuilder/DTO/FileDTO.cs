using System;
using System.IO;

namespace DirBuilder.DTO
{
    public class FileDTO
    {
        public string Name { get; set; }
        public DirectoryDTO Directory { get; set; }

        public string GetFullPath()
        {
            return PathHelper.GetFullPath(this);
        }
    }
}
