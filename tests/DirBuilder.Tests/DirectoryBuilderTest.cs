using System;
using System.IO;
using System.Text;
using DirBuilder.DTO;
using Xunit;

namespace DirBuilder.Tests
{
    public class DirectoryBuilderTest
    {
        [Fact]
        public void Can_Create_Base_Dir_If_Dont_Exists()
        {
            var temp = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            var tempPath = Path.Combine(temp, guid).TrimEnd('/');

            try
            {
                var dirBuilder = new DirectoryBuilder(tempPath, true);
                dirBuilder.Create();
                Directory.Exists(tempPath);
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }
        }

        [Fact]
        public void Throws_If_Base_Dir_Dont_Exists()
        {
            var temp = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            var tempPath = Path.Combine(temp, guid).TrimEnd('/');

            Assert.Throws<DirectoryNotFoundException>(() =>
            {
                new DirectoryBuilder(tempPath, false);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_If_Adding_Empty_Dir_Name(string dirName)
        {
            var dirBuilder = new DirectoryBuilder(Path.GetTempPath());

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddSubdirectoryAndStay(dirName);
            });

            Assert.Equal("directoryName", exception.ParamName);
            Assert.Contains("Directory name can't be null or empty", exception.Message);

            exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddSubdirectoryAndEnter(dirName);
            });

            Assert.Equal("directoryName", exception.ParamName);
            Assert.Contains("Directory name can't be null or empty", exception.Message);
        }

        [Theory(Skip = "Method ValidateDirectoryName() need to be changed")]
        [InlineData("&*#@")]
        public void Throws_If_Adding_Invalid_Dir_Name(string dirName)
        {
            var dirBuilder = new DirectoryBuilder(Path.GetTempPath());

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddSubdirectoryAndStay(dirName);
            });

            Assert.Equal("directoryName", exception.ParamName);
            Assert.Contains("Directory name can't contains invalid chars", exception.Message);

            exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddSubdirectoryAndEnter(dirName);
            });

            Assert.Equal("directoryName", exception.ParamName);
            Assert.Contains("Directory name can't contains invalid chars", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_If_Adding_Empty_File_Name(string fileName)
        {
            var dirBuilder = new DirectoryBuilder(Path.GetTempPath());

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddFile(fileName);
            });

            Assert.Equal("fileName", exception.ParamName);
            Assert.Contains("File name can't be null or empty", exception.Message);

            exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddFile(fileName);
            });

            Assert.Equal("fileName", exception.ParamName);
            Assert.Contains("File name can't be null or empty", exception.Message);
        }

        [Theory(Skip = "Method ValidateFileName() need to be changed")]
        [InlineData("&*#@")]
        public void Throws_If_Adding_Invalid_File_Name(string fileName)
        {
            var dirBuilder = new DirectoryBuilder(Path.GetTempPath());

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddFile(fileName);
            });

            Assert.Equal("fileName", exception.ParamName);
            Assert.Contains("File name can't contains invalid chars", exception.Message);

            exception = Assert.Throws<ArgumentException>(() =>
            {
                dirBuilder.AddFile(fileName);
            });

            Assert.Equal("fileName", exception.ParamName);
            Assert.Contains("File name can't contains invalid chars", exception.Message);
        }

        [Fact]
        public void Can_Describe_Correctly()
        {
            var tempPath = Path.GetTempPath().TrimEnd('/');

            var dirBuilder = new DirectoryBuilder(tempPath, true)
                .AddFile("file1.txt")
                .AddFile("file2.txt")
                .AddSubdirectoryAndEnter("dir1")
                    .AddFile("file11.txt")
                    .AddFile("file12.txt")
                .GoBack()
                .AddSubdirectoryAndStay("dir2")
                .AddSubdirectoryAndEnter("dir3")
                    .AddSubdirectoryAndEnter("dir31")
                        .AddFile("file311.txt")
                        .AddFile("file312.txt")
                    .GoBack()
                .AddFile("file31.txt")
                .GoBack();

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine(tempPath);
            strBuilder.AppendLine(Path.Combine(tempPath, @"file1.txt"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"file2.txt"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir1"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir1/file11.txt"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir1/file12.txt"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir2"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir3"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir3/file31.txt"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir3/dir31"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir3/dir31/file311.txt"));
            strBuilder.AppendLine(Path.Combine(tempPath, @"dir3/dir31/file312.txt"));

            Assert.Equal(strBuilder.ToString(), dirBuilder.ToString());
        }

        [Fact]
        public void Can_Create_Correctly()
        {
            var temp = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            var tempPath = Path.Combine(temp, guid).TrimEnd('/');

            try
            {
                var builder = new DirectoryBuilder(tempPath, true)
                    .AddFile("file1.txt")
                    .AddSubdirectoryAndEnter("dir1")
                        .AddFile("file11.txt")
                    .GoBack();

                builder.Create();

                Assert.True(File.Exists(Path.Combine(tempPath, "file1.txt")));
                Assert.True(Directory.Exists(Path.Combine(tempPath, "dir1")));
                Assert.True(File.Exists(Path.Combine(tempPath, "dir1//file11.txt")));
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }
        }
    }
}
