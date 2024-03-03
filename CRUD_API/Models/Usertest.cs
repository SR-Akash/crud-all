using System;
using System.Collections.Generic;

#nullable disable

namespace CRUD_API.Models
{
    public partial class Usertest
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public byte[] Salt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActiondatetime { get; set; }
    }
}
