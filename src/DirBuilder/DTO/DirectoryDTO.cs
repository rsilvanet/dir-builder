using System;
using System.Collections.Generic;

namespace DirBuilder.DTO
{
    public class DirectoryDTO
    {
        public DirectoryDTO()
        {
            Subdirectories = new List<DirectoryDTO>();
            Files = new List<FileDTO>();
        }
        
        public string Name { get; set; }
        public DirectoryDTO Parent { get; set; }
        public IList<DirectoryDTO> Subdirectories { get; private set; }
        public IList<FileDTO> Files { get; private set; }

        public string GetFullPath()
        {
            return PathHelper.GetFullPath(this);
        }

        public void AddSubdirectory(string name)
        {
            Subdirectories.Add(new DirectoryDTO()
            {
                Name = name,
                Parent = this
            });
        }

        public void AddFile(string name)
        {
            Files.Add(new FileDTO() 
            {
                Name = name,
                Directory = this
            });
        }
    }
}
