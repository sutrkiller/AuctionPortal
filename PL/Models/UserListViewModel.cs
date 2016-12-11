using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTOs.Users;
using X.PagedList;

namespace PL.Models
{
    public class UserListViewModel
    {
        public IPagedList<UserDTO> Users { get; set; }
    }
}