using Git.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Git.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = EncryptPassword(password)
            };

            this.dbContext.Users.AddAsync(user);
            this.dbContext.SaveChangesAsync();
            return user.UserId;
        }

        public string GetUserId(string username, string password)
        {
            var userId = this.dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == EncryptPassword(password))
                .UserId;

            return userId;
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.dbContext.Users.Any(x => x.Email == email);
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.dbContext.Users.Any(x => x.Username == username);
        }

        public static string EncryptPassword(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
