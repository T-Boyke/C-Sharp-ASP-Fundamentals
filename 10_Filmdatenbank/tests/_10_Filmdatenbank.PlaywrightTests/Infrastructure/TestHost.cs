using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;

namespace _10_Filmdatenbank.PlaywrightTests.Infrastructure
{
    public class TestHost<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public string BaseUrl => "http://127.0.0.1:5018";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.UseKestrel(options =>
            {
                options.ListenAnyIP(5018);
            });
            base.ConfigureWebHost(builder);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();
            host.Start();
            return host;
        }
    }
}
