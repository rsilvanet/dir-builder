using System;
using System.IO;
using DirBuilder.DTO;

namespace DirBuilder
{
    public static class PathHelper
    {
        private static string GetFullPathRecursive(DirectoryDTO dto)
        {
            if (dto.Parent != null)
            {
                return Path.Combine(GetFullPathRecursive(dto.Parent), dto.Name);
            }

            return dto.Name;
        }

        public static string GetFullPath(DirectoryDTO dto)
        {
            return Path.Combine(GetFullPathRecursive(dto.Parent), dto.Name);
        }

        public static string GetFullPath(FileDTO dto)
        {
            return Path.Combine(GetFullPathRecursive(dto.Directory), dto.Name);
        }
    }
}
