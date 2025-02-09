using FIAP.DatabaseManagement.Contacts.Queries;
using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.SharedKernel.DTO;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MassTransit;

namespace FIAP.DatabaseManagement.WS.Contacts.Consumers
{
    public class QueryByPhoneCodeConsumer : IConsumer<QueryContactByPhoneCodeIntegrationEvent>
    {
        private readonly IContactRepository _contactQueries;
        public QueryByPhoneCodeConsumer(
            IContactRepository contactQueries)
        {
            _contactQueries = contactQueries;
        }

        public async Task Consume(ConsumeContext<QueryContactByPhoneCodeIntegrationEvent> context)
        {
            var message = context.Message;
            var contacts = await _contactQueries.GetByPhoneCode(message.PhoneCode);
            await context.RespondAsync(new QueryContactByPhoneCodeResponse
            {
                Contacts = contacts.Select(x => new ContactDTO
                {
                    Email = x.Email.Address,
                    PhoneCode = x.PhoneNumber.Code,
                    PhoneNumber = x.PhoneNumber.Number,
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToList()
            });
        }
    }
}
