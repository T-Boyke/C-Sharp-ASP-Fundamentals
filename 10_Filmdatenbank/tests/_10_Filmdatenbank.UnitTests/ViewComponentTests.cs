using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Xunit;
using FluentAssertions;
using _10_Filmdatenbank.Web.ViewComponents;
using _10_Filmdatenbank.Infrastructure.Persistence;
using _10_Filmdatenbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;

namespace _10_Filmdatenbank.UnitTests.Web
{
    public class ViewComponentTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        public ViewComponentTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        [Fact]
        public async Task NotificationCount_Returns_Correct_Count()
        {
            // Arrange
            var userId = "test-user";
            var user = new ApplicationUser { Id = userId, UserName = "test" };
            
            _context.Notifications.Add(new Notification { UserID = userId, Message = "Unread", IsRead = false });
            _context.Notifications.Add(new Notification { UserID = userId, Message = "Read", IsRead = true });
            _context.Notifications.Add(new Notification { UserID = "other", Message = "Unread", IsRead = false });
            await _context.SaveChangesAsync();

            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            var component = new NotificationCountViewComponent(_context, _userManagerMock.Object)
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext
                    {
                        HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
                    }
                }
            };

            // Act
            var result = await component.InvokeAsync() as ViewViewComponentResult;

            // Assert
            result.Should().NotBeNull();
            result!.ViewData!.Model.Should().Be(1);
        }

        [Fact]
        public async Task NotificationCount_Returns_Zero_When_Not_Authenticated()
        {
            // Arrange
            _userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns((string?)null);

            var component = new NotificationCountViewComponent(_context, _userManagerMock.Object)
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext
                    {
                        HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
                    }
                }
            };

            // Act
            var result = await component.InvokeAsync() as ViewViewComponentResult;

            // Assert
            result!.ViewData!.Model.Should().Be(0);
        }
    }
}
