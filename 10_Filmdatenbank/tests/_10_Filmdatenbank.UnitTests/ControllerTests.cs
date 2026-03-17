using _10_Filmdatenbank.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Security.Claims;

namespace _10_Filmdatenbank.UnitTests.Web
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger = new();

        private HomeController GetController()
        {
            var controller = new HomeController(_mockLogger.Object);
            var context = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
            return controller;
        }

        [Fact]
        public void Index_Returns_View_When_Not_Authenticated()
        {
            var controller = GetController();
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_Redirects_To_Dashboard_When_Authenticated()
        {
            var controller = GetController();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "test") }, "Test"));
            controller.HttpContext.User = user;

            var result = controller.Index();
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirectResult.ActionName);
            Assert.Equal("User", redirectResult.ControllerName);
        }

        [Fact]
        public void Privacy_Returns_View()
        {
            var controller = GetController();
            var result = controller.Privacy();
            Assert.IsType<ViewResult>(result);
        }
    }

    public class AccountControllerTests
    {
        private readonly Mock<UserManager<_10_Filmdatenbank.Domain.Entities.ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<_10_Filmdatenbank.Domain.Entities.ApplicationUser>> _mockSignInManager;

        public AccountControllerTests()
        {
            _mockUserManager = new Mock<UserManager<_10_Filmdatenbank.Domain.Entities.ApplicationUser>>(
                new Mock<IUserStore<_10_Filmdatenbank.Domain.Entities.ApplicationUser>>().Object, 
                null!, null!, null!, null!, null!, null!, null!, null!);
            
            _mockSignInManager = new Mock<SignInManager<_10_Filmdatenbank.Domain.Entities.ApplicationUser>>(
                _mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<_10_Filmdatenbank.Domain.Entities.ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<_10_Filmdatenbank.Domain.Entities.ApplicationUser>>>().Object,
                new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<_10_Filmdatenbank.Domain.Entities.ApplicationUser>>().Object);
        }

        [Fact]
        public void Login_Returns_View()
        {
            var controller = new AccountController(_mockSignInManager.Object, _mockUserManager.Object);
            var result = controller.Login();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AccessDenied_Returns_View()
        {
            var controller = new AccountController(_mockSignInManager.Object, _mockUserManager.Object);
            var result = controller.AccessDenied();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Logout_Redirects_To_Home()
        {
            var controller = new AccountController(_mockSignInManager.Object, _mockUserManager.Object);
            var result = await controller.Logout();
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }
    }
}
