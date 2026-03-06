using Microsoft.AspNetCore.Identity.UI.Services;
using Mutqan.BLL.Services.Class;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.Repository.Interface;
namespace Mutqan.PL
{
    public static class AppConfiguration
    {
        public static void Configuration(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSenderService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            var assemblyService = typeof(IScopedService).Assembly;
            services.Scan(s => s.FromAssemblies(assemblyService).AddClasses(c => c.AssignableTo<IScopedService>()).AsImplementedInterfaces().WithScopedLifetime());
            var assemblyRepository = typeof(IScopedRepository).Assembly;
            services.Scan(s => s.FromAssemblies(assemblyRepository).AddClasses(c => c.AssignableTo<IScopedRepository>()).AsImplementedInterfaces().WithScopedLifetime());
        }
    }
}
