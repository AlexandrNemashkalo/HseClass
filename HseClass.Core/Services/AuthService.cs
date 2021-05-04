using System;
using System.Threading.Tasks;
using HseClass.Core.Jwt;
using HseClass.Core.Entities;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;

namespace HseClass.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtGenerator _jwt;
        private readonly IUnitOfWork _data;
        
        public AuthService(
            IJwtGenerator jwt,
            IUnitOfWork  data)
        {
            _data = data;
            _jwt = jwt;
        }

        public async Task<object> Login(string email, string password)
        {
            if (email == null || password == null)
            {
                throw new Exception("Invalid login or password");
            }

            var appUser = await _data.User.FindByEmail(email);

            if(appUser == null)
            {
                throw new Exception("User not found");
            }

            var result = await _data.User.CheckPasswordSignIn(appUser,password);

            if (!result)
            {
                throw new Exception("Something went wrong during registration");
            }

            return await _jwt.GenerateJwt(appUser);
        }

        public async Task<object> RegisterStudent(string email, string password, string name)
        {
            if (email == null || password == null)
            {
                throw new Exception();
            }
            
            var user = new Student()
            {
                Email = email,
                UserName = email,
                Name = name
            };
            
            var result = await  _data.User.Create(user, password);

            if (!result)
                throw new Exception();

            await  _data.User.AddToRole(user, "student");
            
            await _data.User.SignIn(user, false);
            
            return await _jwt.GenerateJwt(user);
        }
        
        public async Task<object> RegisterTeacher(string email, string password, string name)
        {
 
            if (email == null || password == null)
            {
                throw new Exception();
            }

            var user = new Teacher()
            {
                Email = email,
                UserName = email,
                Name = name
            };
            
            var result = await  _data.User.Create(user, password);

            if (!result)
                throw new Exception();
            
            await  _data.User.AddToRole(user, "teacher");
            
            await _data.User.SignIn(user, false);
            
            return await _jwt.GenerateJwt(user);
        }
        
    }
}