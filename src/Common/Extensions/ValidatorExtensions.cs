using Common.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    public static class ValidatorExtensions
    {
        public static IServiceCollection RegisterValidatorsAsServices(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(c => c != typeof(ValidatorExtensions).Assembly).SelectMany(a => a.GetExportedTypes()).Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatable<>)))
                .ToList();

            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions() { ValidateScopes = true }).CreateScope();

            foreach (var type in types)
            {
                var typeConstructorArgumentLength = type.GetConstructors().First().GetParameters().Length;
                var model = Activator.CreateInstance(type, new object[typeConstructorArgumentLength]);

                var methodInfo = type.GetMethod(nameof(IValidatable<object>.ValidateApplicationModel));

                if (model != null)
                {
                    var methodArgument = Activator.CreateInstance(typeof(BaseValidationProvider<>).MakeGenericType(type), serviceProvider);
                    var validator = methodInfo?.Invoke(model, new[] { methodArgument });

                    if (validator != null)
                    {
                        var interfaces = validator.GetType().GetInterfaces();

                        var validatorInterface = interfaces
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                        if (validatorInterface != null)
                            services.AddScoped(validatorInterface, _ => validator);
                    }
                }
            }
            return services;
        }
    }
}