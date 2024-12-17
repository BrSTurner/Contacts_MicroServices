using FIAP.MessageBus;
using FIAP.Modification.Application.Commands;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.Mediator;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using FluentValidation.Results;
using MediatR;

namespace FIAP.Modification.Application.Handlers
{
    public class UpdateContactCommandHandler : CommandHandler, IRequestHandler<UpdateContactCommand, ValidationResult>
    {
        private readonly IMessageBus _bus;

        public UpdateContactCommandHandler(IMessageBus bus) 
        {
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
                return request.ValidationResult;

            var contact = await GetContactToUpdate(request);

            if (contact == null)
            {
                AddError("Contact could not be found to be updated");
                return ValidationResult;
            }

            contact.Update(request.Name, request.Email, request.PhoneCode, request.PhoneNumber);

            await _bus.PublishAsync(new UpdateContactIntegrationEvent
            {
                ContactId = contact.Id,
                Contact = contact,
            });

            return ValidationResult;
        }

        private async Task<Contact?> GetContactToUpdate(UpdateContactCommand request)
        {
            var result = await _bus.RequestAsync<QueryContactByIdIntegrationEvent, QueryContactResponse>(new QueryContactByIdIntegrationEvent
            {
                ContactId = request.Id,
            });

            return result.Contact;
        } 
    }
}
