using FIAP.SharedKernel.Messages;
using FluentValidation.Results;

namespace FIAP.SharedKernel.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
        Task<TResult> SendCommand<TRequest, TResult>(TRequest command) where TRequest : CommandResult<TResult>;
    }
}
