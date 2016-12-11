using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Users
{
    [Serializable]
    public class UserDTO : BaseDTO
    {
        public long ID { get; set; }
        //public long AddressId { get; set; }
       
        public string FirstName { get; set; }
       
        public string LastName { get; set; }

        public string Address { get; set; }
        
        public DateTime BirthDate { get; set; }
       
        public string Email { get; set; }
    }
}
