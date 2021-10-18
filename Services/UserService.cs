using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zmgTestBack.Models;

namespace zmgTestBack.Services
{
    public class UserService : IUserService
    {
        private ZmgTestDbContext _dbContext;

        public UserService(ZmgTestDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public User LogIn(User user)
        {
            User resultUser = _dbContext.Users.SingleOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if(resultUser == null)
                throw new ArgumentNullException(String.Format("The user does not exist"));
            return resultUser;
        }
    }
}
