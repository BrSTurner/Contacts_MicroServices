using FIAP.SharedKernel.Constants;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.Messages;
using FluentValidation.Results;

namespace FIAP.Inquiry.Application.Commands
{
    public class InquiryContactCommand : CommandResult<List<Contact?>>
    {
        public int PhoneCode { get; set; }

        public override bool IsValid()
        {
            var isValid = PhoneCodes.IsCodeValid(this.PhoneCode);

            if (!isValid)
                ValidationResult.Errors.Add(new ValidationFailure("PhoneCode", "Invalid phone code"));

            return isValid;
        }
    }
}
