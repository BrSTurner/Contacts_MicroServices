using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.Messages.Integration;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.UoW;
using FluentValidation.Results;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class UpdateContactConsumer : IConsumer<UpdateContactIntegrationEvent>
    {
        private readonly IContactRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateContactConsumer(
            IContactRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UpdateContactIntegrationEvent> context)
        {
            var command = context.Message;

            if (command == null || command?.Contact == null)
                throw new ArgumentNullException(nameof(command));

            _repository.Update(command.Contact);

            var result = await _unitOfWork.CommitAsync();
            var validationResult = new FluentValidation.Results.ValidationResult();

            if (!result)
                validationResult.Errors.Add(new ValidationFailure("Contact", "Something went wrong updating the Contact"));

            await context.RespondAsync(new ResponseMessage
            {
                ValidationResult = validationResult
            });
        }
    }
}
