using System;
using System.IO;
using DirBuilder.DTO;

namespace DirBuilder
{
    public class DirectoryBuilder
    {
        private readonly string _basePath;
        private readonly DirectoryDTO _rootDirectory;
        private DirectoryDTO _currentDirectory;

        public DirectoryBuilder(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException(basePath);
            }

            var info = new DirectoryInfo(basePath);

            _basePath = basePath;
            _rootDirectory = new DirectoryDTO(info.Name, null);
            _currentDirectory = _rootDirectory;
        }

        private void ValidateDirectoryName(string name)
        {

        }

        private void ValidateFileName(string name)
        {

        }

        public DirectoryDTO AddSubdirectoryAndStay(string name)
        {
            ValidateDirectoryName(name);
            _currentDirectory.AddSubdirectory(name);
            return _currentDirectory;
        }

        public DirectoryDTO AddSubdirectoryAndEnter(string name)
        {
            ValidateDirectoryName(name);
            _currentDirectory = _currentDirectory.AddSubdirectory(name);
            return _currentDirectory;
        }

        public DirectoryDTO AddFile(string name) 
        {
            ValidateFileName(name);
            _currentDirectory.AddFile(name);
            return _currentDirectory;
        }

        private void CreateRecursive(DirectoryDTO dto)
        {
            var fullPath = Path.Combine(_basePath, dto.GetFullPath());

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            foreach (var file in dto.Files)
            {
                var fullFilePath = Path.Combine(_basePath, file.GetFullPath());
                
                if (!File.Exists(fullFilePath))
                {
                    File.Create(fullFilePath);
                }
            }

            foreach (var subDirectory in dto.Subdirectories)
            {
                CreateRecursive(subDirectory);
            }
        }

        public void Create()
        {
            CreateRecursive(_rootDirectory);
        }
    }
}
