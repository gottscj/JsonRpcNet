using System.Collections.Generic;
using JsonRpcNet.Docs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonRpcNet.AspNetCore.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            services.AddWebSocketHandlers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseJsonRpcApi(new JsonRpcInfoDoc
            {
                Description = "Api for JsonRpc chat",
                Title = "Chat API",
                Version = "v1",
                Contact = new ContactDoc
                {
                    Email = "test@test.com"
                }
            });
            
            app.UseWebSockets();
            app.AddJsonRpcService<ChatJsonRpcWebSocketService>();
            var doc = new JsonRpcDoc();
            var serviceDoc = DocGenerator.GenerateJsonRpcServiceDoc(typeof(ChatJsonRpcWebSocketService), doc);

            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}