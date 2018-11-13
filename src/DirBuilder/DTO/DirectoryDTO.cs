using System;
using System.Collections.Generic;
using System.IO;

namespace DirBuilder.DTO
{
    public class DirectoryDTO
    {
        public DirectoryDTO(string name, DirectoryDTO parent) : base()
        {
            Name = name;
            Parent = parent;
            Subdirectories = new List<DirectoryDTO>();
            Files = new List<FileDTO>();
        }

        public string Name { get; private set; }
        public DirectoryDTO Parent { get; private set; }
        public IList<DirectoryDTO> Subdirectories { get; private set; }
        public IList<FileDTO> Files { get; private set; }

        public string GetFullPath()
        {
            return PathHelper.GetFullPath(this);
        }

        public DirectoryDTO AddSubdirectory(string name)
        {
            var dto = new DirectoryDTO(name, this);
            Subdirectories.Add(dto);
            return dto;
        }

        public FileDTO AddFile(string name)
        {
            var dto = new FileDTO(name, this);
            Files.Add(dto);
            return dto; 
        }
    }
}
