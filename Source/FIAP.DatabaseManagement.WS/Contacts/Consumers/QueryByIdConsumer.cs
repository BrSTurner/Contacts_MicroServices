using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class QueryByIdConsumer : IConsumer<QueryContactByIdIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        public QueryByIdConsumer(
            IContactRepository repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<QueryContactByIdIntegrationEvent> context)
        {
            var message = context.Message;
            var contact = await _repository.GetByIdAsync(message.ContactId);
            await context.RespondAsync(new QueryContactResponse
            {
                Contact = contact,
            });
        }
    }
}
