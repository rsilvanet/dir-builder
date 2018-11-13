using System;
using System.IO;
using System.Linq;
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

        private void ValidateDirectoryName(string directoryName)
        {
            if (string.IsNullOrWhiteSpace(directoryName))
            {
                throw new ArgumentException(
                    "Directory name can't be null or empty", 
                    nameof(directoryName)
                );
            }

            if (directoryName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException(
                    "Directory name can't contains invalid chars", 
                    nameof(directoryName)
                );
            }
        }

        private void ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(
                    "File name can't be null or empty", 
                    nameof(fileName)
                );
            }

            if (fileName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException(
                    "File name can't contains invalid chars", 
                    nameof(fileName)
                );
            }
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

        public string DescribeRecursive(DirectoryDTO dto, int level, string content)
        {
            var fullPath = Path.Combine(_basePath, dto.GetFullPath());

            content += fullPath.PadLeft(fullPath.Length + level, '-') + "\r\n";

            foreach (var file in dto.Files.OrderBy(x => x.Name))
            {
                var fullFilePath = Path.Combine(_basePath, file.GetFullPath());
                
                content += fullPath.PadLeft(fullFilePath.Length + level + 1, '-') + "\r\n";
            }

            foreach (var subDirectory in dto.Subdirectories.OrderBy(x => x.Name))
            {
                content += DescribeRecursive(subDirectory, level + 1, content);
            }

            return content;
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

        public void Create()
        {
            CreateRecursive(_rootDirectory);
        }

        public override string ToString() 
        {
            return DescribeRecursive(_rootDirectory, 1, string.Empty);
        }
    }
}
