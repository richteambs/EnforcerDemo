using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnforcerDI.Entities;
using EnforcerDI.PIP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rsk.Enforcer;
using Rsk.Enforcer.PDP;
using Rsk.Enforcer.PEP;

namespace EnforcerDI
{
    public static class Program
    {
        private static IHost host;

        public static async Task Main(string[] args)
        {
            host = CreateHostBuilder(args).Build();

            await RunEvaluationsForUser(User.StandardUser);
            await RunEvaluationsForUser(User.SupplierUser);
        }

        private static async Task RunEvaluationsForUser(User user)
        {
            using var scope = host.Services.CreateScope();

            var pe = scope.ServiceProvider.GetRequiredService<PolicyExecutor>();
            var locations = await pe.GetPermittedLocations(user);
            Console.WriteLine($"There are {locations.Count()} permitted locations for {user.UserName}");
        }

        private static Action<HostBuilderContext, IServiceCollection> ConfigureServices()
        {
            return (hostBuilderContext, services) =>
            {
                var licensee = hostBuilderContext.Configuration["enforcer:licensee"];
                var licenseKey = hostBuilderContext.Configuration["enforcer:licenseKey"];
                var policyRootDirectory = Path.Join(hostBuilderContext.HostingEnvironment.ContentRootPath, "policies");
                services
                    .AddSingleton<SupplierPermissionsCache>()
                    .AddScoped<PolicyExecutor>()
                    .AddScoped<LocationRepository>();
                services.AddEnforcer("MuddyBoots.Global", o =>
                    {
                        o.PolicyInformationPointFailureBehavior = PolicyInformationPointFailureBehavior.FailFast;
                        o.Licensee = licensee;
                        o.LicenseKey = licenseKey;
                    })
                    .AddFileSystemPolicyStore(policyRootDirectory)
                    .AddPolicyAttributeProvider<SupplierLocationAttributeValueProvider>()
                    .AddPolicyEnforcementPoint(o => o.Bias = PepBias.Deny);
            };
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(builder =>
                {
                    builder.SetBasePath(AppContext.BaseDirectory);
                    builder.AddUserSecrets<PolicyExecutor>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices(ConfigureServices());
        }
    }
}
