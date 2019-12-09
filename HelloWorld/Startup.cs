using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HelloWorld.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelloWorld
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //    services.AddEntityFrameworkSqlite()
            //.AddDbContext<HelloWorldDBContext>
            //    (options => options.UseSqlite(Configuration["database:connection"]));
            // 使用第二种方式 操作数据
            services.AddEntityFrameworkSqlite().AddDbContext<HelloWorldDBContext>();
            // 添加Identigy服务
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<HelloWorldDBContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //调试模式
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 添加中间件
            //UseDefaultFiles 中间件

            //app.UseWelcomePage();
            // 静态文件中间件
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            //可以替换上述中间件
            app.UseFileServer();

            app.UseAuthentication();
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(ConfigureRoute);

            // 中间件是一种装配到应用程序管道以处理请求和响应的组件

            // 注册中间件的 终端，在它之后，永远不会调用下一个中间件
            //app.Run(async (context) =>
            //{
            //    //throw new Exception("Throw Exception");
            //    var msg = Configuration["message"];
            //    await context.Response.WriteAsync(msg);
            //});
        }

        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            //Home/Index 
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }

    }
}
