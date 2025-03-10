﻿using Common;
using Common.Exceptions;
using Common.Utilities;
using Contractor.Models;
using Data.Repositories;
using ElmahCore;
using Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sentry;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;

namespace Contractor.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UserController  : ControllerBase
    {
        private IUserRepository userRepository;
        private ILogger<UserController> logger;
        private IJwtService jwtService;

        //public UserController(IUserRepository userRepository) {
        //    this.userRepository = userRepository;

        //}
        public UserController(IUserRepository userRepository, ILogger<UserController> logger , IJwtService jwtService )
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
        }


        [HttpGet]
        [Authorize]
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

        [HttpGet("[action]")]
        public async Task<string> Token(string username,string password, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUserAndPass(username, password, cancellationToken); //jwtService.GenerateAsync(user);
            if (user == null)
            {
                logger.LogError("User Not Found");
                throw new BadRequestException("UserName or Password Not Found");
            }

            var jwt = jwtService.GenerateAsync(user);
            return await jwt;
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
        public async Task<ApiResult<User>> Update(Guid id,User user, CancellationToken cancellationToken)
        {
            throw new Exception("this is test in Update Action for new Config sentri.io");

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
        public async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
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
