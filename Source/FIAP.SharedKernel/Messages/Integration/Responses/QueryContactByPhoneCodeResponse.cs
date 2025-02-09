using FIAP.SharedKernel.DTO;

namespace FIAP.SharedKernel.Messages.Integration.Responses
{
    public class QueryContactByPhoneCodeResponse : ResponseMessage
    {
        public List<ContactDTO> Contacts { get; set; } = new List<ContactDTO>();
    }

}
