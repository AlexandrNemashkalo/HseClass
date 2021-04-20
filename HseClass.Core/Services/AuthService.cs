using System;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Core.Jwt;
using HseClass.Data.Entities;
using HseClass.Data.Enums;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;

namespace HseClass.Core.Services
{
    public enum RoleEnums
    {
        Student,
        Teacher
    }
    
    public class AuthService : IAuthService
    {
        private readonly IJwtGenerator _jwt;
        private readonly HseClassContext _data;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(
            IJwtGenerator jwt,
            HseClassContext  data,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _data = data;
            _jwt = jwt;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<object> Login(string email, string password)
        {
            if (email == null || password == null)
            {
                throw new Exception("Invalid login or password");
            }

            var appUser = await _userManager.FindByEmailAsync(email);

            if(appUser == null)
            {
                throw new Exception("User not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser,password,false);

            if (!result.Succeeded)
            {
                throw new Exception("Something went wrong during registration");
            }

            return await _jwt.GenerateJwt(appUser);
        }

        public async Task<object> Register(string email, string password, string name, RoleEnums role)
        {
 
            if (email == null || password == null)
            {
                throw new Exception();
            }
            
            var user = new User()
            {
                Email = email,
                UserName = email,
                Name = name
            };
            
            var result = await  _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception();

            switch (role)
            {
                case RoleEnums.Student:
                    await  _userManager.AddToRoleAsync(user, "student");
                    break;
                case RoleEnums.Teacher:
                    await  _userManager.AddToRoleAsync(user, "teacher");
                    break;
                default:
                    throw new Exception("такой роли ни существует ");
            }        
            
            await _signInManager.SignInAsync(user, false);
            
            return await _jwt.GenerateJwt(user);
        }
    }
}