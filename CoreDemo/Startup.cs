using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.App_Code;
using CoreDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreDemo
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin",
                    builder =>
                    {
                        //builder.WithOrigins("http://localhost:5000").AllowAnyHeader();
                        builder.SetIsOriginAllowed((string arg) =>
                        {
                            //Console.WriteLine(arg);
                            return true;
                        });
                    });
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "MsSystem API",
                    Description = "A simple example ASP.NET Core Web API"
                    //TermsOfService = "None",
                    //Contact = new Contact { Name = "Shayne Boyer", Email = "", Url = "http://twitter.com/spboyer" },
                    //License = new License { Name = "Use under LICX", Url = "http://url.com" }
                });

                //Determine base path for the application.  
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(basePath, "MsSystem.API.xml");
                options.IncludeXmlComments(xmlPath, true);

                //添加header验证信息
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                options.AddSecurityRequirement(security);
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });

            #region Token
            services.AddSingleton<IMemoryCache>(factory =>
               {
                   var cache = new MemoryCache(new MemoryCacheOptions());
                   return cache;
               });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("System", policy => policy.RequireClaim("SystemType").Build());
                options.AddPolicy("Client", policy => policy.RequireClaim("ClientType").Build());
                options.AddPolicy("Admin", policy => policy.RequireClaim("AdminType").Build());
            });
            #endregion


            //services.AddEntityFrameworkSqlite()
            //    .AddDbContext<MyDbContext>(options => options.UseSqlite(Configuration["database:connection"]));

            services.AddEntityFrameworkSqlite().AddDbContext<MyDbContext>();

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<MyDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigin");

            //app.UseWelcomePage();

            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            app.UseFileServer();

            app.UseAuthentication();

            #region Swagger
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwagger(c =>
            //{
            //    c.RouteTemplate = "docs/{documentName}/docs.json";//使中间件服务生成Swagger作为JSON端点(此处设置是生成接口文档信息，可以理解为老技术中的webservice的soap协议的信息，暴露出接口信息的地方)
            //    //c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Info.Description = httpReq.Path);//请求过滤处理
            //});
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = "docs";//设置文档首页根路径
                //c.SwaggerEndpoint("/docs/v1/docs.json", "V1");//此处配置要和UseSwagger的RouteTemplate匹配
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");//默认终结点
                //c.InjectStylesheet("/swagger-ui/custom.css");//注入style文件
            }); 
            #endregion

            #region TokenAuth
            app.UseMiddleware<TokenAuth>();
            #endregion

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(ConfigRoute);

            // app.Run(async (context) =>
            //     {
            //         var msg = Configuration["message"];
            //         await context.Response.WriteAsync(msg);
            //         //await context.Response.WriteAsync("Hello World!");
            //     });

        }

        public void ConfigRoute(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
