using FluentValidation;

namespace Application.Validation
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contains 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contains 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contains a number")
                .Matches("[^A-Za-z0-9]").WithMessage("Password must contains a special character");
            return options;
        }
    }
}
