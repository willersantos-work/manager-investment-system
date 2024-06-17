using Microsoft.AspNetCore.Mvc;
using InvestManagerSystem.Services.UserService;
using InvestManagerSystem.Interfaces.User;
using InvestManagerSystem.Interfaces.Auth;
using InvestManagerSystem.Services.AuthService;
using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Helpers.ApiPrefix;

namespace InvestManagerSystem.Controllers.Admin
{
    [ApiController]
    [Route(ApiPrefix.Admin + "auth")]
    public class AdminAuthController : ControllerBase
    {
        private readonly ILogger<AdminAuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AdminAuthController(ILogger<AdminAuthController> logger, IAuthService authService, IUserService userService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
        }

        // POST /api/admin/auth/login
        [Route("login")]
        [HttpPost]
        public ActionResult<CredentialResponseDto> Login([FromBody] CredentialDto credential)
        {
            _logger.LogInformation($"start method {nameof(Login)}");
            var response = _authService.LoginAdmin(credential);
            return Ok(response);
        }

        // POST /api/admin/auth/register
        [Route("register")]
        [HttpPost]
        public ActionResult<UserSaveResponseDto> Register([FromBody] UserCreateDto newUser)
        {
            _logger.LogInformation($"start method {nameof(Register)}");
            var response = _userService.Register(newUser, UserTypeEnum.Admin);
            return CreatedAtAction(nameof(Register), response);
        }
    }
}
