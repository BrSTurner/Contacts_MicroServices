using FIAP.Inquiry.Application.Commands;
using FIAP.MessageBus;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.Mediator;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MediatR;

namespace FIAP.Inquiry.Application.Handlers
{
    public class InquiryAllContactsCommandHandler : CommandHandler, IRequestHandler<InquiryAllContactsCommand, List<Contact?>>
    {
        private readonly IMessageBus _bus;

        public InquiryAllContactsCommandHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task<List<Contact?>> Handle(InquiryAllContactsCommand request, CancellationToken cancellationToken)
        {
            var result = await _bus.RequestAsync<QueryAllContactsIntegrationEvent, QueryContactsResponse>(new ());

            return result.Contacts;
        }
    }
}
