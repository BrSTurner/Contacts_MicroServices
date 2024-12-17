using FIAP.Modification.Application.Validation;
using FIAP.SharedKernel.Messages;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

namespace FIAP.Modification.Application.Commands
{
    public class UpdateContactCommand : Command
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public required int PhoneCode { get; init; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateContactValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
