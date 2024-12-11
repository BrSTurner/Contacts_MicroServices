using FIAP.SharedKernel.Entities;

namespace FIAP.SharedKernel.Messages.Integration.Responses
{
    public class QueryContactsResponse : ResponseMessage
    {
        public List<Contact?> Contacts { get; set; }
    }
}
