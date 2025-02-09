namespace FIAP.SharedKernel.Messages.Integration.Events
{
    public class QueryContactByEmailOrPhoneIntegrationEvent : IntegrationEvent
    {
        public required string Email { get; init; }
        public required int PhoneCode { get; init; }
        public required string Phone { get; init; }
    }
}
