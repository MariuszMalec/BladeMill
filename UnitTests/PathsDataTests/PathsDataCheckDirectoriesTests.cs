using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests.PathsDataTests
{
    public class PathsDataCheckDirectoriesTests
    {
        [Theory]
        [ClassData(typeof(FirstNameClassData))]
        public void GetDir_WhenFileNotExist_ReturnError(string value)
        {
            Action act = () => ExtensionMethods.CheckDirectoryIfNotExistThrowException(value);
            act.Should().NotThrow<DirectoryNotFoundException>();
        }
        internal class FirstNameClassData : IEnumerable<object[]>
        {
            static PathDataBase _paths = new PathDataBase();
            string dir0 = _paths.GetDirCmm();
            string dir1 = _paths.GetDirTask();
            string dir2 = _paths.GetCleverHome();
            string dir3 = _paths.GetDirBladeMillScripts();
            string dir4 = _paths.GetDirHtml();
            string dir5 = _paths.GetDirIcon();
            string dir6 = _paths.GetDirProgramExe();
            string dir7 = _paths.GetDirOrders();
            string dir8 = _paths.GetDirVericutProjectTemplate();
            string dir9 = _paths.GetDirMainProgramTemplate();
            string dir10 = _paths.GetDirAutomationScriptLauncher();
            string dir11 = _paths.GetNxDir();
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new string[] { dir0 };
                yield return new string[] { dir1 };
                yield return new string[] { dir2 };
                yield return new string[] { dir3 };
                yield return new string[] { dir4 };
                yield return new string[] { dir5 };
                yield return new string[] { dir6 };
                yield return new string[] { dir7 };
                yield return new string[] { dir8 };
                yield return new string[] { dir9 };
                yield return new string[] { dir10 };
                yield return new string[] { dir11 };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
