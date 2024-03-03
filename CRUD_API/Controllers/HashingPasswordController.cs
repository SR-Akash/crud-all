using CRUD_API.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashingPasswordController : ControllerBase
    {
        private readonly IHashingPassword _password;

        public HashingPasswordController(IHashingPassword password)
        {
            _password = password;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDTO create)
        {
            var msg =await _password.CreateUser(create);
            return Ok(msg);
        }

        [HttpPost]
        [Route("UserVerify")]
        public async Task<IActionResult> UserVerify(UserDTO verify)
        {
            var msg = await _password.UserVerify(verify);
            return Ok(msg);
        }
    }
}
