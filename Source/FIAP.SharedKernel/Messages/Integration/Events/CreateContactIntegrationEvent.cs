using FIAP.SharedKernel.Entities;

namespace FIAP.SharedKernel.Messages.Integration.Events
{
    public class CreateContactIntegrationEvent : IntegrationEvent
    {
        public Contact Contact { get; init; }

        public CreateContactIntegrationEvent(Contact contact)
        {
            Contact = contact;
        }
    }
}
