using EMP;
using EMP.Models;
using Hangfire;
using JWT.Builder;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
[assembly: OwinStartup(typeof(EMP.Startup))]
namespace EMP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            #region old
            //ConfigureOAuth(app);
            //ConfigureOAuthTokenConsumption(app);

            //var config = new HttpConfiguration();
            //WebApiConfig.Register(config);
            //app.UseWebApi(config);

            //var formatters = config.Formatters;
            //var jsonFormatter = formatters.JsonFormatter;
            //var settings = jsonFormatter.SerializerSettings;
            //settings.Formatting = Formatting.Indented;
            //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            #endregion

            ConfigureOAuth(app);
            ConfigureOAuthTokenConsumption(app);

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            string connectionString = ConfigurationManager.ConnectionStrings["EMBContext"].ConnectionString;

            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate("cleanup-screenshots", () => new ScreenshotCleanupJob().DeleteOldScreenshots(), Cron.Daily);
        }
    }
}