using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_API.IRepository
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public interface IHashingPassword
    {
        public Task<string> CreateUser(UserDTO create);
        public Task<string> UserVerify(UserDTO verify);
    }
}
