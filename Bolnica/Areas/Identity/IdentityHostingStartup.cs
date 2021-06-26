using System;
using Bolnica.Areas.Identity.Data;
using Bolnica.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Bolnica.Areas.Identity.IdentityHostingStartup))]
namespace Bolnica.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BolnicaContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("BolnicaContext")));

                
            });
        }
    }
}