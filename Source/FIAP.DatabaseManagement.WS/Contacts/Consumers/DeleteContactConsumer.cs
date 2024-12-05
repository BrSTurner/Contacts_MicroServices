using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.UoW;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class DeleteContactConsumer : IConsumer<DeleteContactIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteContactConsumer(
            IContactRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<DeleteContactIntegrationEvent> context)
        {
            var message = context.Message;
            var contact = await _repository.GetByIdAsync(message.ContactId);

            if (contact == null)
                return;

            _repository.Remove(contact);
            await _unitOfWork.CommitAsync();
        }
    }
}
