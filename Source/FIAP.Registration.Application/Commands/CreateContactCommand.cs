using FIAP.Contacts.Application.Contacts.Validations;
using FIAP.SharedKernel.Messages;

namespace FIAP.Registration.Application.Commands
{
    public class CreateContactCommand : Command
    {
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public required int PhoneCode { get; init; }

        public override bool IsValid()
        {
            ValidationResult = new CreateContactValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
