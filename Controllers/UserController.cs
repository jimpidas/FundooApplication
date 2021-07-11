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
        private readonly IDataRepository<UserModel> _dataRepository;
        public UserController(IDataRepository<UserModel> dataRepository)
        {
            _dataRepository = dataRepository;
        }
       
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<UserModel> users = _dataRepository.GetAll();
            return Ok(users);
        }
       
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            UserModel user = _dataRepository.Get(id);
            if (user == null)
            {
                return NotFound("The user record couldn't be found.");
            }
            return Ok(user);
        }
        
        [HttpPost("register")]
        public IActionResult Post([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest("user is null.");
            }
             _dataRepository.Add(user);
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
                UserModel user = _dataRepository.AthenticateUser(loginUser);
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
