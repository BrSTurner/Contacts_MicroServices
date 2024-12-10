using FIAP.Inquiry.Application.Commands;
using FIAP.MessageBus;
using FIAP.SharedKernel.Mediator;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using FluentValidation.Results;
using MediatR;

namespace FIAP.Inquiry.Application.Handlers
{
    public class InquiryContactCommandHandler : CommandHandler, IRequestHandler<InquiryContactCommand, ValidationResult>
    {
        private readonly IMessageBus _bus;

        public InquiryContactCommandHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(InquiryContactCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return request.ValidationResult;

            var result = await _bus.RequestAsync<QueryContactByPhoneCodeIntegrationEvent, QueryContactResponse>(new QueryContactByPhoneCodeIntegrationEvent
            {
                PhoneCode = request.PhoneCode,
            });

            return ValidationResult;
        }
    }
}
