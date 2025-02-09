namespace FIAP.SharedKernel.UoW
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
