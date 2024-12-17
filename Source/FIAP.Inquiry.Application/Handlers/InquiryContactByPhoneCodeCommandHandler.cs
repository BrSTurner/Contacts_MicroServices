using FIAP.Inquiry.Application.Commands;
using FIAP.MessageBus;
using FIAP.SharedKernel.DTO;
using FIAP.SharedKernel.Mediator;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MediatR;

namespace FIAP.Inquiry.Application.Handlers
{
    public class InquiryContactByPhoneCodeCommandHandler : CommandHandler, IRequestHandler<InquiryContactByPhoneCodeCommand, List<ContactDTO>>
    {
        private readonly IMessageBus _bus;

        public InquiryContactByPhoneCodeCommandHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task<List<ContactDTO>> Handle(InquiryContactByPhoneCodeCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
                return [];

            var result = await _bus.RequestAsync<QueryContactByPhoneCodeIntegrationEvent, QueryContactByPhoneCodeResponse>(new QueryContactByPhoneCodeIntegrationEvent
            {
                PhoneCode = request.PhoneCode,
            });

            return result.Contacts;
        }
    }
}
