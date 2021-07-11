using BusinessLayer1.Interfaces;
using CommonLayer.DatabaseModel;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        IUserBL<UserModel> _userBL;
        public UserController(IUserBL<UserModel> userBL)
        {
            _userBL = userBL;
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
        public IActionResult Register([FromBody] UserModel user)
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
                    // var tokenString = userAuthentication.GenerateSessionJWT(user);
                    return Ok(new
                    {
                        success = true,
                        Message = "User Login Successful",
                        user
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
