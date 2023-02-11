using AutoMapper;
using BladeMill.BLL.DAL;
using BladeMill.BLL.Interfaces;
using BladeMill.BLL.Models;
using BladeMill.BLL.Repositories;
using BladeMill.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserControllerTests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Index_ReturnsUsersViewResult_WhenStatusNotView_ReturnFalse()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddLogging()
            .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<UserController>();

            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<UserDto, User>());
            var mapper = new Mapper(config);

            //var mockRepo = new Mock<IBaseRepository<User>>();
            //mockRepo.Setup(repo => repo.FindAll())
            //    .ReturnsAsync(GetUsers());
            //;

            var mockUser = new Mock<IUserService>();
            mockUser.Setup(r => r.FindAll()).ReturnsAsync(new List<UserDto>() { new UserDto() { 
                Id =1,
                FirstName= "Test",
                LastName = "Test",
                Sso = 1223456,
                Created = DateTime.Now,
                FullName= "Test"
            } });
            
            var mockLogger = new Mock<ILogger<UserController>>().Object;

            var controller = new UserController(mockUser.Object, mockLogger, mapper);

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(result.Model);
        }
    }
}
