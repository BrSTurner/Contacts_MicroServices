using FIAP.SharedKernel.Messages;
using FluentValidation.Results;

namespace FIAP.Termination.Application.Commands
{
    public class DeleteContactCommand : Command
    {
        public Guid ContactId { get; init; }

        public override bool IsValid()
        {
            var isValid = ContactId != Guid.Empty;

            if (!isValid)
                ValidationResult.Errors.Add(new ValidationFailure("ContactId", "ContactId must contain a valid value"));

            return isValid;
        }
    }
}
