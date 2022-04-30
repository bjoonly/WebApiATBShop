using ATBShop.Models;
using DAL.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ATBShop.Validators
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        private readonly UserManager<AppUser> _userManager;
        public RegisterViewModelValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            //First name
            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required!")
                .MinimumLength(2).WithMessage("{PropertyName} must be at least 2 characters!");

            //Second Name
            RuleFor(x => x.SecondName).Cascade(CascadeMode.Stop)
           .NotEmpty().WithMessage("{PropertyName} is required!")
           .MinimumLength(2).WithMessage("{PropertyName} must be at least 2 characters!");

            //Email
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage("{PropertyName} is required!")
               .EmailAddress().WithMessage("Invalid format of {PropertyName}!")
               .DependentRules(() =>
               {
                   RuleFor(x => x.Email).Must(BeUniqueEmail)

                    .WithMessage("This {PropertyName} is already registered!");
               });

            //Phone
            RuleFor(x => x.Phone).Cascade(CascadeMode.Stop)
              .NotEmpty().WithName("Phone number").WithMessage("{PropertyName} is required!")
              .Matches(@"^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$")
              .WithMessage("Invalid format of {PropertyName}!");

            //Password
            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required!")
                .MinimumLength(5).WithMessage("{PropertyName} must be at least 5 characters!");

            //Confirm password
            RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.Stop)
                .NotEmpty().WithName("Confirm Password").WithMessage("{PropertyName} is required!")
                 .Equal(x => x.Password).WithMessage("Password and {PropertyName} do not match!");
        }
        private bool BeUniqueEmail(string email)
        {
            return _userManager.FindByEmailAsync(email).Result == null;
        }
    }
}
