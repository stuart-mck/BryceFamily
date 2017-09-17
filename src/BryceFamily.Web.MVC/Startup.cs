﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Repository;

namespace BryceFamily.Web.MVC
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
            services.AddMvc();

            services.AddSingleton(context =>
            {
                return new MockRepo<FamilyEvent, Guid>(GetMockData());
            });
            services.AddSingleton(context => (IReadModel<FamilyEvent, Guid>)context.GetService<MockRepo<FamilyEvent, Guid>>());
            services.AddSingleton(context => (IWriteModel<FamilyEvent, Guid>)context.GetService<MockRepo<FamilyEvent, Guid>>());

        }

        private List<FamilyEvent> GetMockData()
        {
            var dummyData = new List<FamilyEvent>();
            dummyData.Add(new FamilyEvent()
            {
                ID = Guid.NewGuid(),
                Details = "This is a test event",
                EndDate = new DateTime(2018, 3, 12, 12, 00, 00),
                StartDate = new DateTime(2018, 3, 10, 12, 00, 00),
                EventStatus = EventStatus.Pending,
                EventType = EventType.Gathering,
                Title = "A new event",
                Location = new EventLocation()
                {
                    Title = "the car park"
                }
            });


            return dummyData;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
