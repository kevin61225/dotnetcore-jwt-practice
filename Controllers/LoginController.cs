using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JwtAuthDemo.Models;
using JwtAuthDemo.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtHelpers jwtHelper;
        private readonly ILogger<LoginController> logger;

        public LoginController(ILogger<LoginController> logger, JwtHelpers jwtHelper)
        {
            this.logger = logger;
            this.jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost("~/signin")]
        public IActionResult SignIn(LoginViewModel loginModel)
        {
            var userId = "some.id.here";
            var result = new LoginResult()
            {
                Token = jwtHelper.GenerateToken(loginModel.UserName, userId)
            };
            if (ValidateUser(loginModel))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        private bool ValidateUser(LoginViewModel loginModel)
        {
            /* TODO: Do validation here
             *       You can check if the user is valid or not via Database or other authentication APIs    
             */
            return true; 
        }

        /// <summary>
        /// Return  all items from `User.Claims`
        /// </summary>
        /// <returns>Items from `User.Claims`</returns>
        [Authorize]
        [HttpGet("~/claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        /// <summary>
        /// Get user name from `User.Identity`
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("~/username")]
        public IActionResult GetUserName()
        {
            var result = new { Name = User.Identity.Name };
            return Ok(result);
        }

        
        /// <summary>
        /// Get JWT Id from `User.Claims`
        /// </summary>
        /// <returns>JWT Id</returns>
        [Authorize]
        [HttpGet("~/jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }

        /// <summary>
        /// Get a new token based on existed JWT
        /// </summary>
        /// <param name="RefreshToken">RefreshToken</param>
        /// <returns>a new JWT token</returns>
        [Authorize]
        [HttpPost("~/refresh-token")]
        public IActionResult RefreshToken(RefreshTokenViewModel tokenModel)
        {
            var refreshTokenFromDb = "ecloudvalley";
            var userInDB = "kevin.tu";

            // 確認身份是否存在後再發行 JWT
            if (User.Identity.Name == userInDB && refreshTokenFromDb == tokenModel.RefreshToken)
            {
                var result = new LoginResult()
                {
                    Token = jwtHelper.GenerateToken(User.Identity.Name,
                                                    User.Claims.FirstOrDefault(p => p.Type == "UserId").ToString())
                };

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}