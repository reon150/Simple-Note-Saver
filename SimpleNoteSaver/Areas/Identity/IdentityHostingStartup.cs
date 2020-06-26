using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SimpleNoteSaver.Areas.Identity.IdentityHostingStartup))]
namespace SimpleNoteSaver.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}