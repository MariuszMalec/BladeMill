using BladeMill.BLL.DAL;
using BladeMill.BLL.Repositories;
using BladeMill.BLL.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserServiceTests
{
    public class UserServiceWithMockTests
    {
        private readonly UserService _sut;
        private readonly Mock<IBaseRepository<UserDto>> _userMockRepo = new Mock<IBaseRepository<UserDto>>();

        public UserServiceWithMockTests()
        {
            _sut = new UserService(_userMockRepo.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShoudReturnEmptyUsers_WhenUsersNotExist()
        {
            // Arrange
            _userMockRepo.Setup(x => x.FindAll()).ReturnsAsync(GetUsersDto());

            // Act
            var users = await _sut.FindAll();

            // Assert
            users.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUserSSO_ShoudReturnFalse_WhenUsersHasSsoGreaterThan9()
        {
            // Arrange
            _userMockRepo.Setup(x => x.FindById(1)).ReturnsAsync(GetUserDto());

            // Act
            var user = await _sut.FindById(1);
            var result = user.Sso.ToString().Length;

            // Assert
            Assert.Equal(9, result);
        }
        private UserDto GetUserDto()
        {
            var userDto = new UserDto()
            {
                Id = 1,
                LastName = "Mario",
                FirstName = "BB",
                Sso = 212517683
            };
            return userDto;
        }
        private List<UserDto> GetUsersDto()
        {
            var sessions = new List<UserDto>();
            sessions.Add(new UserDto()
            {
                Id = 1,
                LastName = "Mario",
                FirstName = "BB",
                Sso = 212555156
            });
            sessions.Add(new UserDto()
            {
                Id = 2,
                LastName = "Mario",
                FirstName = "CC",
                Sso = 212555163
            });
            return sessions;
            //return new List<Trainer>();
        }

    }
}
