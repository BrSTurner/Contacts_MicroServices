using FluentValidation.Results;
using MediatR;
using System;

namespace FIAP.SharedKernel.Messages
{
    public abstract class CommandResult<T> : Message, IRequest<T>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected CommandResult()
        {
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
