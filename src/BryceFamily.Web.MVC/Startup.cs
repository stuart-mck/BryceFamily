using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Repo.Core.Write.People;
using BryceFamily.Repo.Core.AWS;
using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.Read.People;
using BryceFamily.Repo.Core.FamilyEvents;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using BryceFamily.Repo.Core.Write.Gallery;
using BryceFamily.Repo.Core.Read.Gallery;
using BryceFamily.Repo.Core.Write.ImageReference;
using BryceFamily.Repo.Core.Read.ImageReference;

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

            
            services.AddScoped<IFamilyEventReadRepository, FamilyEventReadRepository>();
            services.AddScoped<IWriteRepository<FamilyEvent, Guid>, FamilyEventWriteRepository<FamilyEvent, Guid>>();

            services.AddScoped<IGalleryReadRepository, GalleryReadRepository>();
            services.AddScoped<IWriteRepository<Gallery, Guid>, GalleryWriteRepository<Gallery, Guid>>();

            services.AddScoped<IImageReferenceReadRepository, ImageReferenceReadRepository>();
            services.AddScoped<IWriteRepository<ImageReference, Guid>, ImageReferenceWriteRepository<ImageReference, Guid>>();

            services.AddScoped<IFileService>(context => new S3Service(context.GetRequiredService<IAWSClientFactory>(), "familybryce.gallery"));

            services.AddScoped<IPersonReadRepository, PeopleReadRepository>();
            services.AddScoped<IUnionReadRepository, UnionReadRepository>();
            services.AddScoped<IWriteRepository<Person, Guid>, PeopleWriteRepository<Person, Guid>>();
            services.AddScoped<IWriteRepository<Union, Guid>, UnionWriteRepository<Union, Guid>>();

            services.AddSingleton<IAWSClientFactory, AWSClientFactory>();

            services.AddSingleton(context => new DynamoDBOperationConfig()
            {
                TableNamePrefix = "familybryce."
            });

            services.AddSingleton(context => new CDNServiceRoot("https://s3-ap-southeast-2.amazonaws.com"));


            services.AddScoped<ClanAndPeopleService>();

            services.AddMemoryCache();

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
