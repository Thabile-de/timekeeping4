using Microsoft.AspNetCore.Mvc;
using TimeKeeping4.Model;
using TimeKeeping4.Repositories;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TimeKeeping4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<string>> Register(UserRegistration user)
        {
            var result = await _userRepository.CreateUserAsync(user);
            return Ok (result.Message);
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<string>> SignIn(UserSignIn user)
        {           
                var result = await _userRepository.TryLoginAsync(user);
                return Ok("Login Successful!!");
        }

        [HttpPost]
        [Route("confirmUserSignup")]
        public async Task<ActionResult<string>> ConfirmUserSignUp(UserConfirmSignUp user)
        {
             var result = await _userRepository.ConfirmUserSignUpAsync(user);
            return Ok("user confirmed");
       }
    }
}