using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Git.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            string userId = this.usersService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Error("Invalid username or password!");
            }
            this.SignIn(userId);
            return this.Redirect("/Repositories/All");
        }

        [HttpGet]
        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return this.Error("Both passwords did not match!");
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username) || username.Length < 5 || username.Length > 20)
            {
                return this.Error("Invalid username. Username must be between 5 and 20 characters!");
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return this.Error("Invalid email!");
            }

            if (!this.usersService.IsEmailAvailable(email))
            {
                return this.Error("Email already taken!");
            }

            if (!this.usersService.IsUsernameAvailable(username))
            {
                return this.Error("Username already taken!");
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password) || password.Length < 6 || password.Length > 20)
            {
                return this.Error("Invalid password!");
            }

            this.usersService.CreateUser(username, email, password);
            return this.Redirect("/Users/Login");
        }

        [HttpGet]
        public HttpResponse Logout()
        {
            this.SignOut();
            return this.Redirect("/Users/Login");
        }
    }
}
