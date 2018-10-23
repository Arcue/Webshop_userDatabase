using UserAPI.Models;

namespace UserAPI.Services
{
    public interface IUserService
    {
        TableUser Authenticate(string username, string password);
        TableUser Create(TableUser user, string password);
        void Update(TableUser user, string password = null);
        void Delete(int id);
    }
    
    public class UserService : IUserService
    {
        private dfug8uq2aj17f1Context _context;

        public UserService(dfug8uq2aj17f1Context context)
        {
            _context = context;
        }
        
        
    }
}