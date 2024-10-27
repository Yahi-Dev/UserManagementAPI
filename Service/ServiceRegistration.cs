using Layer_Service.Interfaces;
using Layer_Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Layer_Service
{
    public static class ServiceRegistration
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            services.AddTransient<IUserService, UserService>();
            #endregion
        }
    }
}
