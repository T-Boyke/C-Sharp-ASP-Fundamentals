using _10_Filmdatenbank.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace _10_Filmdatenbank.UnitTests.Web
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Privacy();
            Assert.IsType<ViewResult>(result);
        }
    }

    public class AccountControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;

        public AccountControllerTests()
        {
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null);
            
            _mockSignInManager = new Mock<SignInManager<IdentityUser>>(
                _mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                null, null, null, null);
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
