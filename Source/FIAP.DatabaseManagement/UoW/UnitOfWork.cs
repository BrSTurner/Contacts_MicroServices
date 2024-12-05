using FIAP.DatabaseManagement.Context;
using FIAP.SharedKernel.UoW;

namespace FIAP.DatabaseManagement.UoW
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FIAPContext _context;

        public UnitOfWork(FIAPContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync() => await _context.SaveChangesAsync() > 0;

        public void Dispose() => _context?.Dispose();
    }
}
