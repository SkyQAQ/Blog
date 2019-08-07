using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Blog.Core.Common;
using Blog.Core.Common.MiddleWare;
using Blog.Core.Common.Quartz;
using Blog.Core.Common.SignalR;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Blog.Core
{
    /// <summary>
    /// 启动文件
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// log4net日志
        /// </summary>
        public static ILoggerRepository Repository { get; set; }

        /// <summary>
        /// 配置文件
        /// </summary>
        public static ConfigHelper _config = new ConfigHelper();
        /// <summary>
        /// Security配置文件
        /// </summary>
        public static ConfigHelper _s_config = new ConfigHelper(Constants.SecurityCfgPath);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //加载log4net日志配置文件
            Repository = LogManager.CreateRepository(Constants.LogRepositoryName);
            XmlConfigurator.Configure(Repository, new FileInfo(Constants.LogConfigFileName));
            new QuartzHelper().Start();
        }

        /// <summary>
        /// 配置属性
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                // 50000000
                options.ValueLengthLimit = _config.Upload_MaxLength * 1000000;
                options.MultipartBodyLengthLimit = _config.Upload_MaxLength * 1000000;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = "API",//Audience
                        ValidIssuer = "Blog",//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_s_config.Token_Key))//拿到SecurityKey
                    };
                });
            services.AddMvc(options => {
                options.Filters.Add(typeof(ResultHandlerMiddleWare));
                //options.RespectBrowserAcceptHeader = true;
            });
            services.AddCors(options => options.AddPolicy("BlogPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins(_config.OriginServer.Split(';'))
                       .AllowCredentials();
            }));
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "WWYY",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "陈阳", Email = "965929770@qq.com", Url = "www.baidu.com" }
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Blog.Core.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(basePath, "Blog.Core.Model.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath, true);//默认的第二个参数是false，这个是Model的注释，记得修改

                var xmlBizPath = Path.Combine(basePath, "Blog.Core.Biz.xml");//这个就是Biz层的xml文件名
                c.IncludeXmlComments(xmlBizPath, true);//默认的第二个参数是false，这个是Biz的注释，记得修改
            });
            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //跨域支持
            app.UseCors("BlogPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
            app.UseMiddleware(typeof(AuthorizeHandlerMiddleWare));
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "api/{controller}/{id}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                //设置不限制content-type
                ServeUnknownFileTypes = true,
                //ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                //{
                //    { ".apk","application/vnd.android.package-archive"},
                //    { ".nupkg","application/zip"}
                //})
            });

            //var physicalFileSystem = new PhysicalFileSystem(webPath);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                //StaticFileSystem = physicalFileSystem
            };
            //options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
            app.UseFileServer(options);
        }
    }
}
