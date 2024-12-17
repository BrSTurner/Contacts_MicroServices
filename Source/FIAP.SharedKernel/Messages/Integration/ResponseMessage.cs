using FluentValidation.Results;

namespace FIAP.SharedKernel.Messages.Integration
{
    public class ResponseMessage : Message
    {
        public ValidationResult? ValidationResult { get; set; }
    }
}
