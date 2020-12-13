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
            var userId = "ecv6689";
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
            return true; // TODO
        }

        [Authorize]
        [HttpGet("~/claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        [Authorize]
        [HttpGet("~/username")]
        public IActionResult GetUserName()
        {
            var result = new { Name = User.Identity.Name };
            return Ok(result);
        }

        [Authorize]
        [HttpGet("~/jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }

        [Authorize]
        [HttpPost("~/refresh-token")]
        public IActionResult RefreshToken(RefreshTokenViewModel tokenModel)
        {
            var refreshTokenFromDb = "ecloudvalley";
            var userInDB = "kevin.tu";

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