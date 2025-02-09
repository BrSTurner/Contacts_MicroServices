using FIAP.SharedKernel.DomainObjects;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.Repositories;

namespace FIAP.DatabaseManagement.Contacts.Repositories
{
    public interface IContactRepository : IBaseRepository<Contact>
    {
        Task<Contact?> GetByEmailOrPhoneNumber(string email, int phoneCode, string phoneNumber);

        Task<List<Contact>?> GetByPhoneCode(int phoneCode);
    }
}
