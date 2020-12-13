using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JwtAuthDemo.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JwtAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        public JwtSettings JwtSettings { get; }
        private readonly ILogger<LoginController> logger;
        public ConfigController(ILogger<LoginController> logger, IOptions<JwtSettings> jwtSettings)
        {
            this.logger = logger;
            this.JwtSettings = jwtSettings.Value;
        }

        [HttpGet("")]
        public ActionResult<string> GetSettings()
        {
            return Ok(this.JwtSettings);
        }
    }
}