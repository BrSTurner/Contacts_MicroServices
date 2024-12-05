using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.UoW;
using MassTransit;
using FluentValidation.Results;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class PersistContactConsumer : IConsumer<CreateContactIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public PersistContactConsumer(
            IContactRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<CreateContactIntegrationEvent> context)
        {
            var command = context.Message;

            if (command == null || command?.Contact == null)
                throw new ArgumentNullException(nameof(command));

            _repository.Add(command.Contact);

            var result = await _unitOfWork.CommitAsync();
            var validationResult = new FluentValidation.Results.ValidationResult();

            if (!result)
                validationResult.Errors.Add(new ValidationFailure("Contact", "Something went wrong creating the Contact"));

            await context.RespondAsync(new ResponseMessage
            {
                ValidationResult = validationResult
            });
        }
    }
}
