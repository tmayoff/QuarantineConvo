using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuarantineConvo.Areas.Identity.Data;

[assembly: HostingStartup(typeof(QuarantineConvo.Areas.Identity.IdentityHostingStartup))]
namespace QuarantineConvo.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<QuarantineConvoIdentityDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("QuarantineConvoIdentityDbContextConnection")));

                services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<QuarantineConvoIdentityDbContext>();
                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<QuarantineConvoIdentityDbContext>();
            });
        }
    }
}