using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtAuthDemo.Helpers;
using JwtAuthDemo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace JwtAuthDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtAuthDemo", Version = "v1" });
            });

            // 加入 JWT Helper 服務供使用
            services.AddSingleton<JwtHelpers>();

            // 加入 JWT 組態設定並 Map 至 `JwtSettings` 做成強型別
            // 後續於 Controller 時使用 `IOptions<JwtSetting>` 取得內容
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            
            // 加入 JWT 驗證機制
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                        options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉
                        
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                            // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                            // 一般我們都會驗證 Issuer
                            ValidateIssuer = true,
                            ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),
                            

                            // 通常不太需要驗證 Audience
                            ValidateAudience = false,
                            //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

                            // 一般我們都會驗證 Token 的有效期間
                            ValidateLifetime = true,

                            // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                            ValidateIssuerSigningKey = true,

                            // "1234567890123456" 應該從 IConfiguration 取得
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSettings:SignKey")))
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JwtAuthDemo v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // 先驗證身份
            app.UseAuthentication();

            // 再檢查授權
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
