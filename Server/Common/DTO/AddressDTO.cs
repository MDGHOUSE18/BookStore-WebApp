using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string TypeOfAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
