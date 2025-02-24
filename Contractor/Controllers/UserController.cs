using Common;
using Common.Utilities;
using Contractor.Models;
using Data.Repositories;
using Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;

namespace Contractor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController  : ControllerBase
    {
        private IUserRepository userRepository;

        public UserController(IUserRepository userRepository ) {
            this.userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ApiResult<List<User>>> Get()
        {
            var users = await userRepository.TableNoTracking.ToListAsync();
            return users;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ApiResult<User>> Get(Guid id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            return user;
        }



        [HttpPost]
        public async Task<ApiResult<User>> Create(UserDTO user, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                
                UserName = user.UserName,
                FullName = user.FullName,
                Age = user.Age,
                IsActive = true,
                Gender = user.Gender,
                LastLoginDate = DateTime.Now,
                EmailConfirmed = true,
                PasswordHash = SecurityHelper.GetSha256Hash( user.Password),
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = true,
                AccessFailedCount = 1
            };

            await userRepository.AddAsync(newUser, cancellationToken);
            return newUser;

        }

        [HttpPut]
        public async Task<ApiResult<User>> Update(int id,User user, CancellationToken cancellationToken)
        {
           var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName ;
            updateUser.FullName = user.FullName ;
            updateUser.PhoneNumber = user.PhoneNumber;
            updateUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            updateUser.Age = user.Age;
            updateUser.Email = user.Email ;
            updateUser.Gender = user.Gender ;
            updateUser.IsActive = user.IsActive ;
            updateUser.LastLoginDate = user.LastLoginDate ;
            updateUser.PasswordHash = user.PasswordHash ;

            await userRepository.UpdateAsync(updateUser, CancellationToken.None);
            return Ok(updateUser);
        }
        [HttpDelete]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, CancellationToken.None);
            return Ok();
        }

        //public ApiResult GetUserName() {

        //    return new ApiResult(true, ApiResultStatusCode.Success, "User Name");
        //}


    }
}
