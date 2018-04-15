using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(politiekeBarometer.Startup))]
namespace politiekeBarometer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}