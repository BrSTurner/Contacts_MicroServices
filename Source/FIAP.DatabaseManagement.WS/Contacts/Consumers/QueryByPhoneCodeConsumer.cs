using FIAP.DatabaseManagement.Contacts.Queries;
using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class QueryByPhoneCodeConsumer : IConsumer<QueryContactByPhoneCodeIntegrationEvent>
    {
        private readonly IContactQueries _contactQueries;
        public QueryByPhoneCodeConsumer(
            IContactQueries contactQueries)
        {
            _contactQueries = contactQueries;
        }

        public async Task Consume(ConsumeContext<QueryContactByPhoneCodeIntegrationEvent> context)
        {
            var message = context.Message;
            var contacts = await _contactQueries.GetByPhoneCodeAsync(message.PhoneCode);
            await context.RespondAsync(new QueryContactByPhoneCodeResponse
            {
                Contacts = contacts,
            });
        }
    }
}
