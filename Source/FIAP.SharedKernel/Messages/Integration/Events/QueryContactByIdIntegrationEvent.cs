namespace FIAP.SharedKernel.Messages.Integration.Events
{
    public class QueryContactByIdIntegrationEvent : IntegrationEvent
    {
        public Guid ContactId { get; set; }
    }
}
