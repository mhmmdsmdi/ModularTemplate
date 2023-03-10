using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Validations;

public class BaseValidationProvider<TModel> : AbstractValidator<TModel>
{
    public IServiceScope ServiceProvider { get; }

    public BaseValidationProvider(IServiceScope serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
}