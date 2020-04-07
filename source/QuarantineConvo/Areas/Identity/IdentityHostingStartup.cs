using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuarantineConvo.Data;
using QuarantineConvo.Models;

[assembly: HostingStartup(typeof(QuarantineConvo.Areas.Identity.IdentityHostingStartup))]
namespace QuarantineConvo.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<QuarantineConvoIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("QuarantineConvoIdentityContextConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<QuarantineConvoIdentityContext>();
            });
        }
    }
}