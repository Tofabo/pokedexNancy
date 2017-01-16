using Microsoft.AspNetCore.Builder;
using Nancy.Owin;
using Microsoft.Extensions.Logging;

namespace PokeInfo
{
    public class Startup
    {
        public void Configure(IApplicationBuilder App, ILoggerFactory LoggerFactory)
        {
            App.UseOwin(x => x.UseNancy());
            LoggerFactory.AddConsole();
        }
    }
}
