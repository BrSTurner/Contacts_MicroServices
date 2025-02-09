using FIAP.SharedKernel.Entities;

namespace FIAP.SharedKernel.Messages.Integration.Events
{
    public class DeleteContactIntegrationEvent : IntegrationEvent
    {
        public Guid ContactId { get; init; }
        public Contact Contact { get; set; }
    }
}
