using System;
using System.IO;
using System.Linq;
using System.Text;
using DirBuilder.DTO;

namespace DirBuilder
{
    public class DirectoryBuilder
    {
        private readonly string _basePath;
        private readonly DirectoryDTO _rootDirectory;
        private DirectoryDTO _currentDirectory;

        public DirectoryBuilder(string basePath, bool createBasePath = false)
        {
            if (!Directory.Exists(basePath))
            {
                if (!createBasePath)
                {
                    throw new DirectoryNotFoundException(basePath);
                }
            }

            var info = new DirectoryInfo(basePath);

            _basePath = info.Parent.FullName;
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

            if (directoryName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
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

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
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

        public string DescribeRecursive(DirectoryDTO dto, StringBuilder strBuilder)
        {
            strBuilder.AppendLine(Path.Combine(_basePath, dto.GetFullPath()));

            foreach (var file in dto.Files.OrderBy(x => x.Name))
            {
                strBuilder.AppendLine(Path.Combine(_basePath, file.GetFullPath()));
            }

            foreach (var subDirectory in dto.Subdirectories.OrderBy(x => x.Name))
            {
                DescribeRecursive(subDirectory, strBuilder);
            }

            return strBuilder.ToString();
        }

        public DirectoryBuilder AddSubdirectoryAndStay(string name)
        {
            ValidateDirectoryName(name);

            _currentDirectory.AddSubdirectory(name);
            
            return this;
        }

        public DirectoryBuilder AddSubdirectoryAndEnter(string name)
        {
            ValidateDirectoryName(name);
            
            _currentDirectory = _currentDirectory.AddSubdirectory(name);
            
            return this;
        }

        public DirectoryBuilder AddFile(string name) 
        {
            ValidateFileName(name);
            
            _currentDirectory.AddFile(name);
            
            return this;
        }

        public DirectoryBuilder GoBack() 
        {
            _currentDirectory = _currentDirectory.Parent;

            return this;
        }

        public void Create()
        {
            CreateRecursive(_rootDirectory);
        }

        public override string ToString() 
        {
            return DescribeRecursive(_rootDirectory, new StringBuilder());
        }
    }
}
