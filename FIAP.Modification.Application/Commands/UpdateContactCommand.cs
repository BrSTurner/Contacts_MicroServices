using FIAP.SharedKernel.DTO;
using FIAP.SharedKernel.Messages;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

namespace FIAP.Modification.Application.Commands
{
    public class UpdateContactCommand : Command
    {
        public Guid ContactId { get; init; }

        public ContactDTO Contact { get; init; }

        public override bool IsValid()
        {
            var isValid = ContactId != Guid.Empty;

            if (!isValid)
                ValidationResult.Errors.Add(new ValidationFailure("ContactId", "ContactId must contain a valid value"));

            return isValid;
        }
    }
}
