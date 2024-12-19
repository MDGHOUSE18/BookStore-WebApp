using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class ForgetPasswordDTO
    {
        public string Email { get; set; }
        //public int UserId { get; set; }
        public string Token { get; set; }
    }
}
