namespace FIAP.SharedKernel.Messages.Integration.Events
{
    public class DeleteContactIntegrationEvent : IntegrationEvent
    {
        public Guid ContactId { get; init; }
    }
}
