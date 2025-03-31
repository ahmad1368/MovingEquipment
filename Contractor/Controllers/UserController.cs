using Common;
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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Services;

namespace Contractor.Controllers
{ 
    public class UserController  :  BaseController
    {
        private IUserRepository userRepository;
        private ILogger<UserController> logger;
        private IJwtService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper Mapper;

        //public UserController(IUserRepository userRepository) {
        //    this.userRepository = userRepository;

        //}
        public UserController(IUserRepository userRepository, ILogger<UserController> logger , IJwtService jwtService,
            UserManager<User> userManager , RoleManager<Role> roleManager, SignInManager<User> signInManager,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.Mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<ApiResult<List<UserDTO>>> Get()
        {
            //var Role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            var users = await userRepository.TableNoTracking.ProjectTo<UserDTO>(Mapper.ConfigurationProvider).ToListAsync();
            return users;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ApiResult<User>> Get(Guid id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
            {
                logger.LogError("User Not Found");
                throw new NotFoundException("User Not Found");
            }

            await userRepository.UpdateSecurityStampAsync(user, cancellationToken);

            return user;
        }


        /// <summary>
        /// This Method Generat JWT Token 
        /// </summary>
        /// <param name="tokenRequest">information of token request</param> 
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<AccessToken> Token([FromForm] TokenRequest tokenRequest,CancellationToken cancellationToken)
        {
            if(!tokenRequest.grant_type.Equals("password",StringComparison.OrdinalIgnoreCase) )
            {
                logger.LogError("OAuth is Not Password");
                throw new BadRequestException("OAuth is Not Password");
            }

            
            var user = await userRepository.GetByUserAndPass(tokenRequest.username, tokenRequest.password, cancellationToken); //jwtService.GenerateAsync(user);
            if (user == null)
            {
                logger.LogError("User Not Found");
                throw new BadRequestException("UserName or Password Not Found");
            }

            var jwt = jwtService.GenerateAsync(user);
            return await jwt;
        }


        [HttpPost]
        
        public async Task<ApiResult<User>> Create(UserDTO userDTO, CancellationToken cancellationToken)
        {

            var newUser = Mapper.Map<User>(userDTO);
            newUser.PasswordHash = SecurityHelper.GetSha256Hash(userDTO.PASSWORD);
            newUser.SecurityStamp = Guid.NewGuid().ToString();
            newUser.ConcurrencyStamp = Guid.NewGuid().ToString();
            newUser.LastLoginDate = DateTime.Now;
            newUser.EmailConfirmed = true;
            newUser.PhoneNumberConfirmed = true;
            newUser.TwoFactorEnabled = true;
            newUser.LockoutEnabled = true;
            newUser.AccessFailedCount = 1;

            await userRepository.AddAsync(newUser, cancellationToken);
            return newUser;

        }

        [HttpPut]
        public async Task<ApiResult<User>> Update(Guid id,UserDTO userDTO, CancellationToken cancellationToken)
        {

            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            Mapper.Map(userDTO, updateUser);

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
