using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Common;

namespace BL.DTOs.Users
{
    [Serializable]
    public class UserListQueryResultDTO : PagedListQueryResultDTO<UserDTO>, BaseDTO
    {
    }
}
