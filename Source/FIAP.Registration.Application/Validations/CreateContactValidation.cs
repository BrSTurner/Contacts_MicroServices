using FIAP.Registration.Application.Commands;
using FIAP.SharedKernel.Constants;
using FluentValidation;

namespace FIAP.Contacts.Application.Contacts.Validations
{
    public class CreateContactValidation : AbstractValidator<CreateContactCommand>
    {
        public CreateContactValidation() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name must be correctly filled");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("E-mail must be correctly filled")
                .EmailAddress()
                .WithMessage("E-mail must be in the correct format");

            RuleFor(x => x.PhoneCode)
                .NotEmpty()
                .WithMessage("Phone Code must be correctly filled")
                .Must(x => PhoneCodes.IsCodeValid(x))
                .WithMessage("Phone Code not valid");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number must be correctly filled")
                .Matches(@"^[9]\d{8}$")
                .WithMessage("Phone Number must be in the correct format");
        }
    }
}
