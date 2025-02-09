using FIAP.MessageBus;
using FIAP.Registration.Application.Commands;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.Mediator;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using FluentValidation.Results;
using MediatR;

namespace FIAP.Registration.Application.Handlers
{
    public class CreateContactCommandHandler : CommandHandler, IRequestHandler<CreateContactCommand, ValidationResult>
    {
        private readonly IMessageBus _bus;

        public CreateContactCommandHandler(
            IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            EnsureCommandIsValid(request);

            if (!request.IsValid())
                return request.ValidationResult;

            if (await IsContactAlreadyRegistered(request))
            {
                AddError("A Contact with the same E-mail or Phone Number was already created");
                return ValidationResult;
            }

            var createContactEvent = new CreateContactIntegrationEvent(CreateContact(request));

            await _bus.PublishAsync(createContactEvent);

            return ValidationResult;
        }

        private static void EnsureCommandIsValid(CreateContactCommand contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact), "Contact must be correctly filled");
        }

        private async Task<bool> IsContactAlreadyRegistered(CreateContactCommand contact)
        {
            var request = new QueryContactByEmailOrPhoneIntegrationEvent
            {
                Email = contact.Email,
                Phone = contact.PhoneNumber,
                PhoneCode = contact.PhoneCode
            };

            var existentContactQuery = await _bus.RequestAsync<QueryContactByEmailOrPhoneIntegrationEvent, QueryContactResponse>(request);

            return existentContactQuery.Contact != null;
        }

        private static Contact CreateContact(CreateContactCommand request)
        {
            return Contact.Create(
                request.Name,
                request.Email,
                request.PhoneCode,
                request.PhoneNumber);
        }
    }
}
