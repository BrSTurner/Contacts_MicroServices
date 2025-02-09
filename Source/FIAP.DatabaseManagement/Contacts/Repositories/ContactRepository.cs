using FIAP.DatabaseManagement.Context;
using FIAP.DatabaseManagement.Repositories;
using FIAP.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.DatabaseManagement.Contacts.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(FIAPContext context) : base(context)
        {

        }

        public Task<Contact?> GetByEmailOrPhoneNumber(string email, int phoneCode, string phoneNumber)
        {
            var teste = _entity.FirstOrDefaultAsync(x =>
                x.Email.Address.Equals(email) ||
                x.PhoneNumber.Code.Equals(phoneCode) &&
                x.PhoneNumber.Number.Equals(phoneNumber));
            return teste;
        }

        public Task<List<Contact>?> GetByPhoneCode(int phoneCode)
        {
            var teste = _entity.Where(x =>
               x.PhoneNumber.Code.Equals(phoneCode)).ToListAsync();
            return teste;
        }
    }
}
