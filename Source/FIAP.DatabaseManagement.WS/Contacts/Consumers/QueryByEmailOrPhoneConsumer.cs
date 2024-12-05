using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class QueryByEmailOrPhoneConsumer : IConsumer<QueryContactByEmailOrPhoneIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        public QueryByEmailOrPhoneConsumer(
            IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<QueryContactByEmailOrPhoneIntegrationEvent> context)
        {
            var message = context.Message;
            var contact  = await _repository.GetByEmailOrPhoneNumber(message.Email, message.PhoneCode, message.Phone);
            await context.RespondAsync(new QueryContactResponse
            {
                Contact = contact,
            });
        }
    }
}
