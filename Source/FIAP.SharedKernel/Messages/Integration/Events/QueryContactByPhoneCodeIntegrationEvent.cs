namespace FIAP.SharedKernel.Messages.Integration.Events
{
    public class QueryContactByPhoneCodeIntegrationEvent : IntegrationEvent
    {
        public required int PhoneCode { get; init; }
    }
}
