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
        private readonly ILogger<ConfigController> logger;
        public ConfigController(ILogger<ConfigController> logger, IOptions<JwtSettings> jwtSettings)
        {
            this.logger = logger;
            this.JwtSettings = jwtSettings.Value;
        }

        [HttpGet("")]
        public ActionResult<string> GetSettings()
        {
            logger.LogTrace("Level 1");
            logger.LogDebug("Level 2");
            logger.LogInformation("Level 3");
            logger.LogWarning("Level 4");
            logger.LogError("Level 5");
            logger.LogCritical("Level 6");
            
            return Ok(this.JwtSettings);
        }
    }
}