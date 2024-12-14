using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using FIAP.SharedKernel.UoW;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class GetAllContactsConsumer : IConsumer<QueryAllContactsIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllContactsConsumer(
            IContactRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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
