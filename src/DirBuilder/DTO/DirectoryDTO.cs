using System;

namespace DirBuilder
{
    public class DirectoryDTO
    {
        public string Name { get; set; }
        public DirectoryDTO Parent { get; set; }
    }
}
