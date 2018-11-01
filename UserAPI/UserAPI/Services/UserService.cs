using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using Newtonsoft.Json;

using UserAPI.Dto;

using UserAPI.Models;

namespace UserAPI.Services
{
    public interface IUserService
    {
        TableUser Authenticate(string username, string password);
        TableUser Create(TableUserDto user, string password);
        IEnumerable<TableUser> GetAll();

        TableUser GetUserInfo(String token);
        TableUser Update(String token, String newUserInfo);
        void Delete(int id);
        void StoreToken(String token, int userId);


    }
    
    public class UserService : IUserService
    {
        private dfug8uq2aj17f1Context _context;

        public UserService(dfug8uq2aj17f1Context context)
        {
            _context = context;
        }

        public TableUser Authenticate(string name, string password)
        {
            var user = _context.TableUser.SingleOrDefault(x => x.Name == name);
            
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

        public TableUser Create(TableUserDto user, string password)
        {
            var tableUser = new TableUser();
            tableUser.Name = user.name;
            tableUser.Email = user.Email;
            tableUser.Password = user.Password;
            tableUser.Adress = user.Adress;
            tableUser.Postnummer = user.Postnummer;
            tableUser.Stad = user.Stad;
            

            //Kollar att password ärn angivet
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ApplicationException("Password is needed");
            }
            //Kollar ifall det angivna användarnamnet redan finns
            if (_context.TableUser.Any(x => x.Name == user.name))
            {
                throw new ApplicationException("Usernamne is already in use");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            tableUser.Hashedpassword = Convert.ToBase64String(passwordHash);
            tableUser.Salt = Convert.ToBase64String(passwordSalt);
            
            _context.TableUser.Add(tableUser);
            _context.SaveChanges();
            
            return tableUser;
        }
        
        public IEnumerable<TableUser> GetAll()
        {
            return _context.TableUser;
        }




        public TableUser Update(string token, String newUserInfo)

        {
            int userId = getUserId(token);

            if (userId == null)
            {
                throw new ArgumentNullException("password");
            }
            else
            {
                var user = _context.TableUser.Find(userId);

                UpdateUserInfoDto jsonInfo = JsonConvert.DeserializeObject<UpdateUserInfoDto>(newUserInfo);

                if (jsonInfo.name != null)
                {
                    user.Name = jsonInfo.name;
                }
                
                if (jsonInfo.Email!= null)
                {
                    user.Email = jsonInfo.Email;
                }
                
                if (jsonInfo.password!= null)
                {
                    //Måste hasha password!!!!
                    user.Password = jsonInfo.password;
                }
                
                if (jsonInfo.homeadress != null)
                {
                    user.Adress = jsonInfo.homeadress;
                }
                
                if (jsonInfo.postnumber != null)
                {
                    user.Postnummer = jsonInfo.postnumber;
                }
                if (jsonInfo.city != null)
                {
                    user.Stad = jsonInfo.city;
                }
                
                //Detta kan vara orsakan att det är fel
                //Måste kanske sätta in userId för att den ska hitta rätt
                _context.TableUser.Update(user);
                _context.SaveChanges();
                  
                return  user;
            }

            
            
        }
        
        //Hämtar userId baserat på token
        public int getUserId(String token)
        {
            var user = _context.TableUser.SingleOrDefault(x => x.XAuthToken.Equals(token));
            int userId = user.Userid;
            
            return userId;
        }

        public TableUser GetUserInfo(String token)
        {
            var user = _context.TableUser.Find(getUserId(token));

            return user;
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

        public void StoreToken(String token, int userId)
        {
            var user = _context.TableUser.Find(userId);

            if (user == null)
            {
                throw new ApplicationException("User not found");
            }
            
            user.XAuthToken = token;
            _context.TableUser.Update(user);
            _context.SaveChanges();

        }
        
        
    }
}