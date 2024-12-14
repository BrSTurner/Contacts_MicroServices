using FIAP.Inquiry.Application.Commands;
using FIAP.MessageBus;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.Mediator;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MediatR;

namespace FIAP.Inquiry.Application.Handlers
{
    public class InquiryContactByPhoneCodeCommandHandler : CommandHandler, IRequestHandler<InquiryContactByPhoneCodeCommand, List<Contact?>>
    {
        private readonly IMessageBus _bus;

        public InquiryContactByPhoneCodeCommandHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task<List<Contact?>> Handle(InquiryContactByPhoneCodeCommand request, CancellationToken cancellationToken)
        {
            var result = await _bus.RequestAsync<QueryContactByPhoneCodeIntegrationEvent, QueryContactsResponse>(new QueryContactByPhoneCodeIntegrationEvent
            {
                PhoneCode = request.PhoneCode,
            });

            return result.Contacts;
        }
    }
}
