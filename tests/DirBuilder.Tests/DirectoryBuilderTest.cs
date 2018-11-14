using System;
using System.IO;
using DirBuilder.DTO;
using Xunit;

namespace DirBuilder.Tests
{
    public class DirectoryBuilderTest
    {
        [Fact]
        public void Can_Describe()
        {
            var temp = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            var tempPath = Path.Combine(temp, guid).TrimEnd('/');
            
            Directory.CreateDirectory(tempPath);

            try
            {
                var builder = new DirectoryBuilder(tempPath)
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

                var expected = 
                    tempPath + "\r\n" + 
                    tempPath + @"/file1.txt" + "\r\n" + 
                    tempPath + @"/file2.txt" + "\r\n" + 
                    tempPath + @"/dir1" + "\r\n" + 
                    tempPath + @"/dir1/file11.txt" + "\r\n" + 
                    tempPath + @"/dir1/file12.txt" + "\r\n" + 
                    tempPath + @"/dir2" + "\r\n" + 
                    tempPath + @"/dir3" + "\r\n" + 
                    tempPath + @"/dir3/file31.txt" + "\r\n" + 
                    tempPath + @"/dir3/dir31" + "\r\n" + 
                    tempPath + @"/dir3/dir31/file311.txt" + "\r\n" + 
                    tempPath + @"/dir3/dir31/file312.txt" + "\r\n"; 

                Assert.Equal(expected, builder.ToString());
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }
        }
    
        [Fact]
        public void Can_Create()
        {
            var temp = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            var tempPath = Path.Combine(temp, guid).TrimEnd('/');
            
            Directory.CreateDirectory(tempPath);

            try
            {
                var builder = new DirectoryBuilder(tempPath)
                    .AddFile("file1.txt")
                    .AddSubdirectoryAndEnter("dir1")
                        .AddFile("file11.txt");

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
