using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class GetAllContactsConsumer : IConsumer<QueryAllContactsIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        public GetAllContactsConsumer(
            IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<QueryAllContactsIntegrationEvent> context)
        {
            var command = context.Message;

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var contacs = _repository.GetAllAsync();

            await context.RespondAsync(new QueryContactsResponse
            {
                Contacts = contacs.Result,
            });
        }
    }
}
