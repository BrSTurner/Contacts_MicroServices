using Dapper;
using FIAP.DatabaseManagement.Context;
using FIAP.SharedKernel.DTO;

namespace FIAP.DatabaseManagement.Contacts.Queries
{
    public sealed class ContactQueries : IContactQueries
    {
        private readonly FIAPContext _context;

        public ContactQueries(FIAPContext context)
        {
            _context = context;
        }

        public async Task<List<ContactDTO>> GetByPhoneCodeAsync(int phoneCode)
        {
            var sql = "SELECT * FROM Contacts WHERE PhoneCode = @PhoneCode";

            var result = await _context.DbConnection.QueryAsync<ContactDTO>(sql, new { PhoneCode = phoneCode });

            return result.ToList();
        }
    }
}
