using FIAP.Modification.Application.Commands;
using FIAP.SharedKernel.Constants;
using FluentValidation;

namespace FIAP.Modification.Application.Validation
{
    public class UpdateContactValidation : AbstractValidator<UpdateContactCommand>
    {
        public UpdateContactValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id must be correctly filled");

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
                .Must(PhoneCodes.IsCodeValid)
                .WithMessage("Phone Code not valid");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number must be correctly filled")
                .Matches(@"^[9]\d{8}$")
                .WithMessage("Phone Number must be in the correct format");
        }
    }
}
