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
using System.Linq;
using BryceFamily.Repo.Core.Read.Story;
using BryceFamily.Repo.Core.Write.Story;
using AspNetCore.Identity.DynamoDB;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Amazon.DynamoDBv2;
using Amazon;

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

            services.AddScoped<IStoryReadRepository, StoryReadRepository>();
            services.AddScoped<IWriteRepository<StoryContent, Guid>, StoryWriteRepository<StoryContent, Guid>>();


            services.AddSingleton<IAWSClientFactory, AWSClientFactory>();

            services.AddSingleton(context => new DynamoDBOperationConfig()
            {
                TableNamePrefix = "familybryce."
            });

            services.AddSingleton(context => new CDNServiceRoot("https://s3-ap-southeast-2.amazonaws.com"));

            services.AddScoped<ClanAndPeopleService>();
            services.AddScoped(context => new ContextService
            {
                LoggedInPerson = context.GetService<ClanAndPeopleService>().People.FirstOrDefault(t => t.FirstName == "Stuart"),
                EditMode = true,
                IsClanManager = false,
                IsSuperUser = true

            });

            services.AddDynamoDBIdentity<DynamoIdentityUser, DynamoIdentityRole>()
                .AddUserStore()
                .AddRoleStore()
                .AddRoleUsersStore();
                

            services.Configure<IdentityOptions>(options =>
            {
                var dataProtectionPath = Path.Combine(HostingEnvironment.WebRootPath, "identity-artifacts");
                options.Lockout.AllowedForNewUsers = true;
            });

            services.Configure<DynamoDbSettings>(Configuration.GetSection("DynamoDB"));

            //AddDefaultTokenProviders(services);

            // Services used by identity
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/LogIn";
            });


            services.AddOptions();
            services.AddDataProtection();

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMemoryCache();



        }

        private void AddDefaultTokenProviders(IServiceCollection services)
        {
            var dataProtectionProviderType = typeof(DataProtectorTokenProvider<>).MakeGenericType(typeof(DynamoIdentityUser));
            var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<>).MakeGenericType(typeof(DynamoIdentityUser));
            var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(typeof(DynamoIdentityUser));
            AddTokenProvider(services, TokenOptions.DefaultProvider, dataProtectionProviderType);
        }


        private void AddTokenProvider(IServiceCollection services, string providerName, Type provider)
        {
            services.Configure<IdentityOptions>(
                options => { options.Tokens.ProviderMap[providerName] = new TokenProviderDescriptor(provider); });

            services.AddSingleton(provider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

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

            var options = app.ApplicationServices.GetService<IOptions<DynamoDbSettings>>();
            var client =  new AmazonDynamoDBClient(RegionEndpoint.APSoutheast2);

            var context = new DynamoDBContext(client);

            var userStore = app.ApplicationServices
                    .GetService<IUserStore<DynamoIdentityUser>>()
                as DynamoUserStore<DynamoIdentityUser, DynamoIdentityRole>;
            var roleStore = app.ApplicationServices
                    .GetService<IRoleStore<DynamoIdentityRole>>()
                as DynamoRoleStore<DynamoIdentityRole>;
            var roleUsersStore = app.ApplicationServices
                .GetService<DynamoRoleUsersStore<DynamoIdentityRole, DynamoIdentityUser>>();

            userStore.EnsureInitializedAsync(client, context, options.Value.UsersTableName).Wait();
            roleStore.EnsureInitializedAsync(client, context, options.Value.RolesTableName).Wait();
            roleUsersStore.EnsureInitializedAsync(client, context, options.Value.RoleUsersTableName).Wait();
        }
    }

    public class DynamoDbSettings
    {
        public string UsersTableName { get; set; }
        public string RolesTableName { get; set; }
        public string RoleUsersTableName { get; set; }
    }
}
