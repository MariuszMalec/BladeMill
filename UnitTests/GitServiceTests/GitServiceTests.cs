using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Services;
using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnitTests.GitServiceTests
{
    public class GitServiceTests
    {
        [Fact]
        public void GetGITCommitfile_IfNotExist_ThrowFileNotFoundException()
        {
            var gitService = new GitService();
            var commitFile = gitService.GetGITCommitfile();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(commitFile);
            act.Should().NotThrow<FileNotFoundException>();
        }

        [Fact]
        public void GetGITInitfile_IfNotExist_ThrowFileNotFoundException()
        {
            var gitService = new GitService();
            var initFile = gitService.GetGITInitfile();
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(initFile);
            act.Should().NotThrow<FileNotFoundException>();
        }
    }
}
