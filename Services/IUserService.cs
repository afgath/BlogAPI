using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zmgTestBack.Models;

namespace zmgTestBack.Services
{
    public interface IUserService
    {
        User LogIn(UserRequest user);
    }
}
