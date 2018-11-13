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
            var tempPath = Path.GetTempPath().TrimEnd('/');

            var builder = new DirectoryBuilder(tempPath)
                .AddFile("file_1.txt")
                .AddFile("file_2.txt")
                .AddSubdirectoryAndEnter("dir_1")
                    .AddFile("file_1_1.txt")
                    .AddFile("file_1_2.txt")
                    .GoBack()
                .AddSubdirectoryAndStay("dir_2")
                .AddSubdirectoryAndEnter("dir_3")
                    .AddSubdirectoryAndEnter("dir_3_1")
                        .AddFile("file_3_1_1.txt")
                        .AddFile("file_3_1_2.txt")
                        .GoBack()
                    .AddFile("file_3.1.txt")
                    .GoBack();

            var expected = 
                tempPath + "\r\n" + 
                tempPath + @"/file_1.txt" + "\r\n" + 
                tempPath + @"/file_2.txt" + "\r\n" + 
                tempPath + @"/dir_1" + "\r\n" + 
                tempPath + @"/dir_1/file_1_1.txt" + "\r\n" + 
                tempPath + @"/dir_1/file_1_2.txt" + "\r\n" + 
                tempPath + @"/dir_2" + "\r\n" + 
                tempPath + @"/dir_3" + "\r\n" + 
                tempPath + @"/dir_3/file_3.1.txt" + "\r\n" + 
                tempPath + @"/dir_3/dir_3_1" + "\r\n" + 
                tempPath + @"/dir_3/dir_3_1/file_3_1_1.txt" + "\r\n" + 
                tempPath + @"/dir_3/dir_3_1/file_3_1_2.txt" + "\r\n"; 

            Assert.Equal(expected, builder.ToString());
        }
    }
}
