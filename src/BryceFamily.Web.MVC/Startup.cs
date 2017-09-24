using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Repository;
using BryceFamily.Repo.Core.Files;

namespace BryceFamily.Web.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(context => new MockRepo<FamilyEvent, Guid>(GetMockData()));
            services.AddSingleton(context => (IReadModel<FamilyEvent, Guid>)context.GetService<MockRepo<FamilyEvent, Guid>>());
            services.AddSingleton(context => (IWriteModel<FamilyEvent, Guid>)context.GetService<MockRepo<FamilyEvent, Guid>>());

            services.AddSingleton(context => new GalleryMockRepo<Gallery, Guid>(GetMockGalleries()));
            services.AddSingleton(context => (IReadModel<Gallery, Guid>)context.GetService<GalleryMockRepo<Gallery, Guid>>());
            services.AddSingleton(context => (IWriteModel<Gallery, Guid>)context.GetService<GalleryMockRepo<Gallery, Guid>>());

            services.AddSingleton<IFileService>(new MockFileService(HostingEnvironment.WebRootPath));

            services.AddSingleton(context => new MockPeopleService<Repo.Core.Model.Person, Guid>());
            services.AddSingleton(context => (IReadModel<Repo.Core.Model.Person, Guid>)context.GetService<MockPeopleService<Repo.Core.Model.Person, Guid>>());
            services.AddSingleton(context => (IWriteModel<Repo.Core.Model.Person, Guid>)context.GetService<MockPeopleService<Repo.Core.Model.Person, Guid>>());


        }

        private static List<Gallery> GetMockGalleries()
        {
            var dummyData = new List<Gallery>()
            {
                new Gallery()
                {
                    ID = new Guid("af4356dd-34fd-a3e2-2222-1efa3eaa149f"),
                    Name = "A Gallery",
                    Summary = "An image gallery of stuff with pictures of stuff in it",
                    Owner = new Person()
                    {
                        ID = Guid.NewGuid(),
                        FirstName = "Brian",
                        LastName = "Something"
                    },
                    DateCreated = DateTime.Now.AddDays(-36),
                    ImageReferences = new List<ImageReference>()
                    {
                        new ImageReference()
                        {
                            ID = new Guid("33919032-e1bc-480b-8c8e-40c88299706e"),
                            ImageLocation = "images/galleries/af4356dd-34fd-a3e2-2222-1efa3eaa149f",
                            MimeType = "image/jpg",
                            Title = "WIN_20151130_20_12_41_Pro.jpg"
                        }
                    }
                }
            };

            return dummyData;
        }

        private static List<FamilyEvent> GetMockData()
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
