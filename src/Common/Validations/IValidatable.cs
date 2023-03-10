using FluentValidation;

namespace Common.Validations;

public interface IValidatable<TModel> where TModel : class
{
    IValidator<TModel> ValidateApplicationModel(BaseValidationProvider<TModel> validator);
}