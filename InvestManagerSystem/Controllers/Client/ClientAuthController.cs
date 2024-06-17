using Microsoft.AspNetCore.Mvc;
using InvestManagerSystem.Services.UserService;
using InvestManagerSystem.Interfaces.User;
using InvestManagerSystem.Interfaces.Auth;
using InvestManagerSystem.Services.AuthService;
using InvestManagerSystem.Enums;
using InvestManagerSystem.Auth.Decorators;
using InvestManagerSystem.Global.Helpers.ApiPrefix;

namespace InvestManagerSystem.Controllers.Client
{
    [ApiController]
    [Route(ApiPrefix.Client + "auth")]
    public class ClientAuthController : ControllerBase
    {
        private readonly ILogger<ClientAuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public ClientAuthController(ILogger<ClientAuthController> logger, IAuthService authService, IUserService userService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
        }

        // POST /api/client/auth/login
        [Route("login")]
        [HttpPost]
        public ActionResult<CredentialResponseDto> Login([FromBody] CredentialDto credential)
        {
            _logger.LogInformation($"start method {nameof(Login)}");
            var response = _authService.LoginClient(credential);
            return Ok(response);
        }

        // POST /api/client/auth/register
        [Route("register")]
        [HttpPost]
        public ActionResult<UserSaveResponseDto> Register([FromBody] UserCreateDto newUser)
        {
            _logger.LogInformation($"start method {nameof(Register)}");
            var response = _userService.Register(newUser, UserTypeEnum.Client);
            return CreatedAtAction(nameof(Register), response);
        }
    }
}
