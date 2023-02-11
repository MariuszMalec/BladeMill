using BladeMill.BLL.Services;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace UnitTests.UserServiceTests
{
    public class UserServiceTests
    {
        public UserServiceTests()
        {
            Sut = new UserServiceWithoutDatabase();
        }
        private UserServiceWithoutDatabase Sut { get; }

        [Fact]
        public void GetUsers_WhenListUsersIsNotEmpty()
        {
            var users = Sut.GetAll();
            users.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public void GetUser_WhenUserIdNotEmpty()
        {
            var user = Sut.GetById(1);
            //FluentAssertions Nugets
            user.Id.Should().BeGreaterThan(0);
            Assert.NotNull(user);
        }
        [Fact]
        public void GetUserSso_WhenUserSsoIsNotNull()
        {
            var user = Sut.GetUserSso();
            //FluentAssertions Nugets
            user.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(212517683)]
        [InlineData(212537861)]
        [InlineData(212556351)]
        [InlineData(212538300)]
        [InlineData(212736611)]
        [InlineData(212540098)]
        [InlineData(212510479)]
        [InlineData(212564181)]
        [InlineData(212791400)]
        [InlineData(212583581)]
        public void CheckSsoUsers_WhenNotExist_ReturnFalse(object value)
        {
            var users = Sut.GetAll();
            var check = users.Where(u => u.Sso.ToString() == value.ToString()).FirstOrDefault();
            Assert.NotNull(check);
        }

        [Theory]
        [InlineData(212517683, "Malec",true)]
        [InlineData(212537861,"Orzechowski", true)]
        [InlineData(212556351,"Wisniewski", true)]
        [InlineData(212538300,"Popakul", true)]
        [InlineData(212736611,"Obrebski", true)]
        [InlineData(212540098,"Papierowski", true)]
        [InlineData(212510479,"Grajczak", true)]
        [InlineData(212564181,"Kuszyk", true)]
        [InlineData(212791400,"Staszynski", true)]
        [InlineData(212583581, "Mielewczyk", true)]
        public void CheckSsoUsersWithLastName_WhenIsNotPass_ReturnFalse(int sso, string lastname, bool check)
        {
            var users = Sut.GetAll();
            bool result = users.Any(u => u.Sso == sso && u.LastName == lastname);            
            Assert.Equal(check, result);
        }

        [Fact]
        public void CheckNumberUsers_WhenIsNot9_ReturnFalse()
        {
            var users = Sut.GetAll();
            var currentNmbUsers = 10;
            var getUsers = users.Select(u => u.Sso).ToList().Count();
            Assert.Equal(currentNmbUsers, getUsers);
        }
    }
}
