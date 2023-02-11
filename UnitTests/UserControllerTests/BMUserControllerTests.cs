using BladeMill.BLL.DAL;
using BladeMill.BLL.Interfaces;
using BladeMill.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class BMUserControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WhenStatusNoViewtResult_ReturnError()
        {
            // Arrange
            var mockRepo = new Mock<IBMUserService>();
            mockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(GetBmUserDtos());
            ;
            var controller = new BMUserController(mockRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_IfViewResultModelNotCorrect_ReturnError()
        {
            var curentViewUsers = GetBmUserDtos();
            var mock = new Mock<IBMUserService>();
            mock.Setup(u => u.GetAll())
                .ReturnsAsync(curentViewUsers);

            var controller = new BMUserController(mock.Object);

            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.Equal(curentViewUsers, result.Model);
        }

        private IEnumerable<BMUserDto> GetBmUserDtos()
        {
            var users = new List<BMUserDto>()
            {
                new BMUserDto()
                {
                    Id=1,
                    FirstName = "Mariusz",
                    LastName = "Malec",
                    Sso = 212517683,
                    Created = DateTime.Now,
                    FullName = ""
                }
            };
            return users;
        }
    }
}
