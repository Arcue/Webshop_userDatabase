using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UserAPI.Models;

namespace UserAPI.Services
{
    public interface IUserService
    {
        TableUser Authenticate(string username, string password);
        TableUser Create(TableUser user, string password);
        IEnumerable<TableUser> GetAll();
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

        public TableUser Authenticate(string username, string password)
        {
            var user = _context.TableUser.SingleOrDefault(x => x.Username == username);
            
            //Kollar om användarnamnet redan finns i databasen
            if (user == null)
            {
                return null;
            }
            //Kollar password
            if (!VerifyPassword(password, user.Hashedpassword, user.Salt))
            {
                return null;
            }
            
            return user;
        }

        public TableUser Create(TableUser user, string password)
        {
            //Kollar att password ärn angivet
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ApplicationException("Password is needed");
            }
            //Kollar ifall det angivna användarnamnet redan finns
            if (_context.TableUser.Any(x => x.Username == user.Username))
            {
                throw new ApplicationException("Usernamne is already in use");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.Hashedpassword = Convert.ToBase64String(passwordHash);
            user.Salt = Convert.ToBase64String(passwordSalt);
            
            _context.TableUser.Add(user);
            _context.SaveChanges();
            
            return user;
        }
        
        public IEnumerable<TableUser> GetAll()
        {
            return _context.TableUser;
        }

        public void Update(TableUser user, String password = null)
        {
            
        }

        public void Delete(int id)
        {
            var user = _context.TableUser.Find((id));
            if (user != null)
            {
                _context.TableUser.Remove(user);
                _context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(String password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPassword(String password, String hash, String salt)
        {
            //Kom ihåg att konvertera string salt/hashed password till bytes!
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value");
            }

            byte[] hashByte = Convert.FromBase64String(hash);
            byte[] saltByte = Convert.FromBase64String(salt);

            using (var hmac = new System.Security.Cryptography.HMACSHA512(saltByte))
            {
                
                //Jämför stored hash med calculated hash;
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hashByte[i])
                    {
                        return false;
                    }
                }
            }

            
            return true;
        }
        
        
    }
}