using FIAP.SharedKernel.Entities;

namespace FIAP.SharedKernel.Messages.Integration.Events
{
    public class UpdateContactIntegrationEvent : IntegrationEvent
    {
        public Guid ContactId { get; init; }
        public Contact Contact { get; init; }
    }
}
