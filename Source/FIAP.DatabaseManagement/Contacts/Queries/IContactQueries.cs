using FIAP.SharedKernel.DTO;

namespace FIAP.DatabaseManagement.Contacts.Queries
{
    public interface IContactQueries
    {
        Task<List<ContactDTO>> GetByPhoneCodeAsync(int phoneCode);
    }
}
