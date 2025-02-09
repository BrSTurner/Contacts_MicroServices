using FIAP.MessageBus;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.Mediator;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using FIAP.Termination.Application.Commands;
using FluentValidation.Results;
using MediatR;

namespace FIAP.Termination.Application.Handlers
{
    public class DeleteContactCommandHandler : CommandHandler, IRequestHandler<DeleteContactCommand, ValidationResult>
    {
        private readonly IMessageBus _bus;

        public DeleteContactCommandHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
                return request.ValidationResult;

            var contact = await GetContactToDelete(request);

            if (contact == null)
            {
                AddError("Contact could not be found to be deleted");
                return ValidationResult;
            }

            await _bus.PublishAsync(new DeleteContactIntegrationEvent
            {
                ContactId = contact.Id
            });

            return ValidationResult;
        }

        private async Task<Contact?> GetContactToDelete(DeleteContactCommand request)
        {
            var result = await _bus.RequestAsync<QueryContactByIdIntegrationEvent, QueryContactResponse>(new QueryContactByIdIntegrationEvent
            {
                ContactId = request.ContactId,
            });

            return result.Contact;
        }
    }
}
