using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(StoreWebUI.Areas.Identity.IdentityHostingStartup))]

namespace StoreWebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}