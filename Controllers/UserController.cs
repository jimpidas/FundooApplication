using BusinessLayer1.Interfaces;
using CommonLayer.DatabaseModel;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        IUserBL<UserModel> _userBL;
        readonly UserAuthenticationJWT userAuthentication;
        private readonly IConfiguration config;
        public UserController(IUserBL<UserModel> userBL, IConfiguration config)
        {
            _userBL = userBL;
            this.config = config;
            userAuthentication = new UserAuthenticationJWT(this.config);
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<UserModel> users = _userBL.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            UserModel user = _userBL.Get(id);
            if (user == null)
            {
                return NotFound("The user record couldn't be found.");
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public IActionResult Add([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest("user is null.");
            }
            _userBL.Add(user);
            return CreatedAtRoute(
               "Get",
              new { Id = user.UserModelID },
            user);
        }
        [HttpPost("Login")]
        public IActionResult AuthenticateUser(LoginRequestModel loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest("user is null.");
            }
            try
            {
                UserModel user = _userBL.AthenticateUser(loginUser);
                if (user != null)
                {
                    var tokenString = userAuthentication.GenerateJSONWebToken(user);
                    return Ok(new
                    {
                        success = true,
                        Message = "User Login Successful",
                        user,
                        token = tokenString
                    });
                }
                return BadRequest(new { success = false, Message = "User Login Unsuccessful" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }

    }
}
