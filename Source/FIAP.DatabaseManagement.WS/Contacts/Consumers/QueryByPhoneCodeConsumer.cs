using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class QueryByPhoneCodeConsumer : IConsumer<QueryContactByPhoneCodeIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        public QueryByPhoneCodeConsumer(
            IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<QueryContactByPhoneCodeIntegrationEvent> context)
        {
            var message = context.Message;
            var contacts = await _repository.GetByPhoneCode(message.PhoneCode);
            await context.RespondAsync(new QueryContactsResponse
            {
                Contacts = contacts,
            });
        }
    }
}
