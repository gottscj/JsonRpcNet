﻿using JsonRpcNet.Docs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            var info = new JsonRpcInfo
            {
                Description = "Api for JsonRpc chat",
                Title = "Chat API",
                Version = "v1",
                Contact = new JsonRpcContact
                {
                    Name = "The Dude",
                    Email = "the@dude.com",
                    Url = "http://www.thedude.com"
                }
            };
            app.UseJsonRpcApi(info);
            
            app.UseWebSockets();
            app.AddJsonRpcService<ChatJsonRpcWebSocketService>();
            var doc = new JsonRpcDoc(info);
            var serviceDoc = DocGenerator.GenerateJsonRpcServiceDoc(typeof(ChatJsonRpcWebSocketService), doc);

            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}