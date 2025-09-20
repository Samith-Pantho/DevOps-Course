using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Service1.BLL.Repositories;
using Service1.BLL.Services;
using Service1.DAL.Repositories;
using Service1.DAL.Services;

namespace Service1.API.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IDalService1Repository, DalService1Service>();
            services.AddScoped<IService1Repository, Service1Service>();

            services.AddScoped<IDalLogRepository, DalLogService>();

        }
    }
}
