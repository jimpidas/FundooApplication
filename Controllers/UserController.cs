using BusinessLayer.Interfaces;
using CommonLayer.DatabaseModel;
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
        /*IUserBL iuserBL;
        public UserController(IUserBL userBL)
        {
            this.iuserBL = userBL;
        }
        [HttpPost]
       
        public IActionResult RegisterUser(UserModel user)
        {
            try
            {
                this.iuserBL.RegisterUser(user);
                return this.Ok(new { success = true, message = $"Registration Successful {user.FirstName}" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = $"Registration Fail {e.Message}+{e.InnerException}" });
            }
        }
        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                List<UserModel> userList=this.iuserBL.ReturnUserList();
                return this.Ok(new { success = true, message = $"The user list is ", data = userList }); 
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = $"Registration Fail {e.Message}+{e.InnerException}" });
            }
        }*/

        private readonly IDataRepository<UserModel> _dataRepository;
        public UserController(IDataRepository<UserModel> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Employee
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<UserModel> users = _dataRepository.GetAll();
            return Ok(users);
        }
        // GET: api/Employee/5
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
        // POST: api/Employee
        [HttpPost]
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
    }
}
