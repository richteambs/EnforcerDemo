// Copyright (c) 2021 Muddy Boots Software Ltd.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
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
    public class PolicyBenchmarks
    {
        private IHost _host;
        private IServiceProvider _services;
        private IServiceScope _scope;

        [GlobalCleanup]
        public void Cleanup()
        {
            _scope?.Dispose();
            _host?.Dispose();
        }

        [Benchmark(Baseline = true)]
        public async Task HardCodedStandardUser()
        {
            await RunHardCodedPolicyForUser(User.StandardUser);
        }

        [Benchmark]
        public async Task HardCodedSupplierUser()
        {
            await RunHardCodedPolicyForUser(User.RestrictedUser);
        }

        [Benchmark]
        public async Task EnforcerStandardUser()
        {
            await RunEnforcerPolicyForUser(User.StandardUser);
        }

        [Benchmark]
        public async Task EnforcerSupplierUser()
        {
            await RunEnforcerPolicyForUser(User.RestrictedUser);
        }

        private async Task RunEnforcerPolicyForUser(User user)
        {
            var pe = _scope.ServiceProvider.GetRequiredService<EnforcerPolicyExecutor>();
            var locations = await pe.GetPermittedLocations(user);
            Console.WriteLine($"There are {locations.Count()} permitted locations for {user.UserName}");
        }

        private async Task RunHardCodedPolicyForUser(User user)
        {
            var pe = _scope.ServiceProvider.GetRequiredService<EnforcerPolicyExecutor>();
            var locations = await pe.GetPermittedLocations(user);
            Console.WriteLine($"There are {locations.Count()} permitted locations for {user.UserName}");
        }

        [GlobalSetup]
        public void Setup()
        {
            _host = CreateHostBuilder(Array.Empty<string>()).Build();
            _scope = _host.Services.CreateScope();
            _services = _scope.ServiceProvider;
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(builder =>
                {
                    builder.SetBasePath(AppContext.BaseDirectory);
                    builder.AddUserSecrets<EnforcerPolicyExecutor>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices(ConfigureServices());
        }

        private static Action<HostBuilderContext, IServiceCollection> ConfigureServices()
        {
            return (hostBuilderContext, services) =>
            {
                var licensee = hostBuilderContext.Configuration["enforcer:licensee"];
                var licenseKey = hostBuilderContext.Configuration["enforcer:licenseKey"];
                var policyRootDirectory = Path.Join(hostBuilderContext.HostingEnvironment.ContentRootPath, "policies");
                services
                    .AddSingleton<UserLocationPermissionsCache>()
                    .AddScoped<EnforcerPolicyExecutor>()
                    .AddScoped<HardCodedPolicyExecutor>()
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
    }
}
