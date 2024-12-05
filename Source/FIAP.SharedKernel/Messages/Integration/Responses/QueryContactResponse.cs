using FIAP.SharedKernel.Entities;

namespace FIAP.SharedKernel.Messages.Integration.Responses
{
    public class QueryContactResponse : ResponseMessage
    {
        public Contact? Contact { get; set; }
    }
}
