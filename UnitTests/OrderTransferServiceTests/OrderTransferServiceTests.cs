using BladeMill.BLL.ExtentionsMethod;
using BladeMill.BLL.Services;
using BladeMill.BLL.SourceData;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.OrderTransferServiceTests
{
    public class OrderTransferServiceTests
    {
        private static string userPath = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\BladeMill\UnitTests\SourceData");
        public OrderTransferServiceTests()
        {
            Sut = new OrderTransferService();
        }
        private OrderTransferService Sut { get; }

        [Theory]
        [InlineData("-01.MPF")]
        [InlineData("-02.MPF")]
        public void GetCorrectMainProgramName_ReturnError_WhenIsWrong(string value)
        {
            var allFiles = Sut.GetAllFileFromNcDir();

            var result = allFiles.Any(f => f.Contains(value));

            Assert.False(result);
        }

        [Fact]
        public void GetAllFileFromNcDir_ReturnError_WhenIsEmpty()
        {
            var allFiles = Sut.GetAllFileFromNcDir();
            allFiles.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetAllFileFromOrder_ReturnError_WhenIsEmpty()
        {
            var allFiles = Sut.GetAllFileFromOrder();
            allFiles.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(FirstNameClassData))]
        public void CheckFilesForOrderTransferIfExist_WhenFileNotExist_ReturnError(string value)
        {
            Action act = () => ExtensionMethods.CheckFileIfNotExistThrowException(value);
            act.Should().NotThrow<FileNotFoundException>();
        }
        internal class FirstNameClassData : IEnumerable<object[]>
        {
            string file0 = Path.Combine(userPath, "A88888801.MPF");
            string file1 = Path.Combine(userPath, "A88888802.MPF");
            string file2 = Path.Combine(userPath, "A888888.VcProject");
            string file3 = Path.Combine(userPath, "current.xml");
            string file4 = Path.Combine(userPath, "m_BladeBody.stl");
            string file5 = Path.Combine(userPath, "m_CLAMP_ADAPTER.stl");
            string file6 = Path.Combine(userPath, "m_CLAMP_ADAPTER.stl");
            string file7 = Path.Combine(userPath, "m_CLAMPING_GEOMETRY.stl");
            string file8 = Path.Combine(userPath, "A88888811.SPF");
            string file9 = Path.Combine(userPath, "A88888833.SPF");
            string file10 = Path.Combine(userPath, "A88888849.SPF");
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new string[] { file0 };
                yield return new string[] { file1 };
                yield return new string[] { file2 };
                yield return new string[] { file3 };
                yield return new string[] { file4 };
                yield return new string[] { file5 };
                yield return new string[] { file6 };
                yield return new string[] { file7 };
                yield return new string[] { file8 };
                yield return new string[] { file9 };
                yield return new string[] { file10 };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
